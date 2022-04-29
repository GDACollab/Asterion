using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Names of the animation triggers used to open / close the door
    public string openName = "OpenTrigger";
    public string closeName = "CloseTrigger";
    public bool isAsterion;

    // Animator that handles the door's animations
    Animator doorAnimator;

    // Whether or not the door is open
    public bool doorOpen = false;

    // Whether or not the door is locked shut
    public bool locked;

    // Door SFX player
    [HideInInspector] public GameObject doorObject;

    [Header("SFX References")]
    [SerializeField] FMODUnity.EventReference doorOpenSFX;
    [SerializeField] FMODUnity.EventReference doorCloseSFX;

    void Start() {
        doorOpen = false;
        doorAnimator = GetComponent<Animator>();

        doorObject = GetComponentInChildren<Rigidbody>().gameObject;
    }

    // Trigger handling stuff
    void OnTriggerEnter(Collider doorSensor) {
        if(doorSensor.gameObject.tag == "Player") {
            openDoor();
        }
    }

    void OnTriggerExit(Collider doorSensor) {
        closeDoor();
    }

    // Animates the door movement
    public void openDoor() {
        if(!doorOpen && !locked) {
            doorOpen = true;
            FMODUnity.RuntimeManager.PlayOneShotAttached(doorOpenSFX.Guid, doorObject);
            doorAnimator.SetTrigger(openName);

            if(GameManager.Instance.asterionGamesPlayed == 1 && !isAsterion)
            {
                Tutorial_Sequence.Instance.TonyBehindAstramori();
            }
        }
    }

    public void closeDoor() {
        if(doorOpen) {
            doorOpen = false;
            FMODUnity.RuntimeManager.PlayOneShotAttached(doorCloseSFX.Guid, doorObject);
            doorAnimator.SetTrigger(closeName);
        }
    }
}
