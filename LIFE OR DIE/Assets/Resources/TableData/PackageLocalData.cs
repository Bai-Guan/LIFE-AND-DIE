using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageLocalData
{
    private static PackageLocalData _instance;
    public static PackageLocalData Instance
    {
        get
        {
            if (_instance == null) _instance = new PackageLocalData();

            return _instance;

        }
    }

    public List<PackageloaclItem> loaclItems;

    public void savePackage()
    {
        string inventoryJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PackageLocalData", inventoryJson);
        PlayerPrefs.Save();
    }

    //public List<PackageloaclItem> LoadPackage() {
    //    {
    //        if(loaclItems!=null)
    //        {
    //            return loaclItems;
    //        }
    //        if(PlayerPrefs.HasKey("PackageLocalData"))
    //        {
    //            string inventoryJson = PlayerPrefs.GetString("PackageLocalData");
    //            PackageLocalData packageLocalData = JsonUtility.FromJson<PackageLocalData>(inventoryJson);
    //        }
    //    }

    //public void savePackage()
    //{
    
    //}
}

[System.Serializable]
public class PackageloaclItem
{
    public string uid;
    public int id;
    public int num;
    public override  string ToString()
    {
       return string.Format("[id]={0},[num]={1}", id, num);
        
    }
}
