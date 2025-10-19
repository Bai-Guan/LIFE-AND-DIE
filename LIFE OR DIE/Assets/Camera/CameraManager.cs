using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
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
        //TODO:����Ч��
        _CameraSwitch.Update();
    }
    private void FixedUpdate()
    {
        
    }


    void SwitchFollowCamera()
    {
        if(_currentCameraState == CameraState.FollowPlayer) {return;}
        _CameraSwitch?.Quit();
        _currentCameraState= CameraState.FollowPlayer;
        _CameraSwitch=_cameraState[CameraState.FollowPlayer];
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
       

        public virtual void Quit(){ }

        public float CameraZ = -10f;
        protected Camera mainCamera;
        protected Transform PlayerTransform;
    }
     class FollowCamera : StateCameraSwitch
    {
        private float range = 1.3f;
        Vector3 PlayerLastPos = new Vector3();
        float moveX=0; float t_moveX;
        float moveY=0; float t_moveY;
        bool HaveOneTime = true;
        public FollowCamera(Camera camera,GameObject player):base(camera,player)
        {
        
        }
      
        public override void Enter()
        {
            Debug.Log("�������ģʽ");
            PlayerLastPos = PlayerTransform.position;

            //����������ͷλ��
            Vector3 targetPosition = new Vector3(PlayerTransform.transform.position.x, PlayerTransform.transform.position.y,0) ;
            targetPosition.z = CameraZ;
            mainCamera.transform.position = targetPosition;
            HaveOneTime = true;


            Debug.Log("���λ��"+PlayerTransform.position);
            Debug.Log("�������λ��" + mainCamera.transform.position);
        }
        public override void Update()
        {
           
            //�������� ��������ķ�Χ��˿������
            //�����x y����һ֡��ȵ�ƫ����
            float tempX = PlayerTransform.position.x - PlayerLastPos.x;
            float tempY = PlayerTransform.position.y - PlayerLastPos.y;
            t_moveX=moveX; 
            t_moveY=moveY;
            moveX += tempX;  
            moveY += tempY;
            
            if (moveX > range || moveX < -range)
            {
                mainCamera.transform.position += new Vector3(tempX, 0, 0);
                moveX=t_moveX;
            }
            if (moveY > range || moveY < -range)
            {
                mainCamera.transform.position += new Vector3(0,tempY, 0);
                moveY=t_moveY;
            }



            




            //��¼��һ֡λ��
            PlayerLastPos = PlayerTransform.position;
        }

        public override void Quit()
        {
            Debug.Log("�˳�����ģʽ");
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
            Debug.Log("����̶��ӽ�");
            Pos = vector3;
            mainCamera.transform.position = Pos.position;
        }
        public override void Update()
        {
           // mainCamera.transform.position = Pos;
        }
        public override void Quit()
        {
           
            Debug.Log("�˳��̶��Ӿ�ģʽ");
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
            Debug.Log("�����Զ����Ӿ�ģʽ");
            camermAction = action;
        }
        public override void Update()
        { 
            camermAction?.Invoke();
        }
        public override void Quit()
        {
            camermAction=null;
            Debug.Log("�˳��Զ���ģʽ");
        }
    }
}
