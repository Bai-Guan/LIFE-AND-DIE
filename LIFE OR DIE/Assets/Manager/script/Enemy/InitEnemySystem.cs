using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InitEnemySystem : MonoBehaviour
{
  [SerializeField]private EnemyBaseData baseData;
    public string BiologicalName { get { return baseData.Name; } }
    private Rigidbody2D rb;

    [SerializeField] private int currentHP=1;
  public int CurrentHP {  get { return currentHP; } private set { } }
    private int defense=0;
    public int Defenese { get { return defense; } private set { } }
    private int knocked=0;
    public int Knocked { get { return defense; } private set { } }
    public event Action<DamageData,GameObject> beAttacked;
    public event Action BeAttack;
    public event Action AfterDamagedMath;
    public event Action<GameObject> Die;

    public DamageData LastDamage = new DamageData();
    public GameObject LastAttacker;
    public Vector2 LastDir;

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
    //此方法目前只在受伤组件中调用 
    public void BeAttacked()
    {
        BeAttack?.Invoke();
    }
    public void AfterDamage()
    {
        AfterDamagedMath?.Invoke();
    }

    private void BeDied()
    {
        Die?.Invoke(LastAttacker);
    }


    public void MinusHP(int hp)
    {
        currentHP-=hp;
    }
    public void SetRBvelcoity(Vector2 vector2)
    {
        rb.velocity=new Vector2(vector2.x,vector2.y);
    }
    public void SetLastDamageData(DamageData data,Vector2 Dir2)
    {
        LastDamage=data;
        LastDir = Dir2;
    }
    public void SetLastAttacker(GameObject Object)
    {
        LastAttacker=Object;
    }
   private void Update()
    {
        //临时代码
        if(currentHP <= 0)BeDied();
    }
}
