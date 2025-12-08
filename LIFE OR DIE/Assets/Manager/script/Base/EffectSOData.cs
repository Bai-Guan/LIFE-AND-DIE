 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "特效/总表")]
public class EffectSOData : ScriptableObject
{
    [SerializeField] private List<FXEntry> entries ;

    // 运行时字典，只在 Awake 时构建一次
    private Dictionary<string, FXEntry> dict;

    public GameObject GetPrefab(string key)
    {
        if (dict == null) BuildDict();
        return dict.TryGetValue(key, out var e) ? e.prefab : null;
    }
    private void Awake()
    {
        BuildDict();
    }
    private void BuildDict()
    {
        Debug.Log("特效初始化字典");
        dict = new Dictionary<string, FXEntry>();
        foreach (var e in entries)
        {
            if (e == null || string.IsNullOrEmpty(e.key)) continue;
            if (dict.ContainsKey(e.key))
                Debug.LogWarning($"FXDatabase 重复 key：{e.key}");
            else
                dict.Add(e.key, e);
        }
    }
}

[System.Serializable]
public class FXEntry
{
    [Tooltip("调用时用的字符串")]
    public string key;
    [Tooltip("对应粒子预制体")]
  [SerializeField]  public GameObject prefab;
}