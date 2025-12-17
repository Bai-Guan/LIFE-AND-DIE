using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj激活物品 : MonoBehaviour
{
    private NewPlayerControll pc;
    private bool inEnter = false;
    public GameObject obj;
    void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;
        pc = c.GetComponent<NewPlayerControll>();
        pc.OnInteractPressed += 激活物体;
        inEnter = true;
        //   pc.CurrentStair = this;     // 告诉玩家“你踩在楼梯上”
    }

    void 激活物体()
    {
        if (StackInteraction.Instance.Peek() == this.gameObject && inEnter == true)
        {
            StackInteraction.Instance.PopSomeOne(this.gameObject);
          //逻辑
          obj.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;
   

        pc.OnInteractPressed -= 激活物体;
        inEnter = false;
        // if (pc.CurrentStair == this) pc.CurrentStair = null;
    }

    private void OnDestroy()
    {
        if (pc == null) return;
        pc.OnInteractPressed -= 激活物体;
    }
}
