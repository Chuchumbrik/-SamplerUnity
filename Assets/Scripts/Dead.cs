using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : Entity
{
    //private float speed = 0.5f;
    private Vector3 direction;
    private SpriteRenderer sprite;

    private void Awake() // Метод, который вызывается при создании объекта
    {
        sprite = GetComponentInChildren<SpriteRenderer>(); // Присваивает переменной sprite компонент SpriteRenderer, который находится в дочернем объекте
    }

    private void Start() // Метод, который вызывается при запуске сцены
    {
        direction = transform.right; // Присваивает переменной direction значение вектора, указывающего вправо относительно объекта
        lives = 5;
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 1f + transform.right * direction.x * 0.7f, 0.1f);

        if (colliders.Length > 0) direction *= -1f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime);
        sprite.flipX = direction.x < 0.0f;
    }

    private void Update() // Метод, который вызывается каждый кадр
    {
        Move(); // Вызывает метод Move
    }

    private void OnCollisionEnter2D(Collision2D collision) // Метод, который вызывается при столкновении с другим коллайдером
    {
        if (collision.gameObject == Hero.Instance.gameObject) // Если объект столкновения является героем
        {
            Hero.Instance.GetDamage(); // Вызывает метод GetDamage у героя
        }
    }
}