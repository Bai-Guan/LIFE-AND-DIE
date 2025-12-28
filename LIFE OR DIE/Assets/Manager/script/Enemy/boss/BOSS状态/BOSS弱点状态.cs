using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS弱点状态 : BOSS状态基类
{
    public BOSS弱点状态(BOSSAI控制器 aIFsm) : base(aIFsm)
    {
    }
    private Color first = new Color(1, 0.7324f, 0f);

    private Color 闪烁色1 = new Color(1,1,1);
    private Color 闪烁色2 = new Color(1,0,0);

    private float 晕眩时间 = 2.5f;
    private float timer = 0;
    // Start is called before the first frame update
    public override void Enter()
    {
        
        AIFsm.AnimtorEvent.SetBool("hit");
        AIFsm.设置受伤倍率(4);
        AudioManager.Instance.PlaySFX("处决");
        EffectManager.Instance.ChromaticAberrationSet(1f, 0.8f);
        timer = 0;
    

    }



    public override void Exit()
    {
        AIFsm.僵直条.清空僵直条();
        AIFsm.设置受伤倍率(1);
        AIFsm.改变僵直条颜色(first);
    }



    public override void FixedUpdate()
    {
       
        float t = Mathf.PingPong(timer / 0.2f, 1f); // 0→1→0
        Color c = Color.Lerp(闪烁色2, 闪烁色1, t);          // 红↔白
        AIFsm.改变僵直条颜色(c) ;   
    }

    public override void Update()
    {
        AIFsm.只改变僵直条显示_不影响数值(5.5f);
       timer += Time.deltaTime;
        if (timer > 晕眩时间)
        {
            AIFsm.SwitchState(BOSSAITypeState.ldle);
        }
    }
}
