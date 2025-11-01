using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "��Ƶ���ݱ�", menuName = "��Ƶ/��Ƶ����")]

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