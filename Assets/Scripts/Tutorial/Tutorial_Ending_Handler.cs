using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Ending_Handler : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider doorSensor)
    {
        if (doorSensor.gameObject.tag == "Player")
        {
            GameObject.Find("GameManagerObject").GetComponent<Tutorial_Sequence>().EndingMonsterEvent();
            Destroy(gameObject);
        }
    }
}
