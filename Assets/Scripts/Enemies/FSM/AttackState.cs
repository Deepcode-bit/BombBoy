using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class AttackState : EnemyState
    {
        BaseEnemy enemy;
        public void EnterState(BaseEnemy enemy)
        {
            this.enemy = enemy;
            enemy.targetPoint = enemy.targets[0];
            enemy.animator.SetInteger("state", 2);
        }

        public void OnUpdate(BaseEnemy enemy)
        {
            if (enemy.targets.Count == 0)
            {
                enemy.TransToState(new PatrolState());
                return;
            }
            if (enemy.targets.Count == 1) enemy.targetPoint = enemy.targets[0];
            if (enemy.targets.Count > 1) getTarget();
            if (enemy.targetPoint.CompareTag("Player")) enemy.AttackAction();
            if (enemy.targetPoint.CompareTag("Bomb")) enemy.SkillAction();
            enemy.MoveToTarget();
        }

        private void getTarget()
        {
            foreach (var target in enemy.targets)
            {
                if (Math.Abs(enemy.transform.position.x - target.position.x) <
                    Math.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))
                {
                    enemy.targetPoint = target;
                }
            }
        }
    }
}
