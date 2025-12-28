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

   public int MaxHp { get { return baseData.MaxHP; } }
   
    [SerializeField] private int currentHP=1;
  public int CurrentHP {  get { return currentHP; } private set { } }
    private int defense=0;
    public int Defenese { get { return defense; } private set { } }
   
    public int Knocked { get { return baseData.KnockBackResistance; } private set { } }
    public event Action<DamageData,GameObject> beAttacked;
    public event Action BeAttack;
    public event Action AfterDamagedMath;
    public event Action<GameObject> Die;

    public DamageData LastDamage = new DamageData();
    public GameObject LastAttacker;
    public Vector2 LastDir;
    private SpriteRenderer spirteRenderer;

   [SerializeField] private int facingleft=-1;
    private bool _isfacingleft;
    public bool isFacingLeft
    {
        get
        {
            if (facingleft == -1)
            {
                _isfacingleft = true;
                return true;
            }
            else if (facingleft == 1)
            { 
            _isfacingleft= false;
            return false;
             }
            return _isfacingleft;
           
        }
    }

    private bool canBackstab = true;
    public bool 是否可以背刺 { get { return canBackstab; } }
    private void Awake()
    {
     
        InitName();
    }

  private  void Start()
    {
        InitSOData();
    }
    private void InitName()
    {
        rb = this.GetComponent<Rigidbody2D>();
        if (rb == null) { this.transform.AddComponent<Rigidbody2D>(); }

        spirteRenderer =GetComponent<SpriteRenderer>();
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

    public void ResetHP()
    {
        currentHP=MaxHp;
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
    //public void SetFilp(bool tf)
    //{
    //    facingleft = tf?-1:1;
    //}
    public void SetFilp(float dir)
    {
        facingleft=(int)dir;
    }
    public void SetBackstab(bool tf)
    {
        canBackstab=tf;
    }
   private void Update()
    {
        //临时代码
        spirteRenderer.flipX=isFacingLeft;
        if(currentHP <= 0)BeDied();
    }
}
