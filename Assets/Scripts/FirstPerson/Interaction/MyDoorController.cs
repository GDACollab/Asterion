using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDoorController : MonoBehaviour
{
    public Animator doorAnim;

    private bool doorOpen = false;

    public void Awake()
    {
        doorAnim = gameObject.transform.parent.GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        if (!doorOpen)
        {
            doorAnim.Play("Open_Door", 0, 0.0f);
            doorOpen = true;
        }
        else
        {
            doorAnim.Play("Close_Door", 0, 0.0f);
            doorOpen = false;
        }    
    }    
}
