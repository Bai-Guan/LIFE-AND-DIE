using UnityEditor.EditorTools;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private RoomDataSO roomData;

    private BoxCollider2D boxCollider;
    private bool hasTriggered = false;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            RoomManager.Instance?.ActivateRoom(roomData);
            hasTriggered = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (roomData == null) return;

        // 绘制触发器
        Gizmos.color = Color.yellow;
        Vector3 size = boxCollider != null ? boxCollider.size : Vector3.one;
        Gizmos.DrawWireCube(transform.position, size);

        // 绘制到房间的连线
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, roomData.Center);
    }

    private void OnDrawGizmosSelected()
    {
        if (roomData == null) return;

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
}