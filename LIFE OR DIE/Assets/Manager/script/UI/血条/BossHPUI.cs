using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : BasePanel
{
    [SerializeField] private TMPro.TextMeshProUGUI 血量文本UI;
    [SerializeField] private Image 血条本体UI;
    [SerializeField] private Image 僵直条本体UI;


    private void Start()
    {
        血量文本UI = this.transform.Find("血条/血条文本").GetComponent<TMPro.TextMeshProUGUI>();
        血条本体UI = this.transform.Find("血条/血条本体").GetComponent<Image>();
        僵直条本体UI = this.transform.Find("僵直条/僵直条本体").GetComponent<Image>();
    }

    public void 改变血量(int hp,int MaxHP)
    {
        int currentHP = Mathf.Max(0, hp);
       血量文本UI.text=currentHP.ToString();
        float scale = Mathf.Clamp01((float)currentHP / (float)MaxHP);
        血条本体UI.fillAmount = scale;

    }
    public void 改变僵直条数值(float r,float MaxR)
    {
        float currentR = Mathf.Max(0, r);
        float scale = Mathf.Clamp01((float)currentR / (float)MaxR);
        僵直条本体UI.fillAmount=scale;
    }
    public void 改变僵直条颜色(Color color)
    {
        僵直条本体UI.color = color;
    }
}
