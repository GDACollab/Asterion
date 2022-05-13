using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstramoriMusicManager : MonoBehaviour
{
    public GameObject astramoriCabinet;
    [SerializeField] SanityManager sanityManager;

    [Header("Music References")]
    [SerializeField] FMODUnity.EventReference astramoriMusic;
    private FMOD.Studio.EventInstance astramoriMusic_instance;

    void Awake()
    {
        astramoriMusic_instance = FMODUnity.RuntimeManager.CreateInstance(astramoriMusic);
    }

    // Start is called before the first frame update
    void Start()
    {
        astramoriMusic_instance.start();
    }

    // Update is called once per frame
    void Update()
    {
        astramoriMusic_instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(astramoriCabinet));
        
        // NOTE: The assignment of the FMOD parameter "Sanity" to the Unity variable takes place in SanityManager.
    }
}
