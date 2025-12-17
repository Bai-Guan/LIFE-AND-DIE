using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    private RoomDataSO currentRoom;

    // 当前房间活着的怪
    private List<GameObject> currentEnemies = new List<GameObject>();

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // 由 RoomTrigger 调用
    public void OnPlayerEnterRoom(RoomDataSO room, List<GameObject> enemyList)
    {
        if (room == null || room == currentRoom) return;

        // 1. 切摄像机
        CameraManager.Instance.SetCameraBounds(room.visibleRect);

        // 2. 休眠旧房间
        SetActiveList(currentEnemies, false);
        currentEnemies.Clear();

        // 3. 激活新房间
        currentRoom = room;
        currentEnemies = new List<GameObject>(enemyList);
        SetActiveList(currentEnemies, true);

        // 4. 事件
        OnRoomChanged?.Invoke(room);
    }

    private void SetActiveList(List<GameObject> list, bool active)
    {
        foreach (var r in list)
        {
            if (r == null) continue;

            r.SetActive(active);
            if (active)
            {
                // 安全获取接口
                if (r.TryGetComponent(out IEnemyReset er))
                    er.EnemyReset();
            }
        }
    }

    public delegate void RoomChangedHandler(RoomDataSO newRoom);
    public event RoomChangedHandler OnRoomChanged;
}