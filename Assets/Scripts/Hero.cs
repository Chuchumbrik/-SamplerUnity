using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entyty
{
    [SerializeField] private float speed = 3f; // скорость движения
    [SerializeField] private int lives = 5; // скорость движения
    [SerializeField] private float jumpForce = 7f; // сила прыжка
    private bool isGrounded = false;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private states state
    {
        get { return (states)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        Instance = this;
    }


    public static Hero Instance { get; set; }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded) state = states.idle;

        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
    }

    private void Run()
    {
        if (isGrounded) state = states.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;

        if (!isGrounded) state = states.jump;
    }

    public override void GetDamage()

    {

        lives -= 1;
        Debug.Log("Здоровье героя " + lives + "XP");

    }
}

public enum states
{
    idle,
    run,
    jump
}