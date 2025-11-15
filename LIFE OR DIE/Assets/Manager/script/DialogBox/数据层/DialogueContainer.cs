using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_", menuName = "对话系统/对话容器")]
public class DialogueContainer : ScriptableObject
{
    [SerializeField] public string conversationId;
    [SerializeField] public string start;
   // [SerializeField] public DiaglugueNode strat;
    [SerializeField] public List<DiaglugueNode> nodes;

    private Dictionary<string, DiaglugueNode> keyValues;

    //TODO：切换到下一个对话


    private void OnEnable()
    {
        keyValues=new Dictionary<string, DiaglugueNode>(nodes.Count);
        foreach(DiaglugueNode node in nodes)
        {
            keyValues[node.NodeId]=node;
        }
    }
    public DiaglugueNode GetNode(string nodeId)
    {
     return   keyValues.TryGetValue(nodeId, out DiaglugueNode node) ? node : null;
    }

}
