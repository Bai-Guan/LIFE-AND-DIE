using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GMcommnd 
{
    [MenuItem("CMCmd/∂¡»°±Ì∏Ò")]
    public static void ReatTable()
    {
        PackageTable packgeTable = Resources.Load<PackageTable>("TableData/packageTable");
        foreach (PackageTableItem tableItem in packgeTable.DataList)
        {
            Debug.Log(string.Format("[id]={0},[name]={1}",tableItem.id,tableItem.name));
        }
    }
}
