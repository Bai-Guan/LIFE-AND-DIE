using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class InputManager : MonoBehaviour
{
    private static InputManager _input;
    public static InputManager Instance
    {
        get
        {
           
return _input;
        }
        private set { }
    }

    public event Action Input_Key_E; 
   
        private void Awake()
        {
            // 如果已存在且不是自己，销毁当前物体
            if (_input != null && _input != this)
            {
                Destroy(gameObject);
                return;
            }

        // 确立单例
           _input = this;
            // 可选：跨场景不销毁
          //  DontDestroyOnLoad(gameObject);
       }
    

    void Start()
    {
 
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) Input_Key_E?.Invoke();

    }
}
