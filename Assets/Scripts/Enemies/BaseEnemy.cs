using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour,IDamageable
{
    [Header("MoveMent")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;
    public List<Transform> targets;
    protected EnemyState state;
    private GameObject alertSign;
    public Animator animator;

    [Header("AttackSetting")]
    public float attackRange;
    public float skillRange;
    public float waitTime;
    private bool canAttack = true;
    protected bool canSkill = true;

    [Header("State")]
    public float headth;
    public bool isDead = false;
    public bool isBoss = false;

    [Header("Audio")]
    public AudioClip attackClip;
    public AudioClip skillClip;
    public AudioClip hitClip;

    void Start()
    {
        if (isBoss) UIManager.instance.SetBossHealth(headth);
        TransToState(new PatrolState());
        animator = GetComponent<Animator>();
        alertSign = transform.GetChild(0).gameObject;
        var hitPoint = transform.GetChild(1).gameObject;
        hitPoint.GetComponent<HitPoint>().onHit += onHitPointTrgger;
        GameManager.instance.AddEnemies(this);
    }

    void Update()
    {
        if (isBoss) UIManager.instance.UpdateBossHealth(headth);
        if (isDead) return;
        state.OnUpdate(this);
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="state"></param>
    public void TransToState(EnemyState state)
    {
        this.state = state;
        state.EnterState(this);
    }

    void FilpDirection()
    {
        var rotation = transform.position.x < targetPoint.position.x ? 0 : 180;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    public void MoveToTarget()
    {
        FilpDirection();
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (targets.Contains(collision.transform)) return;
        if (collision.CompareTag("Door")) return;
        targets.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targets.Remove(collision.transform);
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (alertSign.activeInHierarchy || isDead) return;
        alertSign.SetActive(true);
        var animTime = alertSign.GetComponent<Animator>()?.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        await Task.Delay(TimeSpan.FromSeconds((double)animTime));
        alertSign?.SetActive(false);
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    public virtual void AttackAction()
    {       
        if (Vector2.Distance(transform.position, targetPoint.position) > attackRange) return;
        if (!canAttack) return;
        if (attackClip != null) AudioSource.PlayClipAtPoint(attackClip, transform.position);
        animator.SetTrigger("attack");
        canAttack = false;
        Task.Factory.StartNew(async() => 
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            canAttack = true;
        });
    }

    /// <summary>
    /// 技能攻击
    /// </summary>
    public virtual void SkillAction()
    {
        if (Vector2.Distance(transform.position, targetPoint.position) > skillRange) return;
        if (!canSkill) return;
        if (skillClip != null) AudioSource.PlayClipAtPoint(skillClip, transform.position);
        animator.SetTrigger("skill");
        canSkill = false;
        Task.Factory.StartNew(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            canSkill = true;
        });
    }

    public void GetHit(float damage)
    {
        if (hitClip != null) AudioSource.PlayClipAtPoint(hitClip, transform.position);
        headth -= damage;
        animator.SetTrigger("hit");
        if (headth <= 0)
        {
            isDead = true;
            animator.SetBool("isDead",isDead);
            GameManager.instance.RemoveEnemies(this);
        }
    }

    /// <summary>
    /// 当打击点接触物体触发事件
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void onHitPointTrgger(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IDamageable>()?.GetHit(1);
        }
    }
}
