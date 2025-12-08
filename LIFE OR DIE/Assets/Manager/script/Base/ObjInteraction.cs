using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ObjInteraction : MonoBehaviour
{
    [Header("输入该可拾取物品的ID，默认值-1为无效ID")]
    [SerializeField] private int ID=-1;

    private NewPlayerControll ctx;
    private void Awake()
    {
    
    }

    private void Start()
    {
      
    }
 
    bool isEnter = false;
  private void PickUpItem()
    {
        if (isEnter&&StackInteraction.Instance.Peek()==this.gameObject)
        {
            if (ID < 0) { Debug.LogWarning("要拾取的物品未配对ID"); return; }
            //弹出该物品
            GameObject pickUpObject=   StackInteraction.Instance.PopSomeOne(this.gameObject);
            if(pickUpObject==null) return;
            //销毁该物品 为背包增加物品
            PackageInventoryService.Instance.AddItem(PackageInventoryService.Instance.GetNewItemById(ID));
            Destroy(pickUpObject );
        }
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEnter = true;
            ctx=collision.GetComponent<NewPlayerControll>();
            ctx.OnInteractPressed += PickUpItem;
        }
          
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ctx.OnInteractPressed -= PickUpItem;
            isEnter = false;
        }
           
    }

    private void OnDestroy()
    {
        ctx.OnInteractPressed -= PickUpItem;
    }
}
