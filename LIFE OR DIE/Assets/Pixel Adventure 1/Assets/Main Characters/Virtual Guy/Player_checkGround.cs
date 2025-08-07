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
            Debug.Log("¬‰µÿºÏ≤‚£°");
            isGround = true;

            //‘ –ÌÃ¯‘æ
            playerControl.setJump(true);
            playerControl.SetDoubleJump(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Debug.Log("¿ÎµÿºÏ≤‚£°");
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
