using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
   [SerializeField] Transform Camera;
  [SerializeField]  Transform background;
    [SerializeField] Transform needPos;
    [SerializeField] private GameObject midGround;
    [SerializeField] float parallaxScaleMid = 0.5f;

    private Vector2 lastCameraPos;
    private void Awake()
    {
       
            midGround.transform.position = new Vector2(needPos.position.x, needPos.position.y-1f);
        
      
    }
    private void Start()
    {
        if (Camera == null) Camera = Camera.transform;
        lastCameraPos = Camera.position;
    }

    private void LateUpdate()
    {
      Vector2  cameraPos = new Vector2(Camera.position.x, Camera.position.y);
        background.position= cameraPos;

        // 计算相机本帧的位移
        Vector2 camDelta = cameraPos - lastCameraPos;

        // 应用视差位移到每个中景对象（仅X方向，也可加Y）
     
                midGround.transform.position += new Vector3(camDelta.x * parallaxScaleMid, camDelta.y * parallaxScaleMid, 0);
        
        

        // 更新记录
        lastCameraPos = Camera.position;
    }
}