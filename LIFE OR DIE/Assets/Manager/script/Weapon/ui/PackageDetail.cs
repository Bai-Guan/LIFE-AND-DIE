using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PackageDetail : MonoBehaviour
{
    Transform UIitemImage;
    TextMeshProUGUI UIcenterText;
    TextMeshProUGUI UIbottomText1;
    TextMeshProUGUI UIbottomText2;
    TextMeshProUGUI UIbottomText3;

    //Dictionary<int, List<PackageTableItem>> staticData;
    PackageLocalItem packageLocalItem;

    private void Awake()
    {
          InitName();
        
        
    }
    void test()
    {
        InitDetail(2);
    }
    private void InitName()
    {
        UIitemImage = transform.Find("top/Image");
        UIcenterText = transform.Find("center/centerText").GetComponent<TextMeshProUGUI>();
        UIbottomText1 = transform.Find("bottom/text1").GetComponent <TextMeshProUGUI>();
        UIbottomText2 = transform.Find("bottom/text2").GetComponent<TextMeshProUGUI>();
        UIbottomText3 = transform.Find("bottom/text3").GetComponent<TextMeshProUGUI>();
    }

    public void InitDetail(int id)
    {
        
        PackageTableItem item = PackageInventoryService.Instance._itemDataCache[id];
        UIitemImage.GetComponent<Image>().sprite=item.itemImage;
        UIcenterText.text = item.name;
        UIbottomText1.text = item.description;
       UIbottomText2.text = item.skillDescript;
       UIbottomText3.text = item.attribute;
    }
}
