using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance
    {
        get {  return _instance; }
        private set { }
        
    }

    private enum CameraState
    {
    FollowPlayer,
    FixedPosition,
    Other
    }

    private CameraState _currentCameraState = CameraState.Other;
    Dictionary<CameraState, StateCameraSwitch> _cameraState = new Dictionary<CameraState, StateCameraSwitch>();
     StateCameraSwitch _CameraSwitch;
    public Camera mainCamera;
    public GameObject Player;

    // 添加边界变量
    [Header("相机边界设置")]
  [SerializeField]  public Rect currentBounds = new Rect(0, 0, 10, 10); // 默认边界
    Vector3 clampedBasePos;   
    // Start is called before the first frame update
    private void Awake()
    { 
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        _cameraState[CameraState.FollowPlayer] = new FollowCamera(mainCamera, Player);
        _cameraState[CameraState.FixedPosition]= new FixedCamera(mainCamera, Player);
        _cameraState[CameraState.Other]= new OtherCamera(mainCamera, Player);

    }

    void Start()
    {
       
        SwitchFollowCamera();
        
    }

    // Update is called once per frame
    void Update()
    {
     
        _CameraSwitch.Update();
    }
    private void FixedUpdate()
    {
        
    }
    // 设置边界的方法
    public void SetCameraBounds(Rect newBounds)
    {
        currentBounds = newBounds;
        // 立即应用新的边界限制
        if (_currentCameraState == CameraState.FollowPlayer)
        {
            ClampCameraToBounds();
        }
    }

    // 边界限制的公共方法，可供外部调用
    public void ClampCameraToBounds()
    {
        if (_cameraState.ContainsKey(_currentCameraState))
        {
            _cameraState[_currentCameraState].ClampToBounds(currentBounds);
        }
    }
    public void CameraShake(float durTime, float power)
    {
        if (mainCamera == null) return;
        switch (_currentCameraState)
        {
            case CameraState.FollowPlayer:
                DeadAreaShake(durTime, power);
            break;
              
            case CameraState.FixedPosition:
                FixedAreaShake(durTime, power);
            break;
        }
    }
    //基于死区移动摄像机的抖动
    public void DeadAreaShake(float durTime, float power)
    {
        if (Player == null) return;

     
        // 先让边界脚本把相机一次性钳到合法位置
        ClampCameraToBounds();
        // 记录“已被钳过”的坐标作为震动基准
        clampedBasePos = mainCamera.transform.position;
        // ----------------------------------

        float timer = 0;
        TimeManager.Instance.FrameTime(durTime,
            () =>
            {
                timer += Time.deltaTime;
                float progress = timer / durTime;
                float currentPower = power * (1f - progress);

                // 生成随机偏移
                float x = UnityEngine.Random.Range(-1f, 1f) * currentPower;
                float y = UnityEngine.Random.Range(-1f, 1f) * currentPower;

                // 直接在“已钳位”基准上偏移，不再 Lerp 到玩家
                mainCamera.transform.position = clampedBasePos + new Vector3(x, y, 0f);
            },
            () =>
            {
                // 震动结束把相机还给跟随逻辑
                mainCamera.transform.position = clampedBasePos;
                _cameraState[_currentCameraState].Reset();
            }
        );
    }

    //基于固定摄像机的抖动
    public void FixedAreaShake( float durTime, float power)
    {
        float timer = 0;
        Vector3 originalPos = mainCamera.transform.position; // 记录震动开始时的摄像机位置

        TimeManager.Instance.FrameTime(durTime,
            () =>
            {
                timer += Time.deltaTime;
                float progress = timer / durTime;

                // 计算当前帧的抖动强度（随时间衰减）
                float currentPower = power * (1f - progress);

                // 生成随机偏移（Vector2）
                float x = UnityEngine.Random.Range(-1f, 1f) * currentPower;
                float y = UnityEngine.Random.Range(-1f, 1f) * currentPower;
                Vector2 shakeOffset = new Vector2(x, y);

                // 以原始摄像机位置为中心应用抖动
                mainCamera.transform.position = new Vector3(
                    originalPos.x + shakeOffset.x,
                    originalPos.y + shakeOffset.y,
                    originalPos.z
                );
            },
            () =>
            {
                // 震动结束后，回到原始位置
                mainCamera.transform.position = originalPos;

               
            }
        );
    }

    void SwitchFollowCamera()
    {
        if(_currentCameraState == CameraState.FollowPlayer) {return;}
        _CameraSwitch?.Quit();
        _currentCameraState= CameraState.FollowPlayer;
        _CameraSwitch=_cameraState[CameraState.FollowPlayer];
        _CameraSwitch.ClampToBounds(currentBounds);
        _CameraSwitch.Enter();
    }
    void SwitchFixedCamera(Transform vector3)
    {
        
        _CameraSwitch?.Quit();
        _currentCameraState = CameraState.FixedPosition;
        _CameraSwitch = _cameraState[CameraState.FixedPosition];
        _CameraSwitch.Enter(vector3);
    }
    void SwitchCustomCamera(Action action)
    {
        _CameraSwitch?.Quit();
        _currentCameraState = CameraState.Other;
        _CameraSwitch = _cameraState[CameraState.Other];
        _CameraSwitch.Enter(action);
    }


   class StateCameraSwitch
    {
        
        public StateCameraSwitch(Camera camera,GameObject player)
        {
            mainCamera = camera;
            PlayerTransform = player.transform;  
        }

        public  StateCameraSwitch() { }
        public virtual void Enter() { }
        public virtual void Enter(Transform vector3) { }
        public virtual void Enter(Action action) { }

        public virtual void Update() { }

        public virtual void Reset() { }

        public virtual void Quit(){ }

        // 添加虚方法用于边界限制
        public virtual void ClampToBounds(Rect bounds) { }

        // 计算相机视口边界的方法
        protected Rect GetCameraViewportWorldBounds()
        {
            if (mainCamera == null) return new Rect();

            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, -CameraZ));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, -CameraZ));

            return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        }

        public float CameraZ = -10f;
        protected Camera mainCamera;
        protected Transform PlayerTransform;
        // 添加边界相关的保护字段
        protected Rect _currentBounds;
        protected bool _hasBounds = false;
    }
    class FollowCamera : StateCameraSwitch
    {
        private float rangeX = 1.3f;
        private float rangeY = 1.3f;
        Vector3 PlayerLastPos = new Vector3();
        float moveX = 0; float t_moveX;
        float moveY = 0; float t_moveY;
        bool HaveOneTime = true;

        private float leftBoundary;   // 相机可见区域左0.4世界坐标
        private float rightBoundary;  // 相机可见区域右0.6世界坐标
        private float bottomBoundary; // 相机可见区域下0.4世界坐标
        private float topBoundary;    // 相机可见区域上0.6世界坐标

        private bool playerBeyondCenterX; // 玩家超出X轴中央区域(在0.4或0.6边界外)
        private bool playerBeyondCenterY; // 玩家超出Y轴中央区域(在0.4或0.6边界外)

        // 新增：记录玩家在边缘时的移动方向
        private bool playerMovingAwayFromCenterX = false;
        private bool playerMovingAwayFromCenterY = false;
        public FollowCamera(Camera camera, GameObject player) : base(camera, player)
        {
        }

        public override void Enter()
        {
            Debug.Log("进入跟随模式");
            PlayerLastPos = PlayerTransform.position;

            // 先设置摄像头位置
            Vector3 targetPosition = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, 0);
            targetPosition.z = CameraZ;
            mainCamera.transform.position = targetPosition;

            // 应用边界限制
            if (_hasBounds)
            {
                ClampCameraPosition();
            }

            HaveOneTime = true;
        }

        void RecalcBoundaries()
        {
            Rect cameraViewport = GetCameraViewportWorldBounds();
            float cameraWidth = cameraViewport.width;
            float cameraHeight = cameraViewport.height;

            // 计算屏幕0.4和0.6的世界坐标
            Vector3 cameraPos = mainCamera.transform.position;
            leftBoundary = cameraPos.x - (cameraWidth * 0.1f);   // 中心-0.1*width = 0.4位置
            rightBoundary = cameraPos.x + (cameraWidth * 0.1f);  // 中心+0.1*width = 0.6位置
            bottomBoundary = cameraPos.y - (cameraHeight * 0.1f); // 中心-0.1*height = 0.4位置
            topBoundary = cameraPos.y + (cameraHeight * 0.1f);    // 中心+0.1*height = 0.6位置
        }

        public override void Update()
        {
            float px = PlayerTransform.position.x;
            float py = PlayerTransform.position.y;

            RecalcBoundaries();
            // 玩家是否超出中央区域 (在0.4或0.6边界外)
            playerBeyondCenterX = px <= leftBoundary || px >= rightBoundary;
            playerBeyondCenterY = py <= bottomBoundary || py >= topBoundary;

            // 基于死区 若超过框的范围则丝滑跟随
            float tempX = PlayerTransform.position.x - PlayerLastPos.x;
            float tempY = PlayerTransform.position.y - PlayerLastPos.y;
            t_moveX = moveX;
            t_moveY = moveY;
            moveX += tempX;
            moveY += tempY;

            // X轴：只有当玩家超出中央0.4-0.6区域时才移动相机
            if (playerBeyondCenterX)
            {
                // 计算玩家相对于相机中心的位置
                float cameraCenterX = mainCamera.transform.position.x;
                float playerToCenter = px - cameraCenterX;

                // 判断玩家是朝屏幕中心移动还是远离屏幕中心
                if (Mathf.Abs(tempX) > 0.01f) // 确保有移动
                {
                    // 玩家在左边界(<= leftBoundary)且向左移动 = 远离中心
                    // 玩家在左边界(<= leftBoundary)且向右移动 = 朝向中心
                    // 玩家在右边界(>= rightBoundary)且向右移动 = 远离中心
                    // 玩家在右边界(>= rightBoundary)且向左移动 = 朝向中心

                    if ((px <= leftBoundary && tempX < 0) || (px >= rightBoundary && tempX > 0))
                    {
                        // 玩家朝远离中心方向移动，相机必须跟随
                        mainCamera.transform.position += new Vector3(tempX, 0, 0);
                        playerMovingAwayFromCenterX = true;
                    }
                    else
                    {
                        // 玩家朝中心方向移动，相机可以不动，等待玩家回到0.4~0.6区域
                        mainCamera.transform.position += new Vector3(0, 0, 0);
                        playerMovingAwayFromCenterX = false;
                    }

                    // 重置死区累积
                    moveX = 0;
                }
                else
                {
                    // 玩家在边界但没有移动
                    mainCamera.transform.position += new Vector3(0, 0, 0);
                    playerMovingAwayFromCenterX = false;
                }
            }
            else if (moveX > rangeX || moveX < -rangeX)
            {
                // 正常死区逻辑 - 当玩家在中央区域时，累积移动直到超过死区才移动相机
                mainCamera.transform.position += new Vector3(tempX, 0, 0);
                moveX = t_moveX;
                playerMovingAwayFromCenterX = false;
            }

            // Y轴：只有当玩家超出中央0.4-0.6区域时才移动相机
            if (playerBeyondCenterY)
            {
                // 计算玩家相对于相机中心的位置
                float cameraCenterY = mainCamera.transform.position.y;
                float playerToCenter = py - cameraCenterY;

                // 判断玩家是朝屏幕中心移动还是远离屏幕中心
                if (Mathf.Abs(tempY) > 0.01f) // 确保有移动
                {
                    // 玩家在下边界(<= bottomBoundary)且向下移动 = 远离中心
                    // 玩家在下边界(<= bottomBoundary)且向上移动 = 朝向中心
                    // 玩家在上边界(>= topBoundary)且向上移动 = 远离中心
                    // 玩家在上边界(>= topBoundary)且向下移动 = 朝向中心

                    if ((py <= bottomBoundary && tempY < 0) || (py >= topBoundary && tempY > 0))
                    {
                        // 玩家朝远离中心方向移动，相机必须跟随
                        mainCamera.transform.position += new Vector3(0, tempY, 0);
                        playerMovingAwayFromCenterY = true;
                    }
                    else
                    {
                        // 玩家朝中心方向移动，相机可以不动，等待玩家回到0.4~0.6区域
                        mainCamera.transform.position += new Vector3(0, 0, 0);
                        playerMovingAwayFromCenterY = false;
                    }

                    // 重置死区累积
                    moveY = 0;
                }
                else
                {
                    // 玩家在边界但没有移动
                    mainCamera.transform.position += new Vector3(0, 0, 0);
                    playerMovingAwayFromCenterY = false;
                }
            }
            else if (moveY > rangeY || moveY < -rangeY)
            {
                // 正常死区逻辑 - 当玩家在中央区域时，累积移动直到超过死区才移动相机
                mainCamera.transform.position += new Vector3(0, tempY, 0);
                moveY = t_moveY;
                playerMovingAwayFromCenterY = false;
            }

            // 应用边界限制
            if (_hasBounds)
            {
                ClampCameraPosition();
            }

            // 记录上一帧位置
            PlayerLastPos = PlayerTransform.position;
        }


        // 实现边界限制方法
        public override void ClampToBounds(Rect bounds)
        {
            _currentBounds = bounds;
            _hasBounds = true;
            ClampCameraPosition(); // 立即应用新的边界
        }

        // 限制相机位置在边界内
        private void ClampCameraPosition()
        {
            if (mainCamera == null) return;

            // 获取相机视口的世界坐标边界
            Rect cameraViewport = GetCameraViewportWorldBounds();
            float cameraWidth = cameraViewport.width;
            float cameraHeight = cameraViewport.height;

            // 计算相机允许移动的范围
            float minX = _currentBounds.x + cameraWidth * 0.5f;
            float maxX = _currentBounds.x + _currentBounds.width - cameraWidth * 0.5f;
            float minY = _currentBounds.y + cameraHeight * 0.5f;
            float maxY = _currentBounds.y + _currentBounds.height - cameraHeight * 0.5f;

            // 如果房间比相机视口小，就让相机居中
            if (_currentBounds.width < cameraWidth)
            {
                minX = maxX = _currentBounds.x + _currentBounds.width * 0.5f;
            }
            if (_currentBounds.height < cameraHeight)
            {
                minY = maxY = _currentBounds.y + _currentBounds.height * 0.5f;
            }

            // 钳制相机位置
            Vector3 currentPos = mainCamera.transform.position;
            float clampedX = Mathf.Clamp(currentPos.x, minX, maxX);
            float clampedY = Mathf.Clamp(currentPos.y, minY, maxY);

            mainCamera.transform.position = new Vector3(clampedX, clampedY, currentPos.z);
        }
        public override void Reset()
        {
            moveX = 0;
            moveY = 0;
            playerMovingAwayFromCenterX = false;
            playerMovingAwayFromCenterY = false;
        }

        public override void Quit()
        {
            Debug.Log("退出跟随模式");
        }
    }




    class FixedCamera : StateCameraSwitch
    {
        public FixedCamera(Camera camera, GameObject player) : base(camera, player)
        {

        }
        public Transform Pos;
        public override void Enter(Transform vector3)
        {
            Debug.Log("进入固定视角");
            Pos = vector3;
            mainCamera.transform.position = Pos.position;
        }
        public override void Update()
        {
           // mainCamera.transform.position = Pos;
        }
        public override void Quit()
        {
           
            Debug.Log("退出固定视觉模式");
        }
    }
    class OtherCamera:StateCameraSwitch
    {
        public OtherCamera(Camera camera, GameObject player) : base(camera, player)
        {

        }
        Action camermAction;
        public override void Enter(Action action)
        {
            Debug.Log("进入自定义视觉模式");
            camermAction = action;
        }
        public override void Update()
        { 
            camermAction?.Invoke();
        }
        public override void Quit()
        {
            camermAction=null;
            Debug.Log("退出自定义模式");
        }
    }

    //----------------------------
    [Header("边界可视化")]
    public bool showBoundsInScene = true;
    public Color boundsColor = Color.green;
    public Color cameraViewColor = Color.cyan;

    // 添加调试绘制
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!showBoundsInScene) return;

        // 绘制当前边界
        DrawBoundsGizmo(currentBounds, boundsColor);

        // 绘制相机视口
        DrawCameraViewportGizmo();
    }

    private void OnDrawGizmosSelected()
    {
        if (!showBoundsInScene) return;

        // 选中时绘制更明显的边界
        DrawBoundsGizmo(currentBounds, Color.yellow, 3f);
    }

    private void DrawBoundsGizmo(Rect bounds, Color color, float thickness = 2f)
    {
        Gizmos.color = color;

        Vector3 bottomLeft = new Vector3(bounds.x, bounds.y, 0);
        Vector3 bottomRight = new Vector3(bounds.x + bounds.width, bounds.y, 0);
        Vector3 topLeft = new Vector3(bounds.x, bounds.y + bounds.height, 0);
        Vector3 topRight = new Vector3(bounds.x + bounds.width, bounds.y + bounds.height, 0);

        // 绘制边界框
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);

        // 绘制对角线（可选，帮助看清中心）
        Gizmos.DrawLine(bottomLeft, topRight);
        Gizmos.DrawLine(bottomRight, topLeft);

        // 绘制中心点
        Vector3 center = new Vector3(bounds.center.x, bounds.center.y, 0);
        Gizmos.DrawWireSphere(center, 0.5f);
    }

    private void DrawCameraViewportGizmo()
    {
        if (mainCamera == null) return;

        Gizmos.color = cameraViewColor;

        // 获取相机视口的世界坐标边界
        Rect viewportBounds = GetCurrentCameraViewportBounds();

        Vector3 bottomLeft = new Vector3(viewportBounds.x, viewportBounds.y, 0);
        Vector3 bottomRight = new Vector3(viewportBounds.x + viewportBounds.width, viewportBounds.y, 0);
        Vector3 topLeft = new Vector3(viewportBounds.x, viewportBounds.y + viewportBounds.height, 0);
        Vector3 topRight = new Vector3(viewportBounds.x + viewportBounds.width, viewportBounds.y + viewportBounds.height, 0);

        // 绘制相机视口框
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }

    private Rect GetCurrentCameraViewportBounds()
    {
        if (mainCamera == null) return new Rect();

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, -mainCamera.transform.position.z));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, -mainCamera.transform.position.z));

        return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }
#endif
}
