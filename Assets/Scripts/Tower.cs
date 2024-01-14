using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Entity
{
    private void Start()
    {
        lives = 5;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            lives--;
            Debug.Log("Tower XP = " + lives);
        }


        if (lives < 1)
            Die();
    }
}
