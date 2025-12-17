using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class combotManager : IBeDamaged
{
    //受伤时候，走这里的方法   
    [SerializeField] private float preInputTime = 0.2f;
    private PlayerControl control;

    //基于预测死亡的判断
    private bool islife = false;
    //判断玩家是否预测成功了
    private bool isPredictionSuccessful = false;


    //玩家是否按下了  舍命
    void PressResurrection(InputAction.CallbackContext callback )
    {
        if(islife==true)return;
        //特效

        //逻辑
        islife=true;
        TimeManager.Instance.OneTime(preInputTime,
            () =>
            {
                islife=false;
            }
            );
    }
    //判断玩家是否预测成功
    //受伤时候调用
    public float OnHurt(DamageData damage, GameObject obj)
    {
        switch (damage.type)
        {
            case DamageType.physics:

                if(islife ==true)
                {
                    isPredictionSuccessful= true;
                    //切换为奇招状态
                }


                break;
            case DamageType.magic:

                break;
        }

        return 999999;
    }
}
