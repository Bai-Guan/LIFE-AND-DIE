using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 重置位置 : MonoBehaviour, IEnemyReset
{
    [SerializeField] private List<GameObject> enemyList;   // 在 Inspector 里拖
    private readonly Dictionary<GameObject, Vector3> spawnPoints = new();

    private void Awake()
    {
        if (enemyList == null) return;

        // 记录每个敌人第一次出现的位置
        foreach (GameObject go in enemyList)
        {
            if (go != null)                       // 防止 Inspector 里留空
                spawnPoints[go] = go.transform.position;
        }
    }
    public void EnemyReset()
    {
        Debug.Log("初始化房间怪物");
        foreach (var kvp in spawnPoints)
        {
            GameObject go = kvp.Key;          // 原始敌人实例
            Vector3 pos = kvp.Value;          // 出生坐标

            // 1. 如果敌人已被销毁
            if (go == null)
            {
                continue;
            }

            // 2. 敌人还在，只是被禁用或死了
            if (!go.activeSelf)
                go.SetActive(true);

            // 3. 拉回出生点
            go.transform.position = pos;
            go.GetComponent<IEnemyReset>().EnemyReset();

            if(玩家的全局变量.是否刷怪==false)
            {
                go.SetActive(false);
            }
        }
    }
}
