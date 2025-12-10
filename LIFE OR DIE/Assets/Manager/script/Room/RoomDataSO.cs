using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "创建房间SO数据", menuName = "Room Data")]
public class RoomDataSO :ScriptableObject
{
   
   
        [Header("房间基本信息")]
        public string roomName = "未命名房间";
        public int roomID;

        [Header("可视区域")]
        public Rect visibleRect = new Rect(0, 0, 20, 15);

        [Header("调试设置")]
        public bool showDebug = true;
        public Color debugColor = new Color(0.2f, 0.8f, 0.3f, 0.3f);

        [Header("房间状态")]
        public bool isDiscovered = false;

        // 快捷属性
       public Vector2 Center => visibleRect.center;
        public Vector2 Size => visibleRect.size;
    
}
