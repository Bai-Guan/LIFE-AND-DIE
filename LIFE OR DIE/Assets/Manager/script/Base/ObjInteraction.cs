using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ObjInteraction : MonoBehaviour
{
    [Header("输入该可拾取物品的ID，默认值-1为无效ID")]
    [SerializeField] private int ID=-1;
    private void Awake()
    {
        
    }

    private void Start()
    {
        InputManager.Instance.Input_Key_E += PickUpItem;
    }
    bool isEnter = false;
  private void PickUpItem()
    {
        if (isEnter)
        {
            if (ID < 0) { Debug.LogWarning("要拾取的物品未配对ID"); return; }
            //弹出该物品
            GameObject pickUpObject=   StackInteraction.Instance.PopSomeOne(this.gameObject);
            if(pickUpObject==null) return;
        //销毁该物品 为背包增加物品
        
           Destroy(pickUpObject );
        }
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
            isEnter = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isEnter = false;
    }

    private void OnDestroy()
    {
        InputManager.Instance.Input_Key_E -= PickUpItem;
    }
}
