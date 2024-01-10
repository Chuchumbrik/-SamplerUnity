using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngryWood : Entyty
{

    private EngryWood()
    {
        lives = 99;
        damage = 1;
        name = "Wood";
    }






    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {

            Debug.Log(lives + " " + damage + " " + name);
            Hero.Instance.GetDamage(damage);
            

        }
    }




}
