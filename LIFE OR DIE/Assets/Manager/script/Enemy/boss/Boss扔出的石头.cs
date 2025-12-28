using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss扔出的石头 : MonoBehaviour
{
    //父物体平移，子物体旋转 超过一定时间自我销毁
    [SerializeField] float 飞行速度 = 3f;
    [SerializeField] float 飞行最大时间 = 10f;
    [SerializeField] private float 旋转速度 = 270f;  // 度/秒
    private int 旋转方向 = 1;   // 1 顺时针  -1 逆时针
    [SerializeField] Rigidbody2D rb;
   [SerializeField] Transform 子物体;


    private float timer = 0;
    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if(子物体==null)
        {
            子物体=this.transform.Find("石头").GetComponent<Transform>();
        }
    }
  
    public void 设置方向(int dir)
    {
        rb.velocity = new Vector2(dir*飞行速度,0);
        旋转方向 = dir;
    }

    // Update is called once per frame
    void Update()
    {
        /* 子物体持续旋转 */
        子物体.Rotate(0, 0, 旋转方向 * 旋转速度 * Time.deltaTime);
        timer += Time.deltaTime;
        if(timer>飞行最大时间)
            Destroy(this.gameObject);

    }
}
