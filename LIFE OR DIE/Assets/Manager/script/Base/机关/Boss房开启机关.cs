using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss房开启机关 : MonoBehaviour
{
    [SerializeField] BOSSAI控制器 Boss;
    [SerializeField] GameObject[] obj;
    private void Awake()
    {
        foreach (GameObject go in obj)
        {
            go.SetActive(false);
        }
        if (Boss != null) { Boss.BossEvent += 开关穿刺机关; }
    }

    public void 开关穿刺机关(bool tf)
    {
        Debug.Log($"开关穿刺机关({tf}) 被调用，数组长度={obj.Length}", this);
        foreach (GameObject go in obj)
        {
            go.SetActive(tf);
        }
    }
    private void OnDestroy()
    {
        if (Boss != null) { Boss.BossEvent -= 开关穿刺机关; }
    }
}
