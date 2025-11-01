using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class DieComponent : MonoBehaviour
{
    private InitEnemySystem body;
    private Explodable explodable;

    private bool CanExploable=true;
    private bool FirstTime=true;
    private void Awake()
    {
        body = GetComponent<InitEnemySystem>();
      explodable = GetComponent<Explodable>();
        if (body == null)
        {
            Debug.LogWarning("��ǰ����δװ�г�ʼ����,����ģ��ʧЧ");
            this.enabled = false;
            return;
        }
        if (explodable == null)
        {
            Debug.LogWarning("��ǰ����δװ����Ƭ��,������ƬЧ������");
            CanExploable = false;
          
        }
    }
    private void Start()
    {
        body.Die += SpecialDieEffect;
    }
    public void SpecialDieEffect()
    {
        //�ٴμ�⵱ǰ�����Ƿ����� �Ƿ��Ѿ�����
        if (!(body.CurrentHP <= 0)) return;
        if(FirstTime==false) return;
        FirstTime = false;
        //���������߼�����

        //-----------------���濪����Ƭ����-----
        if (CanExploable)
        {
            print("������Ƭ");
            explodable.explode(body.LastDamage,body.LastDir);
        }
    }

    private void OnDisable()
    {
        body.Die -= SpecialDieEffect;
    }
}
