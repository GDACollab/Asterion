using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDamageable : MonoBehaviour
{
    public int health;
    public int baseHealth;
    public bool isAlien;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlien)
        {
            if (collision.transform.tag == "StarshipBullet")
            {

            }
        }
        else
        {
            if (collision.transform.tag == "AlienBullet")
            {

            }
        }
    }


}
