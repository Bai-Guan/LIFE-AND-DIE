using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DamagedComponent : MonoBehaviour, IBeDamaged
{
    private InitEnemySystem body;
    private bool isMinusHP =true;
    private bool isCanRepel = true;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        body = GetComponent<InitEnemySystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(body==null)
        {
            Debug.LogWarning("��ǰ����δװ�г�ʼ����,�ܻ�ģ��ʧЧ");
            this.enabled = false;
            return;
        }
    }

    private void Start()
    {
        body.beAttacked += ReturnRBandHP;
    }

    void ReturnRBandHP(DamageData data, GameObject attacker)
    {
        OnHurt(data, attacker);
       
    }

    public void OnHurt(DamageData data, GameObject attacker)
    {
        if (data == null)
        {
            body.MinusHP(0);
            body.SetRBvelcoity(new Vector2(0, 0));
            Debug.LogWarning("���빥��Ϊ��");
            return;
        }

        // 1. �˺�
        int damage = data.type switch
        {
            DamageType.physics => Mathf.Max(1, data.atk - body.Defenese),
            DamageType.magic => data.atk,
            _ => data.atk
        };

        // 2. ����ǿ��
        float resist = Mathf.Clamp01(body.Knocked * 0.01f);
        float scale = 1f - resist;

        // 3. �������
        Vector2 attackerPos = attacker.transform.position;
        Vector2 victimPos = transform.position;

        // X�����ҷ���
        float dirX = Mathf.Sign(victimPos.x - attackerPos.x);

        // Y�����·��򣨴��ݴ�
        float verticalThreshold = 0.5f; // ��λ����
        float dirY = attackerPos.y - victimPos.y > verticalThreshold ? -1f   // ���Ϸ����� �� ���»���
                                                                     : 1f;  // ����Ĭ������

        // �õ�����
        Vector2 dir2= new Vector2(dirX, dirY);

        // 4. �����ٶ�
        float vx = data.RepellingXSpeed * scale * dirX;
        float vy = data.RepellingYSpeed * scale * dirY;

        Vector2 v = new Vector2(vx, vy);

        if (isMinusHP == false) { damage = 0; }
        if (isCanRepel == false) { v.Set(0, 0); }

        //5.�ı�����
        Debug.Log("���" + damage + "�˺�");
     
        body.MinusHP(damage);
        body.SetRBvelcoity(v);
        body.SetLastDamageData(data, dir2);
        //6.������Ч ��Ч
        // Debug.Log(this.name + "��������");
        AudioManager.Instance.PlaySFX(AudioManager._���ܻ���Ч);
        //���� ������Ч�ɹ���������� �˴���д��weapon������
        //  EffectManager.Instance.SpeicalEffectKnife(this.transform, 0.8f, 5f);
        // EffectManager.Instance.ChromaticAberrationSet(0.8f, 1f);
        //  EffectManager.Instance.VerticalBlur(0.8f, 0.9f);
        //InvincibleRendered(0.8f);
        //CameraManager.Instance.CameraShake(2f, 1f);
    }
    //��������ɫ��˸
    public void InvincibleRendered(float t)
    {
        TimeManager.Instance.FrameTime(t,
            () =>
            {
                float e = Mathf.PingPong(Time.time * 4f, 0.7f);
                Color color = new Color(e, e, e);
                spriteRenderer.color = color;


            },
            () =>
            {
                spriteRenderer.color = Color.black;
            }
            );
    }

    private void OnDisable()
    {
        body.beAttacked -= ReturnRBandHP;
    }


}
