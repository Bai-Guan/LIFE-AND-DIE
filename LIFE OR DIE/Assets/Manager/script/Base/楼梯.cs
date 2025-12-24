using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class 楼梯 : MonoBehaviour
{
    private NewPlayerControll pc;
    [Tooltip("沿楼梯向上的世界速度")]
    public float climbSpeed = 3f;
    [Tooltip("1 表示向上  -1 表示向下")]
    public int direction = 1;   // 楼梯朝哪边
    private bool inEnter=false; 
    void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;
        pc = c.GetComponent<NewPlayerControll>();
        pc.OnInteractPressed += 切换状态为爬楼梯状态;
        inEnter = true;
     //   pc.CurrentStair = this;     // 告诉玩家“你踩在楼梯上”
    }

    void 切换状态为爬楼梯状态()
    {
        if(StackInteraction.Instance.Peek()==this.gameObject&&inEnter==true&&玩家的全局变量.玩家是否死亡==false)
        {
            StackInteraction.Instance.PopSomeOne(this.gameObject);
            pc.SwitchState(TypeState.stair);
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;

        pc.SwitchState(TypeState.run);
        pc.OnInteractPressed -= 切换状态为爬楼梯状态;
        inEnter = false;
        // if (pc.CurrentStair == this) pc.CurrentStair = null;
    }

    private void OnDestroy()
    {
        if(pc==null) return;
        pc.OnInteractPressed -= 切换状态为爬楼梯状态;
    }
}
