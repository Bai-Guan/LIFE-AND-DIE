using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ObjChat : MonoBehaviour
{
    private  bool isRange = false;
    [SerializeField] private DialogueContainer dialogue;
    private PlayerInput temp;
    private InputAction inputAction;
    

  
   private void OpenDialogue(InputAction.CallbackContext context)
    {
        //监听对话键位 并且判断是否进入范围
        if (isRange&& StackInteraction.Instance.Peek()==this.gameObject)
        {
           
            StackInteraction.Instance.PopSomeOne(this.gameObject);
            DialogManager.Instance.StartDialogue(dialogue);
         
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isRange = true;
            temp = collision.GetComponent<PlayerInput>();
            if(temp != null)
            {
                inputAction = temp.actions["Interaction"];
                inputAction.performed += OpenDialogue;
            }
        }
        
    }

    private void OnDestroy()
    {
        inputAction.performed -= OpenDialogue;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
           isRange = false;
    }


}
