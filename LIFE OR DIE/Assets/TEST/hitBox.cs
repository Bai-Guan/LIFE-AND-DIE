using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBox : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("��ײ�˺�!");
            collision.GetComponent<PlayerControl>().TakeHit(10, this.transform);
        }
    }

}

