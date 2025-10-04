using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_checkGround : MonoBehaviour
{
    //ͼ��
    int GroundAndPlatform = 6;
    
    public event Action<PlayerEventArgs> groundCollisionTriggered;
    public event Action<PlayerEventArgs> platformCollisionTriggered;

    public List<Transform> checkpoint;
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;
    private bool isGrounded = true;

    public bool IsGrounded {  get { return isGrounded; } }


    PlayerControl playerControl;
   
    private void Start()
    {
        playerControl = GetComponentInParent<PlayerControl>();
        checkpoint.Add(p1);
        checkpoint.Add(p2);
        checkpoint.Add(p3);
        checkpoint.Add(p4);
    }
    private void Update()
    {
        isGrounded=CheckIsGrounded();
        playerControl.SetGrounded(isGrounded);
    }

    [SerializeField] private int groundedleg=0;
    private bool CheckIsGrounded()
    {

        groundedleg = 0;

        if (checkpoint == null || checkpoint.Count == 0)
            return false;

        float checkRadius = 0.08f; // ���뾶

        foreach (Transform t in checkpoint)
        {
            if (t == null) continue;

            // �ڼ���λ�ô���һ��Բ�μ������
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                t.position,
                checkRadius
               
            );

            // ���ӻ��������
            Debug.DrawLine(t.position + Vector3.left * checkRadius, t.position + Vector3.right * checkRadius, Color.blue);
            Debug.DrawLine(t.position + Vector3.up * checkRadius, t.position + Vector3.down * checkRadius, Color.blue);

            bool pointGrounded = false;
            foreach (Collider2D collider in hits)
            {
                if (collider.CompareTag("Ground") || collider.CompareTag("Platform"))
                {
                    pointGrounded = true;
                    break;
                }
            }

            if (pointGrounded)
            {
                groundedleg++;
            }
        }
        if (groundedleg > 1) playerControl.SetSprintOneTimes();
        return groundedleg > 1;
        //groundedleg = 0;



        //foreach (Transform t in checkpoint)
        //{

        //    if (t == null)
        //    {
        //        Debug.LogWarning("�գ�");
        //        continue;
        //    }
        //    Vector3 temp = new Vector3(t.position.x,t.position.y-0.1f,0);
        //    Debug.DrawLine(t.position, temp);
        //    // ʹ��ͼ����ˣ��������
        //    RaycastHit2D hit = Physics2D.Raycast(t.position, Vector2.down, 0.3f);

        //    if (hit.collider != null) // ��ֹ������
        //    {
        //        // ʹ��CompareTag����Ч
        //        if (hit.collider.tag=="Ground" || hit.collider.tag=="Platform")
        //        {
        //            groundedleg++;
        //        }

        //    }



        //}
        //if (groundedleg > 1)
        //    return true;
        //else
        //    return false;


    }
  

}
