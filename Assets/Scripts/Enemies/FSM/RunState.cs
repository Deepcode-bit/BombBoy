using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemies.FSM
{
    public class RunState : EnemyState
    {
        private bool isRun = true;

        public void EnterState(BaseEnemy enemy)
        {
            if (enemy.skillClip != null) AudioSource.PlayClipAtPoint(enemy.skillClip, enemy.transform.position);
            Task.Factory.StartNew(async() =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                isRun = false;
            });
        }

        public void OnUpdate(BaseEnemy enemy)
        {
            if (!isRun || enemy.targetPoint==null)
            {
                enemy.TransToState(new PatrolState());
                enemy.animator.SetBool("isRun", false);
                return;
            }
            if (!enemy.targetPoint.CompareTag("Bomb"))
            {
                enemy.TransToState(new PatrolState());
                enemy.animator.SetBool("isRun", false);
                return;
            }
            FlipDir(enemy);//翻转
            var position = enemy.transform.position;
            var dir = position.x > enemy.targetPoint.position.x ? Vector3.right : Vector3.left;
            position = Vector2.MoveTowards(position, position + dir, enemy.speed * Time.deltaTime);
            enemy.transform.position = position;
        }

        void FlipDir(BaseEnemy enemy)
        {
            var rotation = enemy.transform.position.x < enemy.targetPoint.position.x ? 180 : 0;
            enemy.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }
}
