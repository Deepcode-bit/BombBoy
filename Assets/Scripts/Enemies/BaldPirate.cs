using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class BaldPirate:BaseEnemy
    {
        protected override void onHitPointTrgger(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<IDamageable>()?.GetHit(1);
            }
            if (collision.CompareTag("Bomb"))
            {
                var dir = transform.position.x > collision.transform.position.x ? -1 : 1;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * 10, ForceMode2D.Impulse);
            }
        }
    }
}
