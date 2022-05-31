using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstramoriMusicManager : MonoBehaviour
{

    public GameObject astramoriCabinet;
    [SerializeField] SanityManager sanityManager;

    [Header("Music References")]
    [SerializeField] FMODUnity.EventReference astramoriIdleMusic;
    [SerializeField] FMODUnity.EventReference astramoriMainMusic;

    private FMOD.Studio.EventInstance astramoriIdleMusic_instance;
    private FMOD.Studio.EventInstance astramoriMainMusic_instance;

    private string currentlyPlaying;

    void Awake()
    {
        astramoriIdleMusic_instance = FMODUnity.RuntimeManager.CreateInstance(astramoriIdleMusic);
        astramoriMainMusic_instance = FMODUnity.RuntimeManager.CreateInstance(astramoriMainMusic);

        astramoriIdleMusic_instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(astramoriCabinet));
        astramoriMainMusic_instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(astramoriCabinet));
        
        // NOTE: The assignment of the FMOD parameter "Sanity" to the Unity variable takes place in SanityManager.
    }

    void Start()
    {
        astramoriIdleMusic_instance.start();      
        currentlyPlaying = "idle";
    }

    
    public void setPaused(bool paused)
    {
        switch (currentlyPlaying)
        {
            case "idle":
                astramoriIdleMusic_instance.setPaused(paused);
                break;

            case "main":
                astramoriMainMusic_instance.setPaused(paused);
                break;

            case "none":
                break;

            default:
                //By default, do nothing.
                print("=====================================================\nIMPROPER USE OF setPaused() in AstramoriMusicManager!\n=====================================================");
                break;
        }
    }


    public void PlayMusic(string music)
    {
        switch (music.ToLower())
        {
            case "idle":
                if (currentlyPlaying != "idle")
                {
                    astramoriMainMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                    astramoriIdleMusic_instance.start();
                    currentlyPlaying = "idle";
                }
                break;


            case "main":
                if (currentlyPlaying != "main")
                {
                    astramoriIdleMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                    astramoriMainMusic_instance.start();
                    currentlyPlaying = "main";
                }
                break;


            case "stop all":
                astramoriIdleMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                astramoriMainMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                currentlyPlaying = "none";
                break;


            default:
                //By default, play the idle music.
                if (currentlyPlaying != "idle")
                {
                    astramoriMainMusic_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                    astramoriIdleMusic_instance.start();
                    currentlyPlaying = "idle";
                }
                break;
        }
    }
}
