using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private RoomDataSO roomData;

    [Tooltip("把本房间所有怪物拖进来，已销毁的不管")]
    [SerializeField] private List<GameObject> enemies; // 在 Inspector 里拖

    private BoxCollider2D col;
    public bool isDebug=true;

    // 缓存还活着的、实现了接口的怪物
    private List<GameObject> resettables = new List<GameObject>();

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        if(col==null)
        {
            Debug.LogError(roomData.roomName+"未组装碰撞器");
            return;
        }
        col.isTrigger = true;


        foreach (var m in enemies)
        {
            if (m == null) continue;
            {
                resettables.Add(m);
                m.SetActive(false);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            // 实时把非 null 的传过去，避免缓存失效
            var valid = enemies.FindAll(e => e != null);
            RoomManager.Instance.OnPlayerEnterRoom(roomData, valid);
    
        }
    }

    private void OnDrawGizmos()
    {
        if (roomData == null) return;
        if(isDebug==false)return;
        // 显示房间Rect
        if (roomData.showDebug)
        {
            Gizmos.color = roomData.debugColor;
            Gizmos.DrawWireCube(roomData.Center, roomData.Size);

            // 填充
            Gizmos.color = new Color(roomData.debugColor.r,
                                    roomData.debugColor.g,
                                    roomData.debugColor.b,
                                    0.1f);
            Gizmos.DrawCube(roomData.Center, roomData.Size);

            // 显示房间名称
#if UNITY_EDITOR
            GUIStyle style = new GUIStyle();
            style.normal.textColor = roomData.debugColor;
            style.alignment = TextAnchor.MiddleCenter;
            UnityEditor.Handles.Label(roomData.Center, roomData.roomName, style);
#endif
        }
    }

//    private void OnDrawGizmosSelected()
//    {
//        if (roomData == null) return;

//        // 显示房间Rect
//        if (roomData.showDebug)
//        {
//            Gizmos.color = roomData.debugColor;
//            Gizmos.DrawWireCube(roomData.Center, roomData.Size);

//            // 填充
//            Gizmos.color = new Color(roomData.debugColor.r,
//                                    roomData.debugColor.g,
//                                    roomData.debugColor.b,
//                                    0.1f);
//            Gizmos.DrawCube(roomData.Center, roomData.Size);

//            // 显示房间名称
//#if UNITY_EDITOR
//            GUIStyle style = new GUIStyle();
//            style.normal.textColor = roomData.debugColor;
//            style.alignment = TextAnchor.MiddleCenter;
//            UnityEditor.Handles.Label(roomData.Center, roomData.roomName, style);
//#endif
//        }
//    }
}