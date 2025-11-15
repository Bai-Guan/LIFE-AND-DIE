using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class 输入测试 : MonoBehaviour
{
    public void 移动(InputAction.CallbackContext context)
    {
      
        print(context.ReadValue<Vector2>());
    }
}
