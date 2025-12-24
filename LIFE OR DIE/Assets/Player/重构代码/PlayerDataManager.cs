using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//[RequireComponent(typeof(Player_checkGround))]
public class PlayerDataManager :MonoBehaviour
{
    [Header("基础数据")]
    public int currentHP=1;
    //----
  

    [Tooltip("玩家速度/ 地面摩擦力")]
    public readonly float moveSpeed = 6f;
    public readonly float jumpSpeed = 10f;
    public readonly float groundFriction = 40f;
   
    //----



    [Tooltip("地面检测")]
    private Player_checkGround _checkGround;
    public bool isGround { get { return _checkGround.IsGrounded; } }
    //------------------------------------



    //-----------背刺相关
    [SerializeField] private float rayLength = 1.5f;
    public float 背刺射线长度 { get { return rayLength; } }
  

    //--------------------

    [Tooltip("坠落的最大速度")]
  [SerializeField] float maxFallSpeed = 30f;         // 坠落速度上限
  [SerializeField] float landingSizeFactor = 0.3f; // 速度→尺寸放大系数
  [SerializeField] float _fallDamagePerSecon = 200;//根据时间坠落伤害
    [SerializeField] public float flySpeed = 10f;
   public const float MaxDamage = 3000;
    [Tooltip("下落距离→伤害曲线  5 m=0  30 m=3000")]
    [SerializeField]
    private AnimationCurve fallDistanceToDamage = new AnimationCurve(
     new Keyframe(5, 0),
     new Keyframe(7, 100),
     new Keyframe(10, 200),
     new Keyframe(15, 800),
     new Keyframe(25, 1000),
     new Keyframe(30, 3000)
 );
  //"下落距离→相机震动强度"
     private AnimationCurve fallDistanceToShake = new AnimationCurve(
    new Keyframe(5, 0),
    new Keyframe(15, 0.5f),
    new Keyframe(20, 1f)
);
    public float EvaluateFallDistanceDamage(float distance) =>
        fallDistanceToDamage.Evaluate(Mathf.Clamp(distance, 5f, 30f));
    public float EvaluateFallDistanceShake(float distance)
        => fallDistanceToShake.Evaluate(Mathf.Clamp(distance, 5f, 30f));


    public float LandingSizeFactor { get { return landingSizeFactor; } }
     public float MaxFallSpeed {  get { return maxFallSpeed; } }
    public float FallDamagePerSecon { get { return _fallDamagePerSecon; } }


    //冲刺CD为1秒 同时冲刺所需要的时间为<=0.4s---------------
    [Tooltip("冲刺相关")]
    [SerializeField] private readonly float sprintCD = 0.8f;
    [SerializeField] public readonly float sprintSpeed = 20f;
    [SerializeField] private float _LastpressSprintKey = 0;
    

    //冲刺条件判断 
    public bool isDodgeTimeReady(float currentTime)
    {
        if (currentTime - _LastpressSprintKey >= sprintCD)
        {
            _LastpressSprintKey=currentTime;
            return true;
        }
        else { return false; }
    }
    public float GetDodgeSpeed()
    {
        return sprintSpeed;
    }
    //--------------------------------------------

    [Tooltip("无敌帧相关")]
    [SerializeField] private float InvincibleFrameTime = 1.2f;
    [SerializeField] private bool isInvincible = false;
    public bool IsInvincible {  get { return isInvincible; } }
    public void StartInvincible()
    {
        isInvincible = true;
        TimeManager.Instance.OneTime(InvincibleFrameTime, () => {isInvincible = false;});   
    }
    public void SetInvencibleAndStart(float timer)
    {
        isInvincible=true;
        TimeManager.Instance.OneTime(timer, () => { isInvincible = false; });
    }
    public void SetInvencible(bool i)
    {
        isInvincible = i;
    }
    //----------------------------------------------------
    //土狼跳跃相关参数
    [Tooltip("土狼相关")]
    [SerializeField] private float lastGroundtime = 0;
    //土狼允许的滞空时间
    [SerializeField] public float coyoteTime = 0.15f;
    public bool canJump
    {
        get
        {
            if(Time.time-lastGroundtime <= coyoteTime)return true;
            else return false;
        }
    }
    private void updateCoyoteData()
    {
        if (isGround)
        {
            lastGroundtime=Time.time;
        }
    }
    //碰撞相关---------------------------
    [Tooltip("碰撞相关")]
 [SerializeField]  private GameObject currentCollOBJ;
    public GameObject CurrentObj {  get { return currentCollOBJ; } }
   public void  isCollisionOBJ(GameObject obj)
    {
        currentCollOBJ = obj;
    }


    public void isExitOBJ(GameObject obj)
    {
        if(currentCollOBJ == obj) currentCollOBJ = null;
    }
    //格挡弹反相关------------------
    [Header("弹反相关")]
    [SerializeField] private float 完美格挡时间 = 0.2f;
    public bool isPressBlock = false;

    public bool isPerfectBlock =false;
    public float PerfectBlock { get { return 完美格挡时间; } }


    //死亡受伤相关-----------------------
    private PlayerAddHPCondition hPCondition;
    public void MinusHP() => currentHP -= 1;
    public void AddHP() => currentHP += 1;

    [Header("奇点时间有关")]

    [SerializeField] private bool isInAmazingState=false;
   public bool IsInAmazingState { get { return isInAmazingState; } }
        private float AmazingTime = 0.3f;
     private float 濒死时间 = 10f;
    [SerializeField] public float AgonalTime { get {  return 濒死时间; }  }
    //严格不允许body用作其他地方了
    private NewPlayerControll body;
    public void EnterAmazingTime()
    {
        if (isInAmazingState==true) return;
        isInAmazingState = true;

        TimeManager.Instance.OneTime(AmazingTime,
                () =>
                {
                    isInAmazingState=false;
                    body.改变玩家颜色(Color.white);
                }
                );
    }



    private void Awake()
    {
        _checkGround=this.transform.Find("checkGround").GetComponent<Player_checkGround>();
        hPCondition = new PlayerAddHPCondition(this);
        body=GetComponent<NewPlayerControll>();
        hPCondition.StartListen();
    }
    private void Update()
    {
        //土狼时间需要持续监听最后一次在地上的时间
        updateCoyoteData();
    }


    private void OnDestroy()
    {
        hPCondition.StopListen();
    }
}
