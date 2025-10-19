using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerState
{
    private InitWeaponSystem weapon;
    PlayerControl playerControl;
    AnimationEventHandler eventHandler;
    bool isExiting = false;

    public AttackState(PlayerControl ctx) : base(ctx)
    {
        playerControl = ctx;
    }

    public override void Enter()
    {
        Debug.Log("���빥��ģʽ");
        weapon = playerControl.weapon;
        eventHandler = weapon.EventHandler;
        _ctx.SetRB_X(0);
        isExiting = false;
        eventHandler.OnFinish += Exit;

        weapon.Enter();

    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        if (isExiting) return;
        isExiting = true;

        eventHandler.OnFinish -= Exit;
        playerControl.SwitchStatus(PlayerControl.PlayerStatus.ldle);
        // playerControl.SwitchStatus(PlayerControl.PlayerStatus.ldle);
        // TimeManager.Instance.LaterOneFrame(()=> playerControl.SwitchStatus(PlayerControl.PlayerStatus.ldle));

    }
 
    private void Attack()
    {

    }
}
//public class AttackState : PlayerState
//{
//    ////��д�����߼���� �����๥��ģ�� �ȵ���3��������


//    ////����ʱ������΢�ƶ� ����ʱ������ʱ���ܺ͸� ������ʱ��������Ծ ��Ծʱ������������������
//    ////��һ����ʱ�� ��X���ڽ��й������� �Ϳ��Խ�����һ���������� ÿ�ι����˺��ɶ���
//    ////��x���δ���й���������Ȼ�˳�����ģʽ������״̬
//    ////���빥��״̬ʱ��
//    //public int attackTimes = 0;
//    //bool isAttacking = false;
//    //private float waitTime1 = 0.5f;
//    //private float waitTime2 = 0.7f;
//    //private float timer = 0;
//    //float moveMulNumber = 0.15f;//��ֵС��1 �������ƹ���ʱ�������
//    //Dictionary<int, Action> actions = new Dictionary<int, Action>();

//    //private float attackTime1 = 0.45f;
//    //private float attackTime2 = 0.45f;
//    //private float attackTime3 = 0.6f;


//    public AttackState(PlayerControl ctx) : base(ctx)
//    {

//    }

//    public override void Enter()
//    {
//        Debug.Log("��������ģʽ");

//        //timer = 0;
//        //attackTimes = 0;
//        //isAttacking = true;
//        ////ˮƽ�ٶ�����
//        //_ctx.SetRB_X(0);
//        ////��������״̬
//        //_ctx.isAttacking = true;

//        ////�������ж���
//        //actions.Add(0, _ctx.Anim.TriggerAttack1);
//        //actions.Add(1, _ctx.Anim.TriggerAttack2);
//        //actions.Add(2, _ctx.Anim.TriggerAttack3);

//        //Attack();

//    }
//    public override void Update()
//    {
//        //timer += Time.deltaTime;



//        //if(attackTimes < 2 && _ctx.IsKeyDownAttack == true && timer >= attackTime1)
//        //{
//        //    Attack();
//        //    timer = 0;
//        //}

//        //if (attackTimes >= 2 && _ctx.IsKeyDownAttack == true && timer >= attackTime1)
//        //{
//        //    Attack();
//        //    timer = 0;
//        //}



//        ////��⳯��
//        //_ctx.CheckFill();
//        //if (timer >waitTime1&&attackTimes<2)
//        //{
//        //    _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
//        //}
//        //else if (timer > waitTime2 && attackTimes >= 2)
//        //{
//        //    _ctx.SwitchStatus(PlayerControl.PlayerStatus.ldle);
//        //}

//    }
//    public override void FixedUpdate()
//    {
//        //_ctx.XMove(moveMulNumber);
//    }
//    public override void Exit()
//    {
//        //actions.Clear();
//        //_ctx.isAttacking = false;
//    }

//    private void Attack()
//    {
//        //actions[attackTimes]?.Invoke();
//        //attackTimes++;
//        //attackTimes = attackTimes%3;
//    }
//}