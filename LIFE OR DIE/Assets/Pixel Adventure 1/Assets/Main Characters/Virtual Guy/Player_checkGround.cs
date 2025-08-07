using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_checkGround : MonoBehaviour
{
    PlayerControl playerControl;
    private void Start()
    {
        playerControl=GetComponentInParent<PlayerControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Ground")
        {
            Debug.Log("��ؼ�⣡");
            isGround = true;

            //������Ծ
            playerControl.setJump(true);
            playerControl.SetDoubleJump(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Debug.Log("��ؼ�⣡");
            isGround = false;

            
        }
    }
    public void SetIsGround(bool YesOrNo)
    {
        isGround= YesOrNo;
    }

    public bool CheckGround()
    {
        return isGround;
    }
    bool isGround = true;
}
