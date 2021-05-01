using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private PlayerController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        animator.SetFloat("speed", Mathf.Abs(rigidbody2d.velocity.x));
        animator.SetFloat("velocityY", rigidbody2d.velocity.y);
        animator.SetBool("jump", controller.isJump);
        animator.SetBool("ground", controller.isGround);
    }

}
