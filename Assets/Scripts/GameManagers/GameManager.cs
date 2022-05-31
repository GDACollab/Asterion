using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using AsterionArcade;
using FirstPersonPlayer;
using Interactable;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> alienShipPrefabs;

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Volume postProcessingVolume;
    [SerializeField] Vignette vignette;
    [SerializeField] private List<GameObject> FPDisplay;
    [SerializeField] private Animator tempLoseAnim;
    [SerializeField] private GameObject fpUI;
    [SerializeField] private GameObject hallwayTony3D;
    public IntroUI introUI;
    public Animator playerScareAnim;
    public GameObject pauseUI;
    public GameObject settingsUI;
    public Transform astramoriEnemyBullets;
    public Transform asterionEnemyBullets;
    public AsterionManager asterionManager;
    public AstramoriManager astramoriManager;
    public SanityManager sanityManager;
    public PowerManager powerManager;
    public FirstPersonPlayer.PlayerMovement playerMovement;
    [SerializeField] PlayerLook playerLook;
    [SerializeField] Animator finalScareAnim;
    [SerializeField] public GameObject finalScareCore;

    [Header("Game State")]
    public ShipStats shipStats;
    public bool canPause;
    public bool isPaused;
    public bool isSettings;
    public bool isPlayingArcade;
    public bool gameLost;
    public int coinCount;
    public float gameTime;
    public int asterionGamesPlayed;
    public int astramoriGamesPlayed;

    private GameObject[] lights;
    private GameObject[] arcadeMachines;
    private GameObject gameDoorsAsterion;
    private GameObject gameDoorsAstramori;
    private GameObject SpookyPlane;
    private GameObject[] interactables;
    private GameObject uiFadeImage;


    [Header("SFX Events & Music")]
    [SerializeField] FMODUnity.EventReference jumpscareSFX;
    private FMOD.Studio.EventInstance jumpscareSFX_instance;
    [SerializeField] FMODUnity.EventReference preJumpscareSFX;
    private FMOD.Studio.EventInstance preJumpscareSFX_instance;
    [SerializeField] AsterionMusicManager asterionMusicManager;
    [SerializeField] AstramoriMusicManager astramoriMusicManager;
    private SpookySFXManager spookySFXManager;


    //acts as a singleton which can be easily referenced with GameManager.Instance

    void Awake()
    {
        Application.targetFrameRate = 61;

        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        lights = GameObject.FindGameObjectsWithTag("Lights");
        arcadeMachines = GameObject.FindGameObjectsWithTag("voidArcadeMachines");
        gameDoorsAstramori = GameObject.Find("AstramoriDoor");
        gameDoorsAsterion = GameObject.Find("AsterionDoor");
        SpookyPlane = GameObject.Find("SpookyPlane");
        interactables = GameObject.FindGameObjectsWithTag("interactables");
        uiFadeImage = GameObject.Find("endingFadeImage");
    }

    // Start is called before the first frame update
    void Start()
    {
        coinText.text = "" + coinCount;
        gameTime = 0;
        
        // SFX stuff
        jumpscareSFX_instance = FMODUnity.RuntimeManager.CreateInstance(jumpscareSFX);
        preJumpscareSFX_instance = FMODUnity.RuntimeManager.CreateInstance(preJumpscareSFX);
        spookySFXManager = GetComponent<SpookySFXManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (canPause && !isPlayingArcade && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }



        gameTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        
        
        if(asterionGamesPlayed >= 1)
        {
            UpdateTimeDisplay();
        }
        


    }

    public void CheckPlayerIsPlayingArcadeStatus()
    {
        if (!isPlayingArcade)
        {
            foreach (GameObject g in FPDisplay)
                g.SetActive(true);
        }
        else
        {
            foreach (GameObject g in FPDisplay)
                g.SetActive(false);
        }
    }

    public IEnumerator NewLoseRoutine()
    {
        //preJumpscareSFX_instance.start();
        yield return new WaitForSeconds(8);
        jumpscareSFX_instance.start();

        gameLost = true;
        tempLoseAnim.Play("tempLoseAnim");
        Time.timeScale = 0;
        FMODUnity.StudioEventEmitter[] sounds = FindObjectsOfType<FMODUnity.StudioEventEmitter>();
        foreach (FMODUnity.StudioEventEmitter a in sounds)
        {
            a.EventInstance.setPaused(true);
        }
        Cursor.lockState = CursorLockMode.Confined;
        yield return new WaitForSeconds(1);
    }


    private void disableLights(bool state)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(state);
        }
        
        for (int i = 0; i < arcadeMachines.Length; i++)
        {
            if (state) { arcadeMachines[i].transform.GetComponentInChildren<Renderer>().material.EnableKeyword("_EMISSION"); }
            else
            {
                arcadeMachines[i].transform.GetComponentInChildren<Renderer>().material.DisableKeyword("_EMISSION");
            }
        }
    }

    public IEnumerator LoseRoutine()
    {
        gameLost = true;
        canPause = false;
        

        // Flicker Lights and floating cabinets
        yield return new WaitForSeconds(0.1f);
        disableLights(false);
        yield return new WaitForSeconds(0.1f);
        disableLights(true);
        yield return new WaitForSeconds(0.1f);
        disableLights(false);

        // Pause Sounds
        FMODUnity.StudioEventEmitter[] sounds = FindObjectsOfType<FMODUnity.StudioEventEmitter>();
        foreach (FMODUnity.StudioEventEmitter a in sounds)
        {
            a.EventInstance.setPaused(true);
        }

        spookySFXManager.Mute();

        // Start pre-jumpscare
        preJumpscareSFX_instance.start();

        fpUI.SetActive(false);
        
        


        // Hide Tony
        SpookyPlane.GetComponent<MeshRenderer>().enabled = false;

        // Lock Doors
        gameDoorsAstramori.GetComponent<Door>().locked = true;
        gameDoorsAstramori.GetComponent<Door>().closeDoor();

        gameDoorsAsterion.GetComponent<Door>().locked = true;
        gameDoorsAsterion.GetComponent<Door>().closeDoor();


        // Boot Player from Cabinets and turn off games
        for(int i = 0; i < interactables.Length; i++)
        {
            interactables[i].SetActive(false);
            interactables[i].GetComponent<InteractableManager>().gameEnding = true;
        }

        asterionManager.ExitMachine();
        astramoriManager.ExitMachine();

        // Stop music!
        asterionMusicManager.PlayMusic("stop all");
        astramoriMusicManager.PlayMusic("stop all");
        hallwayTony3D.SetActive(false);

        // Dramatic Pause
        yield return new WaitForSeconds(4);

        // --  Play Tappings  --
        Debug.Log("TAPTAPTAPTAPTAP");

        yield return new WaitForSeconds(4);

        // Lock Player
        Cursor.lockState = CursorLockMode.Confined;

        // JUMP SCARE ANIMATION!
        playerMovement.SetMovementEnabled(false);
        playerMovement.SetTurningEnabled(false);

        jumpscareSFX_instance.start();
        tempLoseAnim.Play("tempLoseAnim");
        finalScareCore.SetActive(true);
        finalScareAnim.Play("jumpscare");

        yield return new WaitForSeconds(3);


    }


    public void UpdateTimeDisplay()
    {

        int minutes = (int)(gameTime / 60);
        int seconds = (int)(gameTime % 60);

        string time = "";

        if (minutes < 10) time += 0;
        time += minutes + ":";

        if (seconds < 10) time += 0;
        time += seconds;

        timeText.text = time;
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            if (isSettings)
            {
                settingsUI.SetActive(false);
                isSettings = false;
            }
            else
            {
                isPaused = false;
                pauseUI.SetActive(false);
                playerMovement._movementEnabled = true;
                playerLook._rotateEnabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                // Time resume
                Time.timeScale = 1;

                FMODUnity.StudioEventEmitter[] sounds = FindObjectsOfType<FMODUnity.StudioEventEmitter>();
                foreach (FMODUnity.StudioEventEmitter a in sounds)
                {
                    a.EventInstance.setPaused(false);
                }
                spookySFXManager.setPaused(false);
                asterionMusicManager.setPaused(false);
                astramoriMusicManager.setPaused(false);
            }
        }

            
        else
        {
            isPaused = true;
            pauseUI.SetActive(true);
            playerMovement._movementEnabled = false;
            playerLook._rotateEnabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            // Added time stop so game is 100% paused while pause menu active
            Time.timeScale = 0;
            FMODUnity.StudioEventEmitter[] sounds = FindObjectsOfType<FMODUnity.StudioEventEmitter>();
            foreach(FMODUnity.StudioEventEmitter a in sounds)
            {
                a.EventInstance.setPaused(true);
            }
            spookySFXManager.setPaused(true);
            asterionMusicManager.setPaused(true);
            astramoriMusicManager.setPaused(true);

        }
    }


    public void ReloadLevel()
    {
        Time.timeScale = 1;
        StopAllCoroutines();
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        isSettings = true;
        settingsUI.SetActive(true);

    }

    public void CloseSettings()
    {
        isSettings = false;
        settingsUI.SetActive(false);
    }

    public void AlterCoins(int diff)
    {
        coinCount += diff;
        if (coinCount < 0)
        {
            coinCount = 0;
        }

        coinText.text = "" + coinCount;
    }
}

