using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("当前激活的房间")]
    [SerializeField] private RoomDataSO currentRoom;

    [Header("遮蔽设置")]
    [SerializeField] private LayerMask roomLayers = -1;

    // 缓存每个房间中的对象
    private Dictionary<RoomDataSO, List<GameObject>> roomObjects = new Dictionary<RoomDataSO, List<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        //FindAllRoomObjects();
    }

    // 查找所有房间中的对象并缓存
    private void FindAllRoomObjects()
    {
        // 查找所有RoomDataSO资源
        var allRooms = Resources.FindObjectsOfTypeAll<RoomDataSO>();

        foreach (var room in allRooms)
        {
            CacheRoomObjects(room);
        }
    }

    private void CacheRoomObjects(RoomDataSO room)
    {
        // 查找房间Rect内的所有对象
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            room.Center,
            room.Size,
            0f,
            roomLayers
        );

        List<GameObject> objects = new List<GameObject>();
        foreach (var collider in colliders)
        {
            if (!objects.Contains(collider.gameObject))
            {
                objects.Add(collider.gameObject);
            }
        }

        roomObjects[room] = objects;

        // 初始隐藏所有房间（除了当前房间）
        if (room != currentRoom)
        {
            SetRoomVisible(room, false);
        }
    }

    public void ActivateRoom(RoomDataSO room)
    {
        if (room == null || room == currentRoom) return;

        // 隐藏当前房间
        if (currentRoom != null)
        {
            SetRoomVisible(currentRoom, false);
        }
        //切换摄像头
        CameraManager.Instance.SetCameraBounds(room.visibleRect);

        // 显示新房间
        currentRoom = room;
        room.isDiscovered = true;
        SetRoomVisible(room, true);


        // 触发房间切换事件
        OnRoomChanged?.Invoke(room);
    }

    private void SetRoomVisible(RoomDataSO room, bool visible)
    {
        if (roomObjects.TryGetValue(room, out var objects))
        {
            foreach (var obj in objects)
            {
                if (obj != null)
                {
                    SetObjectVisibility(obj, visible);
                }
            }
        }
    }

    private void SetObjectVisibility(GameObject obj, bool visible)
    {
        // 方式1：直接启用/禁用对象（最彻底）
        obj.SetActive(visible);

        // 方式2：控制渲染器（保留碰撞等）
        /*
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = visible;
        }
        */
    }

    // 刷新房间对象缓存（如果场景动态加载了对象）
    public void RefreshRoomCache(RoomDataSO room)
    {
        if (room != null)
        {
            CacheRoomObjects(room);
        }
    }

    public RoomDataSO GetCurrentRoom()
    {
        return currentRoom;
    }

    // 事件
    public delegate void RoomChangedHandler(RoomDataSO newRoom);
    public event RoomChangedHandler OnRoomChanged;
}