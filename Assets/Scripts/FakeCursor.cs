using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeCursor : MonoBehaviour
{
    public List<Button> collidingObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CanvasButton")
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



}
