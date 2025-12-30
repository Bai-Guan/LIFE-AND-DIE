using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class obj拾取加血 : MonoBehaviour
{
    [SerializeField] private int hp = 10;
    private PlayerDataManager temp;
    private bool isEnter=false;
    private NewPlayerControll ctx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            isEnter = true;
            temp = collision.GetComponent<PlayerDataManager>();
            ctx = collision.GetComponent<NewPlayerControll>();
            ctx.OnInteractPressed += AddHP;
        }

    }
    private void AddHP()
    {
        if (isEnter && StackInteraction.Instance.Peek() == this.gameObject)
        {
            for (int i = 1; i <= hp; i++)
                temp.AddHP();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ctx.OnInteractPressed -= AddHP;
            isEnter = false;
        }

    }

    private void OnDestroy()
    {
        ctx.OnInteractPressed -= AddHP;
    }
}
