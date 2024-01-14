using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Entity
{
    [SerializeField] private float speed = 3f; // скорость движения
    [SerializeField] private int health; // Кол-во жизней
    [SerializeField] private float jumpForce = 15f; // сила прыжка
    private bool isGrounded = false;

    [SerializeField] private Image[] hearth;

    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;

    public bool isAttacking = false;
    public bool isRecharged = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;
    

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    public static Hero Instance { get; set; }

    private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        lives = 5;
        health = lives;
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        isRecharged = true;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded && !isAttacking) State = States.idle;

        if (!isAttacking && Input.GetButton("Horizontal"))
            Run();
        if (!isAttacking && isGrounded && Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButtonDown("Fire1"))
            Attack();
        if (health > lives)
            health = lives;

        for (int i = 0; i < hearth.Length; i++) 
        {
            if (i < health)
                hearth[i].sprite = aliveHeart;
            else
                hearth[i].sprite = deadHeart;


           /* if (i < health)
                hearth[i].enabled = true;
            else
                hearth[i].enabled = false;*/
        }
    }


    private void Attack() 
    {
        if (isGrounded && isRecharged) 
        {
            State = States.attack;
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    private IEnumerator EnemyOnAttack(Collider2D enemy)
    {
        SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();
        enemyColor.color = new Color(1f, 0.0859375f, 0.10546875f);
        yield return new WaitForSeconds(0.2f);
        enemyColor.color = new Color(1, 1, 1);
    }



    private void OnAttack() 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < colliders.Length; i++) 
        {
            colliders[i].GetComponent<Entity>().GetDamage();
            StartCoroutine(EnemyOnAttack(colliders[i]));

        }

    }


    private void Run()
    {
        if (isGrounded) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected() // Рисует сферу атаки
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.35f);
        isGrounded = collider.Length > 1;

        if (!isGrounded) State = States.jump;
    }



    public override void GetDamage()
    {
        lives -= 1;
        Debug.Log(lives);
        if (health <= 1) 
        {
            Die();
            foreach (var h in hearth)
                h.sprite = deadHeart;
        }
    }
}

public enum States
{
    idle,
    run,
    jump,
    attack

}