using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementZone : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] private Collider2D boxcollider;
    [SerializeField] private Transform cursor;
    public bool isContact;
    public bool shouldntMove;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckBoundsRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (!shouldntMove)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
            
        }

        
        
        

        

    }


    public IEnumerator CheckBoundsRoutine()
    {
        while (true)
        {
            if (boxcollider.bounds.Contains(cursor.transform.position))
            {
                isContact = true;
            }
            else
            {
                isContact = false;
            }

            yield return new WaitForSeconds(0.05f);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AstramoriPlacementZone")
        {
            //isContact = true;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "AstramoriPlacementZone")
        {
            //isContact = false;
        }
    }

    
}
