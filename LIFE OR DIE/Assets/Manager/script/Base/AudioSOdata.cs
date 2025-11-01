using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "音频数据表", menuName = "音频/音频数据")]

public class AudioSOdata :ScriptableObject
{
    [SerializeField]public List<AudioData> Audiodata;
}
[System.Serializable]
public class AudioData
{
    [SerializeField] public string Name;
    [SerializeField] public AudioClip audioClip;
}