using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship1 : MonoBehaviour
{
    Rigidbody2D rb;
    Transform starfighter;

    [SerializeField] float speed;

    bool active;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        // If active, move ship towards Starfighter
        rb.velocity = active ? (Vector2)(starfighter.position - transform.position).normalized * speed : Vector2.zero;
    }

    public void Activate(bool active)
    {
        this.active = active;
    }

    public void SetStarfighter(Transform starfighter)
    {
        this.starfighter = starfighter;
    }
}
