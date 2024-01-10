using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entyty : MonoBehaviour
{

    public int damage;
    public new string name;
    public int lives;


    public virtual void GetDamage(int recivedDamage)
    {

        lives -= recivedDamage;
        Debug.Log("Здоровье " + name +" - "+ lives + " XP");

    }

    public void Die()
    {

        Destroy(this.gameObject);
    }
     






}
