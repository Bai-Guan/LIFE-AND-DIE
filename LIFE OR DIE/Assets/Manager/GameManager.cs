using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        InitData();
    }
    private void InitData()
    {
        PackageInventoryService.Instance.InitPackage();
    }


    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager._1999±≥æ∞“Ù¿÷);
    }
}
