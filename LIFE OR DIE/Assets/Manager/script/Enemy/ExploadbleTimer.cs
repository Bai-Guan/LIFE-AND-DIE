using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploadbleTimer : MonoBehaviour
{
    private float timer = 0;
    private float random;

    private float percentage = 1;
    private void Awake()
    {
      random=  Random.Range(3f, 4f);
        percentage=1f-(timer/random);
    }


    // Update is called once per frame
    void Update()
    {
       timer+=Time.deltaTime;
        percentage = 1f - (timer / random);

        if(timer>=random)
        {
            Destroy(this.gameObject);
        }
    }
}
