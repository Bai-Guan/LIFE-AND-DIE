using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_", menuName = "对话系统/对话容器")]
public class DialogueContainer : ScriptableObject
{
    [SerializeField] public string conversationId;
    [SerializeField] public DiaglugueNode strat;
    [SerializeField] public List<DiaglugueNode> nodes;
}
