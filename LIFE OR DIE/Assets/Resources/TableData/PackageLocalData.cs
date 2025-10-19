using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PackageLocalData
{
    public string FilePath = "D:\\UnityProjectFile\\LIFE OR DIE\\LIFE-AND-DIE\\LIFE OR DIE\\Assets\\Save";
    public string SaveName = "saveUnity.json";
    public string FileTestPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), // 桌面
   "saveUnity.json");

    public PackageSaveData saveData = new PackageSaveData();


    [MenuItem("CMCmd/创建背包测试数据")]
    public void savePackage()
    {
        if (saveData.localAllItems == null)
        { 
            Directory.CreateDirectory(Path.GetDirectoryName(FileTestPath));
        File.WriteAllText(FileTestPath, "noSave");
            Debug.Log("存档失败");
        }
        
        string inventoryJson = JsonUtility.ToJson(saveData, true);
        Directory.CreateDirectory(Path.GetDirectoryName(FileTestPath));
        File.WriteAllText(FileTestPath, inventoryJson);
        Debug.Log("存档成功");
     }

    [MenuItem("CMCmd/读取背包测试数据")]
    public List<PackageLocalItem> LoadPackage()
    {

        if (saveData.localAllItems != null)
        {
            return saveData.localAllItems;
        }
        if (!File.Exists(FileTestPath))
        { 
            Debug.Log("无档可读");
            saveData.localAllItems = new List<PackageLocalItem>();
            return null;
        }
        if (FileTestPath != null)
        {
            string inventoryJson = File.ReadAllText(FileTestPath);

            Debug.Log(inventoryJson);
          
            saveData.localAllItems = JsonUtility.FromJson<PackageSaveData>(inventoryJson).localAllItems;
            Debug.Log("读档成功");
            return saveData.localAllItems;
        }
        else
        {
            return null;
        }



    }
   
}
[System.Serializable]
public class PackageSaveData
{

    public List<PackageLocalItem> localAllItems;

}

[System.Serializable]
    public class PackageLocalItem
    {
        public string uid;
        public itemType type;
        public int id;
        public int count = 1;       // 物品数量（默认1）
        private bool isStackable = false; // 是否可叠加
      public bool IsStackable
      {
        get
        {
            // 食物可叠加，武器护甲不可叠加
            return type == itemType.Food;
        }
      }
      public override string ToString()
      {
            return string.Format("[id]={0},[name]={1},[num]={2}", id, uid,count);

      }
    }

