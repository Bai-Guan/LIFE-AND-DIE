using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������񵲵��л��� playerControl������л� ��Ϊ������ʱ���л�����
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
