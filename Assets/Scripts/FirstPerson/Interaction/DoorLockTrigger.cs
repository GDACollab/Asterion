using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A relatively simple class in charge of a trigger which, when triggered, will shut and lock the door it's attatched to
public class DoorLockTrigger : MonoBehaviour
{
    // The Door to be shut and locked once
    public Door door;
    private bool TriggeredTutorial = false;

    // Once the player enters the trigger, the door will close and lock, and the trigger will deactivate
    void OnTriggerEnter(Collider detection) {
        if(detection.gameObject.tag == "Player") {
            door.closeDoor();
            door.locked = true;
            gameObject.SetActive(false);

            if (TriggeredTutorial == false)
            {
                GameObject.Find("EmergencyLight (1)").GetComponent<Light>().intensity = 2.8f;
                TriggeredTutorial = true;
            }
        }
    }
}
