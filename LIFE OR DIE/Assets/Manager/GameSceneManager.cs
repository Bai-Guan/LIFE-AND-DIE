using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    // 跳转到任意场景（名字或索引都行）
    public static void Load(string sceneName) => SceneManager.LoadScene(sceneName);
    public static void Load(int buildIndex) => SceneManager.LoadScene(buildIndex);

    // 重新载入当前场景
    public static void Reload() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

}
