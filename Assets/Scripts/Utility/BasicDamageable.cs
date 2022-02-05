using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDamageable : MonoBehaviour
{
    public int health;
    public int baseHealth;
    public bool isAlien;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    public virtual void Start()
    {
        health = baseHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Death();
        }
    }

    public virtual void Death() { 
        
    
    }

    public virtual void Disable()
    {

    }


    


}
