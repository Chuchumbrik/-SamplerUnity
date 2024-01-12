using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entyty : MonoBehaviour
{

    protected int lives;

    public virtual void GetDamage()
    {
        Debug.Log(name + " " +lives);
        lives--;
        if (lives < 1)
            Die();
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }


}
