using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntController : MonoBehaviour
{
   public enum GruntState
    {
        Patrol,
        Attack,
        Hurt,
        Death
    }
    private GruntState _current = GruntState.Patrol;
    [SerializeField] private EnemyBaseData _enemyData;
    private Dictionary<GruntState, GruntBaseState> DicGruntState = new Dictionary<GruntState, GruntBaseState>();
    private GruntBaseState Gruntstate;

    private bool IsFindingPlayer = false;
    private GruntEventAnim _eventAnim;

    private Animator anim;

    [SerializeField] private float _quickMoveSpeed;
    public float QuickMoveSpeed {  get; private set; }


    private int currentHP;
    private int Defense;
    private int Knocked;

    private void Awake()
    {
        DicGruntState[GruntState.Patrol] = new GruntPatrolState(this);
        DicGruntState[GruntState.Attack] = new GruntAttackState(this);
        DicGruntState[GruntState.Death] = new GruntDeathState(this);
        DicGruntState[GruntState.Hurt] = new GruntHurtState(this);

        _eventAnim = this.transform.GetComponent<GruntEventAnim>();
         anim = this.transform.GetComponent<Animator>();
        Gruntstate = DicGruntState[GruntState.Patrol];
    }


    void Start()
    {
        _eventAnim.DieFinish += DieFinish;
        _eventAnim.prepareFinish += preFinish;
        InitSOData();

    }
    void InitSOData()
    {
        currentHP=_enemyData.MaxHP;
        Defense = _enemyData.Defense;
        Knocked = _enemyData.KnockBackResistance;
    }
   
    void Update()
    {
        Gruntstate.OnUpdate();
    }

   public void SwitchEnemyState(GruntState state)
    {
        if (state == _current) return;
        Gruntstate.OnExit();
        _current = state;
        Gruntstate=DicGruntState[_current];
        anim.SetInteger("currentState", (int)_current);
        Gruntstate.OnEnter();
    }

    private void preFinish()
    {
        anim.SetTrigger("QuickMove");
    }

    private void DieFinish()
    {

    }

    public void BeAttacked(DamageData damage)
    {

    }

    private void OnDisable()
    {
        _eventAnim.DieFinish -= DieFinish;
        _eventAnim.prepareFinish -= preFinish;
    }
}
