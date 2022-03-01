using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeCursor : MonoBehaviour
{
    public List<Button> collidingObjects;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CanvasButton" && collision.gameObject.activeInHierarchy)
        {
            
            
                collidingObjects.Add(collision.transform.GetComponent<Button>());
            
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CanvasButton")
        {
            
            collidingObjects.Remove(collision.transform.GetComponent<Button>());
        }
    }

    private void LateUpdate()
    {
        
    }


}
