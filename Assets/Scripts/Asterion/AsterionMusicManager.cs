using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsterionMusicManager : MonoBehaviour
{

    public GameObject asterionCabinet;
    [SerializeField] SanityManager sanityManager;

    [Header("Music References")]
    [SerializeField] FMODUnity.EventReference asterionIdleMusic;
    private FMOD.Studio.EventInstance asterionIdleMusic_instance;
    [SerializeField] FMODUnity.EventReference asterionMenuMusic;
    private FMOD.Studio.EventInstance asterionMenuMusic_instance;
    [SerializeField] FMODUnity.EventReference asterionMainMusic;
    private FMOD.Studio.EventInstance asterionMainMusic_instance;

    private string currentlyPlaying;

    void Awake()
    {
        asterionIdleMusic_instance = FMODUnity.RuntimeManager.CreateInstance(asterionIdleMusic);
        asterionMenuMusic_instance = FMODUnity.RuntimeManager.CreateInstance(asterionMenuMusic);
        asterionMainMusic_instance = FMODUnity.RuntimeManager.CreateInstance(asterionMainMusic);

        asterionIdleMusic_instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(asterionCabinet));
        asterionMenuMusic_instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(asterionCabinet));
        asterionMainMusic_instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(asterionCabinet));

        // NOTE: The assignment of the FMOD parameter "Sanity" to the Unity variable takes place in SanityManager.
    }

    void Start()
    {
        asterionIdleMusic_instance.start();      
        currentlyPlaying = "idle";
    }

    public void PlayMusic(string music)
    {
        switch (music.ToLower())
        {
            case "idle":
                if (currentlyPlaying != "idle")
                {
                    asterionMenuMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    asterionMainMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                    asterionIdleMusic_instance.start();
                    currentlyPlaying = "idle";
                }
                break;

            case "menu":
                if (currentlyPlaying != "menu")
                {
                    asterionIdleMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    asterionMainMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                    asterionMenuMusic_instance.start();
                    currentlyPlaying = "menu";
                }
                break;

            case "main":
                if (currentlyPlaying != "main")
                {
                    asterionIdleMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    asterionMenuMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                    asterionMainMusic_instance.start();
                    currentlyPlaying = "main";
                }
                break;

            case "stop all":
                asterionIdleMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                asterionMenuMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                asterionMainMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                currentlyPlaying = "none";
                break;

            default:
                //By default, play the idle music.
                if (currentlyPlaying != "idle")
                {
                    asterionMenuMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    asterionMainMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                    asterionIdleMusic_instance.start();
                    currentlyPlaying = "idle";
                }
                break;
        }
    }


}
