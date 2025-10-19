using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    // 技能字典：SkillName -> 对应的技能方法
    private Dictionary<SkillName, System.Action<SkillContext>> skillDictionary;

    [System.Serializable]
    public class SkillContext
    {
        public GameObject user;           // 使用者
        public GameObject target;         // 目标（可为null）
        public PackageTableItem item;     // 触发技能的物品
        public Vector3 position;          // 使用位置
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

    // 初始化技能字典
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


    // 执行技能
    public void ExecuteSkill(SkillName skillName, SkillContext context)
    {
        if (skillDictionary.ContainsKey(skillName) && skillDictionary[skillName] != null)
        {
            skillDictionary[skillName].Invoke(context);
        }
        else if (skillName != SkillName.Null)
        {
            Debug.LogWarning($"技能 {skillName} 未在字典中注册");
        }
    }

    // ========== 具体技能实现 ==========
    //剑成长效果 
    private void ForgeSwordsFromFleshSkill(SkillContext context)
    {
       
    }



    // 火焰剑燃烧效果
    private void FireSwordBurnSkill(SkillContext context)
    {
        //if (context.target != null)
        //{
        //    var enemy = context.target.GetComponent<EnemyController>();
        //    if (enemy != null)
        //    {
        //        enemy.ApplyBurnEffect(5f, 3f); // 每秒5伤害，持续3秒
        //        Debug.Log($"火焰剑效果：敌人开始燃烧！");
        //    }
        //}
    }



    // 生命恢复护甲
    private void HealthRegenArmorSkill(SkillContext context)
    {
        //var player = context.user.GetComponent<PlayerHealth>();
        //if (player != null)
        //{
        //    player.StartHealthRegeneration(2f, 10f); // 每秒恢复2生命，持续10秒
        //    Debug.Log($"恢复护甲效果：开始恢复生命值！");
        //}
    }

   
}