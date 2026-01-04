using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 通关界面 : BasePanel
{
 
    public void 重新开始这一关()
    {
        GameSceneManager.Reload();

    }
    public void 退出游戏()
    {
        Application.Quit();

    }
}
