using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class 胜利结算 : MonoBehaviour
{
    NewPlayerControll temp;
    private PlayerInput inputAction;
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.StopMusic();
       
            inputAction=collision.gameObject.GetComponent<PlayerInput>();
            TimeManager.Instance.OneTime(1f, () => inputAction.SwitchCurrentActionMap("Inventory"));
       

            TimeManager.Instance.OneTime(4f, () => UIManager.Instance.OpenPanel(UIManager.UIConst.PassLevel));
        }

        
   
    }
}
