using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementZone : MonoBehaviour
{

    [SerializeField] GameObject player;
    public bool isContact;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.transform.position;
        isContact = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "AstramoriPlacementZone")
        {
            isContact = false;
        }
    }

    
}
