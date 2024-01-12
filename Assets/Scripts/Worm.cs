using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Entyty
{

    private void Start()
    {
        lives = 1;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {

            Hero.Instance.GetDamage();
            lives--;
            Debug.Log("У червя осталось " + lives + "XP");

        }


        if (lives < 1)
            Die();

    }

    


}
