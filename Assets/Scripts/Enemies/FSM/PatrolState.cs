using Assets.Scripts.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyState
{
    public void EnterState(BaseEnemy enemy)
    {
        enemy.targetPoint = enemy.pointB;
    }

    public void OnUpdate(BaseEnemy enemy)
    {
        if (enemy.targets.Count != 0)
        {
            enemy.TransToState(new AttackState());
            return;
        }

        if (!enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            enemy.animator.SetInteger("state", 1);
            enemy.MoveToTarget();
        }
        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f)
        {
            enemy.animator.SetInteger("state", 0);
            enemy.targetPoint = enemy.targetPoint == enemy.pointA ? enemy.pointB : enemy.pointA;
        }
    }

}
