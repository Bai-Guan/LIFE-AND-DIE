using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerHP : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textUI;
  [SerializeField]  private PlayerDataManager player;
    private float 死亡倒计时 = 10f;
    private float timer = 0f;
    void Start()
    {
        // 1. 获取玩家控制器）
        if (player == null)
            GameObject.Find("MainPlayer");
        if (textUI == null)
            textUI = this.transform.Find("背景/文本").GetComponent< TMPro.TextMeshProUGUI>();
        // 2. 订阅事件
        if (player != null)
        {
            死亡倒计时=player.AgonalTime;
            player.PlayerHPChange.AddListener(UpdateHealthBar);
        }
    }

    void OnDestroy()
    {
        // 避免内存泄漏
        if (player != null)
        {
            player.PlayerHPChange.RemoveListener(UpdateHealthBar);
        }
    }
    private void Update()
    {
    
    }
    private void UpdateHealthBar(int currentHealth)
    {
        string temp = currentHealth.ToString();
        if (currentHealth < 0)
            temp = "0";
        
      textUI.text =temp;
    }
}
