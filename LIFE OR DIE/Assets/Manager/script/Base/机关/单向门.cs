using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 单向门 : MonoBehaviour
{
    bool block = false;
  [SerializeField]  private GameObject 门本体;
    [SerializeField] private Transform 门的位移点;
    [SerializeField] private float 开启时间 = 3f;    // 保持开启几秒
    [SerializeField] private float 移动速度 = 3f;    // 米/秒
    private Vector3 初始位置;
    private bool 已经开过 = false;   // 单向：只开一次
    private float 计时器 = 3f;
    private float timer=0;
    private enum 状态 { 静止, 去, 等, 回 }
    private 状态 当前状态 = 状态.静止;
    private NewPlayerControll NewPlayerControll;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NewPlayerControll = collision.GetComponent<NewPlayerControll>();
            if (NewPlayerControll != null)
                NewPlayerControll.OnInteractPressed += 开启单向门;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (NewPlayerControll != null)
            NewPlayerControll.OnInteractPressed -= 开启单向门;
        block = false;
    }
    private void OnDestroy()
    {
        if (NewPlayerControll != null)
            NewPlayerControll.OnInteractPressed -= 开启单向门;
    }
    // 按交互键回调
    private void 开启单向门()
    {
        if (已经开过 || 当前状态 != 状态.静止) return;

     
        if (StackInteraction.Instance.Peek() != this.gameObject) return;
        StackInteraction.Instance.PopSomeOne(this.gameObject);

        已经开过 = true;
        当前状态 = 状态.去;
    }
    private void Awake()
    {
        初始位置=门本体.transform.position;
    }
    private void Update()
    {
        switch (当前状态)
        {
            case 状态.去:
                门本体.transform.position = Vector3.MoveTowards(
                    门本体.transform.position,
                    门的位移点.position,
                    移动速度 * Time.deltaTime);

                if (门本体.transform.position == 门的位移点.position)
                {
                   
                    当前状态 = 状态.等;
                    计时器 = 开启时间;
                }
                break;

            case 状态.等:
                timer += Time.deltaTime;
                if (timer>=计时器)
                {
                    当前状态 = 状态.回;
                }
                break;

            case 状态.回:
                门本体.transform.position = Vector3.MoveTowards(
                    门本体.transform.position,
                    初始位置,
                    移动速度 * Time.deltaTime);

                if (门本体.transform.position == 初始位置)
                {
                 
                    当前状态 = 状态.静止;
                    已经开过=false;
                    timer = 3;
                }
                break;
        }
    }
}
