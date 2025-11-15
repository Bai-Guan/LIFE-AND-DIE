using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isRemove = false;
    protected new string name;
    protected RectTransform rectTrans;

    protected virtual void Awake()
    {
        rectTrans = this.transform.GetComponent<RectTransform>();
    }

    
    public virtual void OpenPanel(string name)
    {
        if (isRemove == true) return;
       

        this.name = name;
        gameObject.SetActive(true);
    }

    public virtual void ClosePanel(string name,bool 是否为默认淡出) 
    {
        if (isRemove == true) return;
      isRemove = true;
        float timer = 0;
        //淡出
        if (rectTrans != null&&是否为默认淡出)
        {
            TimeManager.Instance.FrameTime(0.2f,
                () =>
                {
                    timer += Time.deltaTime;
                    float temp = timer / 0.2f;
                    float scale = Mathf.Lerp(1f, 0f, temp);
                    rectTrans.localScale = new Vector3(scale, scale, 0);
                },
                () =>
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                }
                );
        }
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    

       
    }
}
