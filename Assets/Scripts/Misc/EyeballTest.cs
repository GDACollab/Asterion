using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballTest : MonoBehaviour
{

    public Renderer planeMat;
    public GameObject player;
    public Vector3 normalizedOffset;
    public int axis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        normalizedOffset = (player.transform.position - transform.position).normalized;

        
        if(axis == 0)
        {
            planeMat.material.SetVector("_vectorOffset", new Vector3(normalizedOffset.x, 0, 0));
        }
        else if (axis == 1)
        {
            planeMat.material.SetVector("_vectorOffset", new Vector3(-normalizedOffset.z, 0, 0));
        }
        
    }
}
