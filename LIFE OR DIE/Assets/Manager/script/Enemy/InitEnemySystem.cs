using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InitEnemySystem : MonoBehaviour
{
  [SerializeField]private EnemyBaseData baseData;
    private Rigidbody2D rb;

    private int currentHP=1;
    private int defense=0;
    public int Defenese { get { return defense; } private set { } }
    private int knocked=0;
    public int Knocked { get { return defense; } private set { } }
    public event Action<DamageData,GameObject> beAttacked;


    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        if(rb == null) {this.transform.AddComponent<Rigidbody2D>();}    
    }

  private  void Start()
    {
        InitSOData();
    }
    private void InitName()
    {

    }

  private  void InitSOData()
    {
        if(baseData == null)
        {
            Debug.LogWarning("当前怪物未进行基础数值初始化"+this.gameObject.name);
            this.gameObject.SetActive(false);
            return;
        }
        currentHP = baseData.MaxHP;
        defense = baseData.Defense;
        knocked = baseData.KnockBackResistance;
    }

    public void BeAttacked(DamageData damage,GameObject obj)
    {
        beAttacked?.Invoke(damage,obj);
    }

    public void MinusHP(int hp)
    {
        currentHP-=hp;
    }
    public void SetRBvelcoity(Vector2 vector2)
    {
        rb.velocity=new Vector2(vector2.x,vector2.y);
    }
    
   private void Update()
    {
        
    }
}
