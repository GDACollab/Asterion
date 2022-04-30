using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Door_Handler : MonoBehaviour
{
    void OnTriggerEnter(Collider doorSensor)
    {
        if (doorSensor.gameObject.tag == "Player")
        {
            GameObject.Find("EmergencyLight (1)").GetComponent<Light>().intensity = 2.8f;
            this.transform.parent.gameObject.GetComponent<Door>().locked = true;
            Destroy(gameObject);
        }
    }
}
