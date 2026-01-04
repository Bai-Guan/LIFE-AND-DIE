using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 按钮跳转场景 : MonoBehaviour
{
    public void 切换关卡(int id)
    {
        GameSceneManager.Load(id);
    }
}
