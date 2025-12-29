using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 连锁销毁 : MonoBehaviour
{
    private bool 是否存在过=false;
    public GameObject obj;
    void Start()
    {
      if (obj != null)
        {
            是否存在过 = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(obj == null&&是否存在过==true)
        {
            Destroy(this.gameObject);
        }
    }
}
