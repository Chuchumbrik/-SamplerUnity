using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entyty
{
    [SerializeField] private float speed = 3f; // скорость движения
    [SerializeField] private int lives1 = 5; // жизни
    [SerializeField] private float jumpForce = 7f; // сила прыжка
    private bool isGrounded = false;

    public bool isAttacking = false; // Атакуем лм сейчас
    public bool isRecharged = true;  // Перезарядились ли

    public Transform attackPos; // Позиция атаки
    public float attackRange;  // Дальность атаки
    public LayerMask enemy;  //Слой с врагами


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
        isRecharged = true; // На начало игры перезаряжено
    }


    public static Hero Instance { get; set; }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded) state = states.skin2_idle;

        if (!isAttacking && Input.GetButton("Horizontal"))
            Run();
        if (!isAttacking && isGrounded && Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButtonDown("Fire1")) //Считывание нажатия кнопки для атаки
            Attack();
    }

    private void Run()
    {
        if (isGrounded) state = states.skin2_run;

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }




    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;

        if (!isGrounded) state = states.skin2_jump;
    }

    public override void GetDamage()

    {

        lives1 -= 1;
        Debug.Log("Здоровье героя " + lives1 + "XP");

    }

    private IEnumerator AttackAnimation() //Подсчёт времени атаки
    {
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown() //Подсчёт времени перезарядки
    {
        yield return new WaitForSeconds(0.6f);  
        isRecharged = true;
    }

    private void Attack()
    {
        if (isGrounded && isRecharged)
        {

            state = states.skin2_attack;
            isAttacking = true;
            isRecharged = false;


            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());

        }

    }

    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position,attackRange, enemy);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entyty>().GetDamage();
        }


    }







}


public enum states
{
    //idle,
    //run,
    //jump
    skin2_idle,
    skin2_run,
    skin2_jump,
    skin2_attack  // состояние атаки
}