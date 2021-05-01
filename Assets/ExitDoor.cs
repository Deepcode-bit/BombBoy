using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private Animator animator;
    private Collider2D doorCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        doorCollider = GetComponent<Collider2D>();
        doorCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.NextLevle();
        }
    }

    public void Open()
    {
        animator.Play("Open");
        doorCollider.enabled = true;
    }
}
