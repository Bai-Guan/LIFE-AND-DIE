using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuckCamera : MonoBehaviour
{
    Vector3 showPos;
    void Start()
    {
        showPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -0.94f);
        this.transform.position = showPos;
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    private void LateUpdate()
    {
        showPos.x = Camera.main.transform.position.x;
        showPos.y = Camera.main.transform.position.y;

        this.transform.position = showPos;
    }
}
