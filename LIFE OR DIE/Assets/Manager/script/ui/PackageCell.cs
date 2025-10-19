using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PackageCell : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
   [SerializeField] Transform select1;
    [SerializeField] Transform select2;
    [SerializeField] Transform obj;
    [SerializeField] Transform numBG;
    int _id;
    public int ID {  get { return _id; } }
    int objNum;
    string _name;
    
    [SerializeField] TextMeshProUGUI _Textname;
    [SerializeField] TextMeshProUGUI _Textnum;

    Transform TranClick;
    Transform TranPass;

    Animator AnimClick;
    Animator AnimPass;

    public static Action<PackageCell> onAnyClicked;
    public static PackageCell _current = null;
    private void Awake()
    {
       
        InitName();
        


    }
    private void showData()
    {
        _Textname.text= _name;
        _Textnum.text= objNum.ToString();
        if(objNum>1)
        {
            numBG.gameObject.SetActive(true);
        }
    }


    private void InitName()
    {
        select1 = transform.Find("select1");
        select2 = transform.Find("select2");
        obj = transform.Find("object");
        _Textname = transform.Find("name").GetComponent<TextMeshProUGUI>();
        _Textnum = transform.Find("numBG/num").GetComponent<TextMeshProUGUI>();
        numBG = transform.Find("numBG");

        TranClick = transform.Find("Anim/click");
        TranPass =  transform.Find("Anim/show");

        AnimClick = transform.Find("Anim/click").GetComponent<Animator>();
        AnimPass = transform.Find("Anim/show").GetComponent<Animator>();

     
    }

    private void InitSet()
    {
        TranClick.gameObject.SetActive(false);
        TranPass.gameObject.SetActive(false);
        select1.gameObject.SetActive(false);
        select2.gameObject.SetActive(false);
    }

    public void Set(int id,int num,string name,Sprite sprite)
    {
        _id = id;
        objNum = num;
        _name = name;
        if (obj == null) { Debug.LogError("OBJÎª¿Õ£¡");return;}
        obj.GetComponent<Image>().sprite= sprite;
        showData();
    }

    public void Set(PackageCell cell)
    {
        _id = cell._id;
        objNum =cell.objNum;
        _name = cell._name;
        obj.GetComponent<Image>().sprite = cell.obj.GetComponent<Image>().sprite;
        showData();
    }

    



    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointClick" + eventData.ToString());
        _current = this;
        onAnyClicked?.Invoke(this);
      
        
        AnimClick.SetTrigger("Click");
        
    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointEnter" + eventData.ToString());

        TranClick.gameObject.SetActive(true);
        TranPass.gameObject.SetActive(true);

        AnimPass.SetTrigger("In");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointExit" + eventData.ToString());

        
        TimeManager.Instance.OneTime(0.2f,
            () =>
            {
                TranClick.gameObject.SetActive(false);
                TranPass.gameObject.SetActive(false);
            }
            );
        

        AnimPass.SetTrigger("Out");
    }
}
