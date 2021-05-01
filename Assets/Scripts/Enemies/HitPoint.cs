using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public delegate void OnHitPointTrigger(Collider2D collision);
    public event OnHitPointTrigger onHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onHit?.Invoke(collision);
    }
}
