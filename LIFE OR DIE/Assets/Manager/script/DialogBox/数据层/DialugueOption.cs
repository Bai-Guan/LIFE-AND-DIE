using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
[CreateAssetMenu(fileName = "Dialogue_", menuName = "对话系统/对话选项")]
[System.Serializable]
public class DialugueOption :ScriptableObject
{
    [Header("选项内容")]
    [SerializeField] public string OptionText;
    [SerializeField] public string NextNodeID;
    [SerializeField] public Sprite bg;
    
}
