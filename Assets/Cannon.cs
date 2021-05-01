using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

public class Cannon : MonoBehaviour,IDamageable
{
    private bool canAttack = true;
    private bool isDead = false;
    private Animator animator;
    public int dir;

    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    
    void Update()
    {
        if (isDead) return;
        Attack();
    }

    void Attack()
    {
        if (!canAttack) return;
        animator.SetTrigger("attack");
        Task.Factory.StartNew(async() => 
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            canAttack = true;
        });
        canAttack = false;
    }

    #region Animation Event
    public void PopBomb()
    {
        var bomb = Instantiate(Resources.Load("Bomb") as GameObject, transform.parent);
        bomb.transform.position = transform.position;
        bomb.transform.position += new Vector3(dir, 1, 0);
        var force = new Random().Next(5,20);
        bomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 0) * force, ForceMode2D.Impulse);
    }
    #endregion

    public async void GetHit(float damage)
    {
        isDead = true;
        await Task.Delay(TimeSpan.FromSeconds(3));
        Destroy(transform.gameObject);
    }
    

}
