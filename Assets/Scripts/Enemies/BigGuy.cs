using Assets.Scripts.Enemies.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class BigGuy:BaseEnemy
    {
        public Transform pickPoint;
        private bool hasBomb = false;
        public float power;

        #region Animation Event
        public void PickUpBomb()
        {
            if (!targetPoint.CompareTag("Bomb") || hasBomb) return;
            targetPoint.gameObject.transform.position = pickPoint.position;
            targetPoint.SetParent(pickPoint);
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            hasBomb = true;
            TransToState(new HasBombState());
        }

        public void ThrowBomb()
        {
            if (hasBomb)
            {
                targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                targetPoint.SetParent(transform.parent.parent);
                var player = FindObjectOfType<PlayerController>()?.gameObject;
                var force = player.transform.position.x - transform.position.x < 0 ? -1 : 1;
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 1) * power, ForceMode2D.Impulse);
            }
            hasBomb = false;
            TransToState(new PatrolState());
        }
        #endregion
    }
}
