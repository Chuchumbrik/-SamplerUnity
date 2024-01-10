using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Entyty
{

    //[SerializeField] public int lives = 4;
    //[SerializeField] public int damage = 2;
    //[SerializeField] public new string name = "Worm";


    private Worm()
    {
        lives = 5;
        damage = 1;
        name = "Worm";
    }


    private void OnCollisionEnter2D(Collision2D collision) // Метод, который вызывается при столкновении с другим коллайдером
    {
        if (collision.gameObject == Hero.Instance.gameObject) // Если объект столкновения является героем
        {
            Debug.Log(lives + " " + damage + " " + name);
            Hero.Instance.GetDamage(damage);
            GetDamage(Hero.Instance.damage);

        }


        if (lives < 1)
            Die();
        
    }

    


}
