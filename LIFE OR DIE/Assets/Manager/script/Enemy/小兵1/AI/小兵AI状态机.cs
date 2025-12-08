using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 小兵AI状态机 : MonoBehaviour
{
    //[Header("巡逻设置")]
    //public Transform patrolPointA;
    //public Transform patrolPointB;
    //public float patrolSpeed = 2f;
    //public float waitTimeAtPoints = 1f;

    //[Header("战斗设置")]
    //public float chaseSpeed = 3.5f;
    //public float attackRange = 2f;
    //public float chargeAttackRange = 7f;
    //[Range(0, 1)] public float chargeAttackProbability = 0.2f;
    //[Range(0, 1)] public float slashProbability = 0.7f;

    //[Header("返回设置")]
    //public float returnDistance = 3f;
    //public float returnWaitTime = 2f;

    //// 组件引用
    //private EnemyRadioGraphic detector;
    //private Rigidbody2D rb;
    //private Animator animator;

    //// 状态管理
    //private enum AIState { Patrol, Combat, Return, Victory }
    //private AIState currentState = AIState.Patrol;

    //// 巡逻变量
    //private Transform currentPatrolTarget;
    //private bool isWaiting = false;
    //private int killCount = 0;

    //// 战斗变量
    //private bool isBlocking = false;
    //private float lastAttackTime = 0f;
    //public float attackCooldown = 1f;

    //void Start()
    //{
    //    detector = GetComponent<EnemyRadioGraphic>();
    //    rb = GetComponent<Rigidbody2D>();
    //    animator = GetComponent<Animator>();

    //    // 初始化巡逻
    //    if (patrolPointA != null && patrolPointB != null)
    //    {
    //        currentPatrolTarget = patrolPointA;
    //    }
    //    else
    //    {
    //        // 如果没有设置巡逻点，就站在原地
    //        currentState = AIState.Patrol;
    //        currentPatrolTarget = null;
    //    }
    //}

    //void Update()
    //{
    //    // 更新朝向
    //    UpdateFacingDirection();

    //    // 状态机
    //    switch (currentState)
    //    {
    //        case AIState.Patrol:
    //            PatrolState();
    //            break;
    //        case AIState.Combat:
    //            CombatState();
    //            break;
    //        case AIState.Return:
    //            // 在协程中处理，这里不需要更新
    //            break;
    //        case AIState.Victory:
    //            VictoryState();
    //            break;
    //    }

    //    // 更新动画参数
    //    UpdateAnimator();
    //}

    //void PatrolState()
    //{
    //    // 检测到玩家，进入战斗状态
    //    if (detector.IsPlayerVisible)
    //    {
    //        currentState = AIState.Combat;
    //        return;
    //    }

    //    // 如果没有巡逻点，就站在原地
    //    if (currentPatrolTarget == null) return;

    //    // 如果正在等待，不执行移动
    //    if (isWaiting) return;

    //    // 移动向当前巡逻点
    //    Vector2 direction = (currentPatrolTarget.position - transform.position).normalized;
    //    rb.velocity = new Vector2(direction.x * patrolSpeed, rb.velocity.y);

    //    // 检查是否到达巡逻点
    //    float distance = Vector2.Distance(transform.position, currentPatrolTarget.position);
    //    if (distance < 0.1f)
    //    {
    //        StartCoroutine(WaitAtPatrolPoint());
    //    }
    //}

    //IEnumerator WaitAtPatrolPoint()
    //{
    //    isWaiting = true;
    //    rb.velocity = Vector2.zero;

    //    yield return new WaitForSeconds(waitTimeAtPoints);

    //    // 切换巡逻目标
    //    if (currentPatrolTarget == patrolPointA)
    //        currentPatrolTarget = patrolPointB;
    //    else
    //        currentPatrolTarget = patrolPointA;

    //    isWaiting = false;
    //}

    //void CombatState()
    //{
    //    // 玩家不可见，返回巡逻
    //    if (!detector.IsPlayerVisible)
    //    {
    //        currentState = AIState.Patrol;
    //        return;
    //    }

    //    // 检查玩家是否死亡（需要你自己实现玩家死亡检测）
    //    if (IsPlayerDead())
    //    {
    //        killCount++;
    //        if (killCount == 1)
    //        {
    //            StartCoroutine(ReturnAfterKill());
    //        }
    //        else if (killCount >= 2)
    //        {
    //            currentState = AIState.Victory;
    //        }
    //        return;
    //    }

    //    // 移动向玩家
    //    Vector2 direction = (detector.PlayerPosition - (Vector2)transform.position).normalized;
    //    float distanceToPlayer = Vector2.Distance(transform.position, detector.PlayerPosition);

    //    // 根据距离决定行为
    //    if (distanceToPlayer > attackRange)
    //    {
    //        // 追逐玩家
    //        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);

    //        // 在冲锋攻击范围内，有概率使用冲锋攻击
    //        if (distanceToPlayer <= chargeAttackRange &&
    //            distanceToPlayer > attackRange &&
    //            Random.value < chargeAttackProbability &&
    //            Time.time > lastAttackTime + attackCooldown)
    //        {
    //            PerformChargeAttack();
    //        }
    //    }
    //    else
    //    {
    //        // 在攻击范围内，停止移动并攻击
    //        rb.velocity = new Vector2(0, rb.velocity.y);

    //        // 检查是否可以攻击
    //        if (Time.time > lastAttackTime + attackCooldown)
    //        {
    //            // 读指令：如果玩家正在攻击，则格挡
    //            if (IsPlayerAttacking())
    //            {
    //                PerformBlock();
    //            }
    //            else
    //            {
    //                // 否则根据概率选择攻击方式
    //                if (Random.value < slashProbability)
    //                {
    //                    PerformSlashAttack();
    //                }
    //                else
    //                {
    //                    PerformChargeAttack();
    //                }
    //            }
    //        }
    //    }
    //}

    //IEnumerator ReturnAfterKill()
    //{
    //    currentState = AIState.Return;

    //    // 背对玩家
    //    bool playerIsLeft = detector.PlayerPosition.x < transform.position.x;
    //    detector.SetFlip(!playerIsLeft);

    //    // 往回走一段距离
    //    Vector2 returnDirection = playerIsLeft ? Vector2.right : Vector2.left;
    //    Vector2 returnTarget = (Vector2)transform.position + returnDirection * returnDistance;

    //    float startTime = Time.time;
    //    while (Vector2.Distance(transform.position, returnTarget) > 0.1f && Time.time < startTime + 3f)
    //    {
    //        rb.velocity = new Vector2(returnDirection.x * patrolSpeed, rb.velocity.y);
    //        yield return null;
    //    }

    //    rb.velocity = Vector2.zero;

    //    // 等待一段时间
    //    yield return new WaitForSeconds(returnWaitTime);

    //    // 返回巡逻状态
    //    currentState = AIState.Patrol;
    //}

    //void VictoryState()
    //{
    //    // 停止移动
    //    rb.velocity = Vector2.zero;

    //    // 盯着玩家
    //    if (detector.IsPlayerVisible)
    //    {
    //        bool playerIsLeft = detector.PlayerPosition.x < transform.position.x;
    //        detector.SetFlip(!playerIsLeft);
    //    }
    //}

    //void UpdateFacingDirection()
    //{
    //    // 根据速度方向更新朝向
    //    if (Mathf.Abs(rb.velocity.x) > 0.1f)
    //    {
    //        bool facingLeft = rb.velocity.x < 0;
    //        detector.SetFlip(facingLeft);
    //    }
    //}

    //void UpdateAnimator()
    //{
    //    if (animator == null) return;

    //    animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    //    animator.SetBool("IsBlocking", isBlocking);
    //    animator.SetInteger("KillCount", killCount);
    //}

    //// 攻击和格挡方法
    //void PerformSlashAttack()
    //{
    //    lastAttackTime = Time.time;
    //    animator.SetTrigger("SlashAttack");
    //    // 这里可以添加攻击逻辑，如伤害检测等
    //}

    //void PerformChargeAttack()
    //{
    //    lastAttackTime = Time.time;
    //    animator.SetTrigger("ChargeAttack");
    //    // 这里可以添加冲锋攻击逻辑
    //}

    //void PerformBlock()
    //{
    //    isBlocking = true;
    //    animator.SetTrigger("Block");

    //    // 格挡一段时间后结束
    //    StartCoroutine(EndBlocking());
    //}

    //IEnumerator EndBlocking()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    isBlocking = false;
    //}

    //// 需要你根据游戏实现的方法
    //bool IsPlayerDead()
    //{
    //    // 实现玩家死亡检测逻辑
    //    // 例如：return playerHealth <= 0;
    //    return false;
    //}

    //bool IsPlayerAttacking()
    //{
    //    // 实现玩家攻击检测逻辑
    //    // 例如：检查玩家是否在攻击动画中或有攻击输入
    //    return false;
    //}

    //// 可视化巡逻点
    //void OnDrawGizmosSelected()
    //{
    //    if (patrolPointA != null && patrolPointB != null)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawWireSphere(patrolPointA.position, 0.3f);
    //        Gizmos.DrawWireSphere(patrolPointB.position, 0.3f);
    //        Gizmos.DrawLine(patrolPointA.position, patrolPointB.position);
    //    }

    //    // 显示攻击范围
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, chargeAttackRange);
    //}
}
