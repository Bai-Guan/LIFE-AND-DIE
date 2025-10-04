using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//受伤与格挡的切换在 playerControl类进行切换 因为属于随时可切换类型
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance {  get { return _instance; } }

    public Image HP;
    private void Awake()
    {
        if(_instance != null&&_instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHPUI(int hp,int MAXhp)
    {
        if(hp<=MAXhp) 
        HP.fillAmount =(float) hp/MAXhp;
        else if(hp>MAXhp)HP.fillAmount = 1;
    }
}
