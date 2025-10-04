using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHitBox : MonoBehaviour
{

    //这里的事件均由 动画事件 调用     动画事件的调用由有限状态机的攻击状态调用
    public BoxCollider2D attack1;
    public BoxCollider2D attack2;
    public BoxCollider2D attack3;

    private void Awake()
    {
        //attack1.enabled = false;
        //attack2.enabled = false;
        //attack3.enabled = false;
    }

 
}
