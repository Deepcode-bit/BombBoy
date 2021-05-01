using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    private Rigidbody2D rigidbody2d;
    public float speed;
    public float jumpForce;
    private bool canJump;
    private Animator animator;
    private FloatingJoystick joystick;

    [Header("Gound Check")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("States Check")]
    public bool isGround;
    public bool isJump;

    [Header("FX")]
    public GameObject jumpFX;
    public GameObject landFX;

    [Header("Attack setting")]
    public GameObject bomb;
    public int waitTime;
    private bool canAttack = true;

    [Header("State")]
    public float health;
    public bool isDead = false;

    [Header("Camera")]
    public int cameraMin;
    public int cameraMax;

    [Header("Audio")]
    public AudioClip hitClip;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = PlayerPrefs.GetFloat("player_health", 3f);
        UIManager.instance.UpdateHealth(health);
        joystick = FindObjectOfType<FloatingJoystick>();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        if (Time.timeScale == 0) return;
        MoveMent();
        GroundCheck();
        Jump();
    }

    //private void Update() => CheckInput();

    private void Update()
    {
        float playerX = Mathf.Clamp(transform.position.x, cameraMin, cameraMax);
        Camera.main.transform.position =
            Vector3.Lerp(Camera.main.transform.position, 
            new Vector3(playerX, Camera.main.transform.position.y, Camera.main.transform.position.z), 3 * Time.deltaTime);
    }
    /// <summary>
    /// 玩家移动方法
    /// </summary>
    private void MoveMent()
    {
        //float horizontalInput = Input.GetAxisRaw("Horizontal");
        float horizontalInput = joystick.Horizontal;
        rigidbody2d.velocity = new Vector2(horizontalInput * speed, rigidbody2d.velocity.y);
        //人物翻转
        if (horizontalInput == 0) return;
        var rotation = horizontalInput > 0 ? 0 : 180;
        transform.eulerAngles = new Vector3(0, rotation, 0);
    }

    private void CheckInput()
    {
        if (Time.timeScale == 0 || GameManager.instance.gameOver) return;
        if (Input.GetButtonDown("Jump") && isGround) canJump = true;
        if (Input.GetKeyDown(KeyCode.J)) Attack();
    }

    /// <summary>
    /// 玩家跳跃方法
    /// </summary>
    private void Jump()
    {
        if (!canJump) return;
        isJump = true;
        canJump = false;
        jumpFX.SetActive(true);
        jumpFX.transform.position = transform.position + new Vector3(0, -0.67f, 0);
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
    }

    private void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        rigidbody2d.gravityScale = isGround ? 1 : 4;
        if (isGround) isJump = false;
    }

    private void OnDrawGizmos() => Gizmos.DrawWireSphere(groundCheck.position, checkRadius);

    /// <summary>
    /// 动画事件
    /// </summary>
    public void onLand()
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.79f, 0);
    }

    /// <summary>
    /// 玩家攻击方法
    /// </summary>
    private void Attack()
    {
        if (!canAttack) return;
        if (Time.timeScale == 0 || GameManager.instance.gameOver) return;
        Instantiate(bomb, transform.position, bomb.transform.rotation);
        canAttack = false;
        Task.Run(() =>
        {
            Thread.Sleep(waitTime * 1000);
            canAttack = true;
        });
    }

    public void GetHit(float damage)
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("player_hit")) return;
        if (hitClip != null) AudioSource.PlayClipAtPoint(hitClip, transform.position);
        health -= damage;
        animator.SetTrigger("hit");
        UIManager.instance.UpdateHealth(health);
        if (health <= 0)
        {
            gameObject.layer = LayerMask.NameToLayer("NPC");
            GameManager.instance.GameOver();
            isDead = true;
            animator.SetBool("isDead", isDead);
        }
    }

    #region Button Event
    public void AttackClick() => Attack();
    public void JumpClick()
    {
        if (isGround)
            canJump = true;
    }
    public async void Rebirth()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        health = 3;
        isDead = false;
        GameManager.instance.gameOver = false;
        animator.SetBool("isDead", isDead);
        gameObject.layer = LayerMask.NameToLayer("Player");
        UIManager.instance.UpdateHealth(health);
    }
    #endregion
}
