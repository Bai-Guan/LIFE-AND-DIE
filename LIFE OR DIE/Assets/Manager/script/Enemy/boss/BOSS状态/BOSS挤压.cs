using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS挤压 : BOSS状态基类
{
    public BOSS挤压(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }
    private GameObject currentStone;
    private float distance=1f;
    private float distancey = 0.07f;
    private float dir = 0;
    private bool isEnter=false;

    private float _curSpeed = 0f;
    private float 挤压最大速度 = 10f;
    private const float AccelPerSec = 2f;
    private float timer = 0f;
    public override void Enter()
    {
        AIFsm.面朝玩家();
        AIFsm.AnimtorEvent.SetBool("push");
      currentStone=  GameObject.Instantiate(AIFsm.Boss要推的石头);
        //设置假父物体
        currentStone.GetComponent<随设定的物体移动>().SetParent(AIFsm.gameObject);
        dir = AIFsm.ReturnDirToPlayer();
       Vector2 temp  = AIFsm.transform.position;
        temp.x += distance * dir;
        temp.y += distancey;
        currentStone.transform.position=temp;
        //currentStone.transform.SetParent(AIFsm.transform);
        AIFsm.BossEvent?.Invoke(true);
        _curSpeed = 3f;
        timer = 0f;
        isEnter = true;

        Debug.Log("方向为："+dir.ToString()+" 生成石头的位置在"+currentStone.transform.position.ToString());
    }

    public override void Exit()
    {
        AIFsm.BossEvent?.Invoke(false);
        if (currentStone != null)
        {
            GameObject.Destroy(currentStone);
        }
    }

    public override void FixedUpdate()
    {
        if (玩家的全局变量.玩家是否死亡 == true) return;

        if (_curSpeed < 挤压最大速度)
        {
            _curSpeed += AccelPerSec * Time.fixedDeltaTime;
            _curSpeed = Mathf.Min(_curSpeed, 挤压最大速度);
        }


        AIFsm.rb.velocity = new Vector2(_curSpeed * AIFsm.ReturnDirToPlayer(), AIFsm.rb.velocity.y);
        AIFsm.SetFacing(AIFsm.ReturnDirToPlayer());

      
    }

    public override void Update()
    {
        if (玩家的全局变量.玩家是否死亡 == true)
        {
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                GameObject.Destroy(currentStone);
                AIFsm.SwitchState(BOSSAITypeState.ldle);
                AIFsm.BossEvent?.Invoke(false);
                return;
            }
        }
        if (currentStone == null && isEnter == true&&玩家的全局变量.玩家是否死亡==false)
        {
            //石头被玩家打碎 进入5连击状态
          float temp=  Random.Range(0f,1f);
         
            if (temp<=0.4f)
            {
                AudioManager.Instance.PlaySFX("危");
                AIFsm.SwitchState(BOSSAITypeState.fiveAttack);
                EffectManager.Instance.全局慢动作(1.5f);
                return;
            }
            else
            {
                AudioManager.Instance.PlaySFX("危");
                AIFsm.SwitchState(BOSSAITypeState.quickAttack);
                EffectManager.Instance.全局慢动作(1.5f);
                return;
            }
            
        }
    }
}
