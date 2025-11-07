using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploadbleTimer : MonoBehaviour
{
    private float timer = 0;
    private float random;
    
    private float percentage = 1;
    MaterialPropertyBlock mpb;
    private Renderer ma;
    private const string 溶解值 = "_DissolveValue";
    private void Awake()
    {
      random=  Random.Range(5f, 8f);
        percentage=1f-(timer/random);
        ma = GetComponent<Renderer>();
         mpb = new MaterialPropertyBlock();
     
    }


    // Update is called once per frame
    void Update()
    {
       timer+=Time.deltaTime;
        percentage = 1f - (timer / random);
        float dissolve = 1f - percentage;     // 0→1

        mpb.SetFloat(溶解值, dissolve);
        ma.SetPropertyBlock(mpb);
        if (timer>=random)
        {
            Destroy(this.gameObject);
        }
    }
}
