using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_", menuName = "对话系统/对话节点")]
[System.Serializable]
public class DiaglugueNode :ScriptableObject
{
    [Header("基本属性/文本")]
    [SerializeField] public string NodeId;
    [SerializeField] public string text;
    [SerializeField] public string speaker;

    //[Header("文本特殊效果")]
    //[Header("特殊文本的字符下标范围")]
    //[SerializeField] public Vector2 range;
    //[SerializeField] public Color color;
    //[SerializeField] public string effect;
    [Header("下一个节点ID")]
    [SerializeField] public string NextNodeId;
    [Header("分支选项")]
    [SerializeField] public  List<DialugueOption> options;

}

