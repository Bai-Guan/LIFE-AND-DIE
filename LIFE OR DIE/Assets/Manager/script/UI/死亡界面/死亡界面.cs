using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class 死亡界面 :BasePanel
{
    [SerializeField] private TMPro.TextMeshProUGUI 可修改Text;
    [SerializeField] protected TMPro.TextMeshProUGUI 按钮文本;
    [SerializeField] protected Image 背景;
    [SerializeField] protected Image 按钮背景;
    [SerializeField] protected float 淡入时间 = 2f;
    private float timer = 0;

    public void 修改文本(string text)
    {
        可修改Text.text = text;
    }
    public void 重新开始()
    {
        GameSceneManager.Reload();
    }
    public void 切换关卡(int id)
    {
        GameSceneManager.Load(id);
    }

    private void Start()
    {
        背景.color = new Color(背景.color.r, 背景.color.g, 背景.color.b, 0);
        可修改Text.color = new Color(可修改Text.color.r, 可修改Text.color.g, 可修改Text.color.b,0);
        按钮背景.color = new Color(按钮背景.color.r, 按钮背景.color.g, 按钮背景.color.b, 0);
        按钮文本.color = new Color(按钮文本.color.r, 按钮文本.color.g, 按钮文本.color.b, 0);
    }
    public void Update()
    {
        timer += Time.deltaTime;
        float scale = Mathf.Clamp01(timer/淡入时间);
        背景.color = new Color(背景.color.r, 背景.color.g, 背景.color.b, scale);
        可修改Text.color = new Color(可修改Text.color.r, 可修改Text.color.g, 可修改Text.color.b, scale);
        按钮背景.color=new Color(按钮背景.color.r, 按钮背景.color.g, 按钮背景.color.b,scale);
        按钮文本.color = new Color(按钮文本.color.r, 按钮文本.color.g,按钮文本.color.b,scale);
    }
}
