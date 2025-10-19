using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
   
   private  Vector3 CameraLastPos = new Vector3();
   

    [SerializeField] Transform prospect;
    [SerializeField] Transform MediumShot;

    Camera mainCamera;

    Transform medium1;
    Transform medium2;
    Transform medium3;
    //Transform medium4;
    List<Transform> ListMed = new List<Transform>();
    float mediumSizeWidth;
    float cameraSizeWidth;

    public float speed = 0.5f;

    void Start()
    {
        InitName();
        InitData();
        if (mainCamera == null)
        {
            Debug.LogError(" mainCamera not found in scene!");
            enabled = false;
            return;
        }
        else
        {
            this.gameObject.transform.position= mainCamera.transform.position;
        }

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
      
        //计算玩家位移量
        Vector2 amountToMove = new Vector2(mainCamera.transform.position.x- CameraLastPos.x, mainCamera.transform.position.y - CameraLastPos.y);
        //更改背景形成视差
        prospectFollow(amountToMove);
        mediumFollow(amountToMove);
        //更新最后的玩家位置信息
        CameraLastPos = mainCamera.transform.position;
        //地表时候无限循环的中景
        infiniteMedium();
        //Debug.Log("Update running -> amountToMove=" + amountToMove);
    }

    void InitName()
    {
        
        mainCamera = Camera.main;
        prospect = transform.Find("Prospect");
        MediumShot = transform.Find("Medium shot");
        medium1 = transform.Find("Medium shot/medium1");
        medium2 = transform.Find("Medium shot/medium2");
        medium3 = transform.Find("Medium shot/medium3");
        //medium4 = transform.Find("Medium shot/medium4");
    }
    void InitData()
    {
        mediumSizeWidth = medium1.GetComponent<SpriteRenderer>().sprite.bounds.size.x * medium1.localScale.x;
        cameraSizeWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
        ListMed.Add(medium1);
        ListMed.Add(medium2);
        ListMed.Add(medium3);
       // ListMed.Add(medium4);
    }

    private void prospectFollow(Vector2 amountToMove)
    {
        prospect.position+= new Vector3(amountToMove.x, amountToMove.y , 0);
    }
    private void mediumFollow(Vector2 amountToMove)
    {
        MediumShot.position += new Vector3(amountToMove.x * speed, amountToMove.y * speed, 0);
    }
    private void infiniteMedium()
    {
        foreach (Transform medium in ListMed)
        {
            if (medium.position.x + mediumSizeWidth / 2 <= mainCamera.transform.position.x - cameraSizeWidth/2  )
            {
                medium.position += new Vector3(mediumSizeWidth + cameraSizeWidth, 0, 0);
            }
            else if (medium.position.x - mediumSizeWidth / 2 >= mainCamera.transform.position.x + cameraSizeWidth/2  )
            {
                medium.position -= new Vector3(mediumSizeWidth + cameraSizeWidth, 0, 0);
            }
        }
    }
}
