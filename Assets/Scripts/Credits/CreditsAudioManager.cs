using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Music References")]
    [SerializeField] FMODUnity.EventReference creditsMusic;
    private FMOD.Studio.EventInstance creditsMusic_instance;

    private string currentlyPlaying;

    // Sets all instance variables
    void Awake() {
        creditsMusic_instance = FMODUnity.RuntimeManager.CreateInstance(creditsMusic);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        creditsMusic_instance.start();
        currentlyPlaying("credits");
    }
}
