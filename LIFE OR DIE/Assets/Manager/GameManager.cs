using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance;

    private 游戏状态 currentGameState = 游戏状态.游玩中玩家控制;
  public  enum 游戏状态
    {
        选择界面,
        游玩中玩家控制,
        脚本动画,
        物品栏,
        对话框,
    }
    private void Awake()
    {
        if(instance != null&&instance !=this)
            Destroy(instance);
        instance = this;
        InitData();
    }
    private void InitData()
    {
        PackageInventoryService.Instance.InitPackage();
    }
    public void ChangeGameState(游戏状态 状态)
    {
        currentGameState = 状态;
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager._1999背景音乐);
    }
}
