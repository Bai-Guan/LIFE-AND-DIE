using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    // �����ֵ䣺SkillName -> ��Ӧ�ļ��ܷ���
    private Dictionary<SkillName, System.Action<SkillContext>> skillDictionary;

    [System.Serializable]
    public class SkillContext
    {
        public GameObject user;           // ʹ����
        public GameObject target;         // Ŀ�꣨��Ϊnull��
        public PackageTableItem item;     // �������ܵ���Ʒ
        public Vector3 position;          // ʹ��λ��
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeSkillDictionary();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ��ʼ�������ֵ�
    private void InitializeSkillDictionary()
    {
        skillDictionary = new Dictionary<SkillName, System.Action<SkillContext>>()
        {
            { SkillName.Null, null },

            { SkillName.FireSwordBurn, FireSwordBurnSkill },
           
            { SkillName.HealthRegenArmor, HealthRegenArmorSkill },

            {SkillName.ForgeSwordsFromFlesh, ForgeSwordsFromFleshSkill}
          
        };
    }


    // ִ�м���
    public void ExecuteSkill(SkillName skillName, SkillContext context)
    {
        if (skillDictionary.ContainsKey(skillName) && skillDictionary[skillName] != null)
        {
            skillDictionary[skillName].Invoke(context);
        }
        else if (skillName != SkillName.Null)
        {
            Debug.LogWarning($"���� {skillName} δ���ֵ���ע��");
        }
    }

    // ========== ���弼��ʵ�� ==========
    //���ɳ�Ч�� 
    private void ForgeSwordsFromFleshSkill(SkillContext context)
    {
       
    }



    // ���潣ȼ��Ч��
    private void FireSwordBurnSkill(SkillContext context)
    {
        //if (context.target != null)
        //{
        //    var enemy = context.target.GetComponent<EnemyController>();
        //    if (enemy != null)
        //    {
        //        enemy.ApplyBurnEffect(5f, 3f); // ÿ��5�˺�������3��
        //        Debug.Log($"���潣Ч�������˿�ʼȼ�գ�");
        //    }
        //}
    }



    // �����ָ�����
    private void HealthRegenArmorSkill(SkillContext context)
    {
        //var player = context.user.GetComponent<PlayerHealth>();
        //if (player != null)
        //{
        //    player.StartHealthRegeneration(2f, 10f); // ÿ��ָ�2����������10��
        //    Debug.Log($"�ָ�����Ч������ʼ�ָ�����ֵ��");
        //}
    }

   
}