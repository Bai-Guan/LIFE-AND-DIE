using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(PlayerDataManager))]
public class NewPlayerControll : MonoBehaviour, IBeDamaged
{
    private FSM fSM;
    public int currentstate { get { return (int)fSM.curState; } }

    private PlayerInput playerInput;
    public PlayerInput PlayerInput { get { return playerInput; } }

    private PlayerDataManager dataMan;
    public PlayerDataManager DataMan { get { return dataMan; } }

    [HideInInspector] public PlayerAnimControl Anim;

    public InitWeaponSystem weapon;
    //输入处理
    private Vector2 _moveinputData;
    public float InputX { get { return _moveinputData.x; } }
    public float InputY { get { return _moveinputData.y; } }
    private bool lastFacing = false;
    public bool isFacingLeft
    {
        get
        {
            if (InputX < 0)
            {
                lastFacing = false;
                return true;
            }
            else if (InputX > 0)
            {
                lastFacing = true;
                return false;
            }
            else { return lastFacing; }
        }
    }

    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        InitComponent();
        fSM = new FSM();
        InitFSM();

        fSM.curState = TypeState.ldle;
        fSM.ICurrentState = fSM._dicTypeState[fSM.curState];
    }





    private void InitFSM()
    {
        fSM.AddState(TypeState.ldle, new IdleState(this));
        //if (fSM._dicTypeState.TryGetValue(TypeState.ldle, out IPlayerState temp))
        // {
        //     Debug.Log($"添加 IdleState: {temp}");
        // }
        //else
        // {
        //     Debug.LogWarning("Value为空");
        // }

        fSM.AddState(TypeState.run, new RunState(this));
        fSM.AddState(TypeState.fall, new FallState(this));
        fSM.AddState(TypeState.attack, new AttackState(this));
        fSM.AddState(TypeState.died, new DieState(this));
        fSM.AddState(TypeState.Unexpected, new playerSurprisedState(this));
        fSM.AddState(TypeState.jump, new JumpState(this));
        fSM.AddState(TypeState.sprint, new SprintState(this));
        fSM.AddState(TypeState.collision, new CollisionState(this));

        //_states[TypeState.ldle] = new IdleState();
        //_states[TypeState.run] = new RunState();
        //_states[TypeState.jump] = new JumpState();
        //_states[TypeState.fall] = new FallState(this);
        //_states[TypeState.other] = new OtherState(this);
        //_states[TypeState.sprint] = new SprintState(this);
        //_states[TypeState.Unexpected] = new UnexpectedState(this);
        //_states[TypeState.attack] = new AttackState(this);
        //_states[TypeState.block] = new BlockState(this);
        //_states[TypeState.died] = new DieState(this);
    }

    private void InitComponent()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        playerInput = this.GetComponent<PlayerInput>();
        dataMan = this.GetComponent<PlayerDataManager>();
        Anim = this.GetComponent<PlayerAnimControl>();
        rb = this.GetComponent<Rigidbody2D>();

        weapon = transform.Find("Weapon").GetComponent<InitWeaponSystem>();
    }
    private void Start()
    {

    }
    public void Update()
    {
        fSM.Update();
    }
    public void FixedUpdate()
    {
        fSM.FixedUpdate();
    }

    public void PressJump(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            _moveinputData.y = 1;

        }
        if (callback.canceled)
        {
            _moveinputData.y = 0;
        }
    }
    public void PressMove(InputAction.CallbackContext callback)
    {
        float h = callback.ReadValue<Vector2>().x;
        _moveinputData.x = h;
    }

    public void PressAttack(InputAction.CallbackContext callback)
    {
        //if(fSM.curState != TypeState.died && fSM.curState != TypeState.Unexpected)
        fSM.Attack();
    }

    public void PressDodge(InputAction.CallbackContext callback)
    {
        ////不为死亡状态 不为特殊状态 满足时间要求
        //if (dataMan.isDodgeTimeReady(Time.time) && fSM.curState != TypeState.died && fSM.curState != TypeState.Unexpected)
        //{ 
        fSM.Dodge();
        // }
    }
    public void PressResurgence(InputAction.CallbackContext callback)
    {
        fSM.ContractPower();
    }

    public void PressTask(InputAction.CallbackContext callback)
    {
        TaskManager.Instance.NormalOpenTaskUI();
        playerInput.SwitchCurrentActionMap("TaskUI");
    }
    public void CloseTask(InputAction.CallbackContext context)
    {
        TaskManager.Instance.QuitTaskUI();
        playerInput.SwitchCurrentActionMap("GamePlay");
    }


    public void PressPackage(InputAction.CallbackContext callback)
    {
        UIManager.Instance.OpenPanel(UIManager.UIConst.BackPack);
        playerInput.SwitchCurrentActionMap("Inventory");
    }
    public void ClosePackage(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClosePanel(UIManager.UIConst.BackPack, true);
        playerInput.SwitchCurrentActionMap("GamePlay");
    }


    //对话/交互
    public void Interaction(InputAction.CallbackContext context)
    {

        if (dataMan.CurrentObj != null && StackInteraction.Instance.Peek() == dataMan.CurrentObj && dataMan.CurrentObj.TryGetComponent<ObjChat>(out ObjChat temp))
        {

            StackInteraction.Instance.PopSomeOne(dataMan.CurrentObj);

            DialogManager.Instance.StartDialogue(temp.GetDialogue());

        }


    }

    public void SwitchState(TypeState typeState)
    {
        fSM.SwitchStatus(typeState);
    }


    //为状态机提供的公共方法-----------------------------------
    public void CheckFill()
    {

        //检测朝向
        if (InputX > 0.5f)
        {
            spriteRenderer.flipX = false;
        }
        if (InputX < -0.5f)
        {
            //Debug.LogWarning("向左了！");
            spriteRenderer.flipX = true;
        }
    }

    public void OnHurt(DamageData damage, GameObject obj)
    {
        //无敌状态,死亡状态,奇点状态 一律return
        if (dataMan.IsInvincible == true && fSM.curState == TypeState.died && fSM.curState == TypeState.Unexpected)
        {
            return;
        }
        //如果处于奇点状态 则切换为奇点模式

        SwitchState(TypeState.died);
    }

    public void 卡肉(float timer)
    {


        //创建限定攻击框()
        Rect temp = new Rect()
        {
            x = 0.19f,
            y = 0f,
            width = 2.59f,
            height = 2.2f

        };
        int flip = isFacingLeft ? -1 : 1;
        Vector2 center = (Vector2)transform.position + new Vector2(temp.x * flip, temp.y);
        Vector2 size = new Vector2(temp.width * flip, temp.height);   // ① 翻转宽度
        Vector2 farEnd = center + new Vector2(temp.width * flip * 0.5f, 0); // 只画 X 轴半边
        Debug.DrawLine(center, farEnd, Color.red, 3f);


        Collider2D[] cols = Physics2D.OverlapBoxAll(center, size, 0f,
                                                     LayerMask.GetMask("Enemy"));

        if (cols.Length > 0)
            特殊攻击是否打到人 = true;

        if (特殊攻击是否打到人 == true)
        {
            Anim.SetAnimSpeed(0);
            //震动
            CameraManager.Instance.CameraShake(timer, 0.3f);
            //延迟受伤
            TimeManager.Instance.OneTime(timer,
                () =>
                {
                    Anim.SetAnimSpeed(1);
                    foreach (var col in cols)
                    {
                        if (col.TryGetComponent<IBeDamaged>(out IBeDamaged enemy))
                        {

                            enemy.OnHurt(speicalDamage, this.gameObject);
                        }


                    }
                }
                );
        }
    }
    private bool 特殊攻击是否打到人 = false;
    DamageData speicalDamage = new DamageData()
    {
        type = DamageType.magic,
        atk = 999,
        RepellingXSpeed = 8,
        RepellingYSpeed = 1.5f,
        RepellingXDistance = 1,
        RepellingYDistance = 0.1f
    };
    private float restoreSpeed = 1f;
    public void 整体时间缓慢(float timer)
    {
        if (特殊攻击是否打到人 == false) return;
        特殊攻击是否打到人 = false;

        float startTimeScale = Time.timeScale;
        float targetTimeScale = 0.1f;
        float slowDuration = 0.3f;
        float stayDuration = 0.3f;
        float totalDuration = slowDuration * 2 + stayDuration;

        float elapsedTime = 0f;

        TimeManager.Instance.ReallyFrameTime(totalDuration,
            () =>
            {
                elapsedTime += Time.unscaledDeltaTime;
                Time.fixedDeltaTime = Mathf.Min(0.02f * Time.timeScale, 0.02f);
                float progress = elapsedTime / totalDuration;

                float currentScale;
                if (elapsedTime < slowDuration)
                {
                    // 降速阶段
                    float t = elapsedTime / slowDuration;
                    currentScale = Mathf.SmoothStep(startTimeScale, targetTimeScale, t);
                }
                else if (elapsedTime < slowDuration + stayDuration)
                {
                    // 停留阶段
                    currentScale = targetTimeScale;
                }
                else
                {
                    // 恢复阶段
                    float t = (elapsedTime - slowDuration - stayDuration) / slowDuration;
                    currentScale = Mathf.SmoothStep(targetTimeScale, startTimeScale, t);
                }

                Time.timeScale = currentScale;
            },
            () =>
            {
                Time.timeScale = startTimeScale;
                Time.fixedDeltaTime = 0.02f;
                SwitchState(TypeState.ldle);
            }
        );
    }
}
//    public void 整体时间缓慢(float timer)
//    {
//        if (!特殊攻击是否打到人) return;
//        特殊攻击是否打到人 = false;

//        float elapsed = 0f;

//        // ① 渐慢 0.3 现实秒
//        TimeManager.Instance.ReallyFrameTime(0.3f,
//            () =>
//            {
//                elapsed += Time.unscaledDeltaTime;
//                Time.timeScale = Mathf.Lerp(1f, 0.1f, elapsed / 0.3f);
//                Time.fixedDeltaTime = Mathf.Min(0.02f * Time.timeScale, 0.02f);
//            },
//            () =>
//            {
//                Time.timeScale = 0.1f;   // 确保正好 0.1
//                停留();                 // ② 恒定慢
//            });
//    }

//    private void 停留()
//    {
//        // ③ 恒定 0.1x 停留 0.3 现实秒
//       TimeManager.Instance.ReallyOneTime(0.3f, 恢复);
//        Time.fixedDeltaTime = Mathf.Min(0.02f * Time.timeScale, 0.02f);
//        //TimeManager.Instance.ReallyOneTime(0.3f, ()=>
//        //{
//        //    Time.timeScale = 1;
//        //});
//    }

//    private void 恢复()
//    {
//        float elapsed = 0f;
//        // ④ 渐快 0.3 现实秒
//        TimeManager.Instance.ReallyFrameTime(0.3f,
//            () =>
//            {
//                elapsed += Time.unscaledDeltaTime;
//                Time.timeScale = Mathf.Lerp(0.1f, 1f, elapsed /0.3f);
//                Time.fixedDeltaTime = Mathf.Min(0.02f * Time.timeScale, 0.02f);
//            },
//            () =>
//            {
//                Time.timeScale = 1f;
//                Time.fixedDeltaTime =0.02f;
//                SwitchState(TypeState.ldle);
//            });
//    }

//}
//状态机注意
//武器系统也要改动
//攻击状态也注意
//状态机的创建后 看看需要传入什么参数