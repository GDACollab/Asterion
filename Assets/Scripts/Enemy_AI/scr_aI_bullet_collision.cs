using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_aI_bullet_collision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ai_GameBoundry") {Destroy(gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
