using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ObjChat : MonoBehaviour
{

    [SerializeField] private DialogueContainer dialogue;
    NewPlayerControll temp;
    private InputAction inputAction;
    
    public DialogueContainer GetDialogue()
    {
        return dialogue;
    }
  
   //private void OpenDialogue(InputAction.CallbackContext context)
   // {
   //     //监听对话键位 并且判断是否进入范围
   //     if (isRange&& StackInteraction.Instance.Peek()==this.gameObject)
   //     {
           
   //         StackInteraction.Instance.PopSomeOne(this.gameObject);
   //         DialogManager.Instance.StartDialogue(dialogue);
         
   //     }
   // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            temp = collision.GetComponent<NewPlayerControll>();
            if(temp != null)
            {
              temp.DataMan.isCollisionOBJ(this.gameObject);
                
            }
        }
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (temp != null)
            {
                temp.DataMan.isExitOBJ(this.gameObject);

            }
        }
         
    }


}
