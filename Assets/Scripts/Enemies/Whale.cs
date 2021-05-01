using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Enemies
{
    class Whale:BaseEnemy
    {
        public float scale = 1.2f;
        private List<Transform> bombs = new List<Transform>();

        #region Animation Event
        public async void Swalow()
        {
            if (!targetPoint.CompareTag("Bomb")) return;
            targetPoint.GetComponent<Bomb>()?.TurnOff();
            targetPoint.gameObject.SetActive(false);
            transform.localScale *= scale;
            bombs.Add(targetPoint);
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            if (bombs.Count >= 4)
            {
                isDead = true;
                animator.SetBool("isDead", isDead);
                GameManager.instance.RemoveEnemies(this);
                return;
            }
        }
        public async void Explosion()
        {
            Random random = new Random();
            foreach(var bomb in bombs)
            {
                while (Time.timeScale == 0) await Task.Yield(); 
                var dir = random.Next(2) == 0 ? 1 : -1;
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                bomb.GetComponent<Bomb>()?.TurnOn();
                bomb.position = transform.position;
                bomb.gameObject.SetActive(true);
                bomb.gameObject.GetComponent<Rigidbody2D>()?.AddForce(new Vector2(dir, 1) * 10, ForceMode2D.Impulse);
                transform.localScale *= (2 - scale);
            }
        }
        #endregion
    }
}
