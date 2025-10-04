using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

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

    private CameraState _currentCameraState = CameraState.FollowPlayer;
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
       _CameraSwitch=_cameraState[CameraState.FollowPlayer];
    }

    // Update is called once per frame
    void Update()
    {
        //TODO:抖动效果
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
            Player = player.transform;  
        }

        public  StateCameraSwitch() { }
        public virtual void Enter() { }
        public virtual void Enter(Transform vector3) { }
        public virtual void Enter(Action action) { }

        public virtual void Update() { }
       

        public virtual void Quit(){ }

        public float CameraZ = -10f;
        protected Camera mainCamera;
        protected Transform Player;
    }
     class FollowCamera : StateCameraSwitch
    {
        public FollowCamera(Camera camera,GameObject player):base(camera,player)
        {
        
        }
      
        public override void Enter()
        {
            Debug.Log("进入跟随模式");
        }
        public override void Update()
        {
            //简单的跟随

            Vector3 targetPosition = Player.position;
            targetPosition.z = CameraZ;
            mainCamera.transform.position = targetPosition;
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
}
