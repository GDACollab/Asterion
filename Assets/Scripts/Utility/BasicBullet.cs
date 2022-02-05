using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    public int damage;
    public bool isAlien;
    public float lifetime;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Destroy(this.gameObject, lifetime);
    }



    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAlien)
        {
            if(collision.transform.tag == "Player")
            {
                collision.transform.GetComponent<BasicDamageable>().TakeDamage(damage);
                Destroy(this.gameObject);
            }

            
        }
        else
        {
            if (collision.transform.tag == "AlienShip")
            {
                collision.transform.GetComponent<BasicDamageable>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }


}
