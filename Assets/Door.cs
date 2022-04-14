using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Names of the animation triggers used to open / close the door
    public string openName = "OpenTrigger";
    public string closeName = "CloseTrigger";

    // Animator that handles the door's animations
    Animator doorAnimator;

    // Whether or not the door is open
    bool doorOpen;

    void Start() {
        doorOpen = false;
        doorAnimator = GetComponent<Animator>();
        Debug.Log("Start" + doorAnimator);
    }

    // Trigger handling stuff
    void OnTriggerEnter(Collider doorSensor) {
        openDoor();
    }

    void OnTriggerExit(Collider doorSensor) {
        closeDoor();
    }

    // Animates the door movement
    public void openDoor() {
        if(!doorOpen) {
            doorOpen = true;
            doorAnimator.SetTrigger(openName);
        }
    }

    public void closeDoor() {
        if(doorOpen) {
            doorOpen = false;
            doorAnimator.SetTrigger(closeName);
        }
    }
}
