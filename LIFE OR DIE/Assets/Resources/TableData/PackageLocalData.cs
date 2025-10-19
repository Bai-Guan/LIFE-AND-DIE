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
    public string FileTestPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), // ����
   "saveUnity.json");

    public PackageSaveData saveData = new PackageSaveData();


    [MenuItem("CMCmd/����������������")]
    public void savePackage()
    {
        if (saveData.localAllItems == null)
        { 
            Directory.CreateDirectory(Path.GetDirectoryName(FileTestPath));
        File.WriteAllText(FileTestPath, "noSave");
            Debug.Log("�浵ʧ��");
        }
        
        string inventoryJson = JsonUtility.ToJson(saveData, true);
        Directory.CreateDirectory(Path.GetDirectoryName(FileTestPath));
        File.WriteAllText(FileTestPath, inventoryJson);
        Debug.Log("�浵�ɹ�");
     }

    [MenuItem("CMCmd/��ȡ������������")]
    public List<PackageLocalItem> LoadPackage()
    {

        if (saveData.localAllItems != null)
        {
            return saveData.localAllItems;
        }
        if (!File.Exists(FileTestPath))
        { 
            Debug.Log("�޵��ɶ�");
            saveData.localAllItems = new List<PackageLocalItem>();
            return null;
        }
        if (FileTestPath != null)
        {
            string inventoryJson = File.ReadAllText(FileTestPath);

            Debug.Log(inventoryJson);
          
            saveData.localAllItems = JsonUtility.FromJson<PackageSaveData>(inventoryJson).localAllItems;
            Debug.Log("�����ɹ�");
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
        public int count = 1;       // ��Ʒ������Ĭ��1��
        private bool isStackable = false; // �Ƿ�ɵ���
      public bool IsStackable
      {
        get
        {
            // ʳ��ɵ��ӣ��������ײ��ɵ���
            return type == itemType.Food;
        }
      }
      public override string ToString()
      {
            return string.Format("[id]={0},[name]={1},[num]={2}", id, uid,count);

      }
    }

