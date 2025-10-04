using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCheck : MonoBehaviour
{
    PlayerControl control;
    SpriteRenderer SpriteRenderer;
   private bool isCollision = false;
    Transform GP;
    private void Start()
    {
        control=GetComponentInParent<PlayerControl>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        isfacingLeft=control.IsFacingLeft;
    }
    private void Update()
    {
        isfacingLeft = control.IsFacingLeft;
    }
    private float RayLength = 0.5f;
    LayerMask layerMask = 1 << 6;
    void checkCollision()
    {
        var dit = isfacingLeft ? -1 : 1;
        Vector2 dir = new Vector2(dit, 0);
        Debug.DrawRay(this.transform.position, dir*RayLength,Color.red);
        RaycastHit2D hit= Physics2D.Raycast(
            this.transform.position,
            dir,
            RayLength,
            layerMask
            );
        if(hit.collider == null )
        {
            isCollision = false ;
            return;
        }
        if(hit.collider.tag=="Ground"||hit.collider.tag=="Platform")
        {
            
            isCollision=true ;
            GP=hit.collider.transform ;
        }
    }

    private void FixedUpdate()
    {
        checkCollision();
    }

    

   
    public bool IsCollision()
    {
        return isCollision;
    }
    public Transform GetCollisionTransform()
    {
        return GP;
    }
    bool isfacingLeft;
}
