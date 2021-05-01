using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator animator;
    private Collider2D collider2d;
    private Rigidbody2D rigidbody2d;
    private bool isOff = false;

    public float startTime;
    public float waitTime;
    public float bombForce;
    [Header("check")]
    public float radius;
    public LayerMask targetLayer;
    [Header("Audio")]
    public AudioClip boomClip;
    public AudioClip lightClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        startTime = Time.time;       
    }

    void Update()
    {
        if (Time.time > startTime + waitTime && !isOff)
        {
            animator.Play("Explotion");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Explotion()
    {
        audioSource.Pause();
        AudioSource.PlayClipAtPoint(boomClip, transform.position);
        collider2d.enabled = false;
        rigidbody2d.gravityScale = 0;
        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        foreach (var item in aroundObjects)
        {
            Vector3 pos = transform.position - item.transform.position;
            item.GetComponent<Rigidbody2D>().AddForce((-pos+Vector3.up) * bombForce, ForceMode2D.Impulse);
            item.GetComponent<IDamageable>()?.GetHit(3);
            if (item.CompareTag("Bomb") && item.GetComponent<Bomb>().isOff)
            {    
                item.GetComponent<Bomb>().TurnOn();
            }
        }
    }

    public void onExplotionEnd()
    {
        Destroy(transform.gameObject);
    }

    public void TurnOff()
    {
        audioSource.Pause();
        animator.Play("BombOff");
        isOff = true;
        gameObject.layer = LayerMask.NameToLayer("NPC");
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    public void TurnOn()
    {
        audioSource.Play();
        if (gameObject.activeSelf) animator.Play("BombOn");
        isOff = false;
        gameObject.layer = LayerMask.NameToLayer("Bomb");
        GetComponent<SpriteRenderer>().sortingOrder = 10;
        startTime = Time.time;
    }
}
