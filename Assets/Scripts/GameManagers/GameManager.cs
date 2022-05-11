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
    public GameObject pauseUI;
    public Transform astramoriEnemyBullets;
    public Transform asterionEnemyBullets;
    public AsterionManager asterionManager;
    public AstramoriManager astramoriManager;
    public SanityManager sanityManager;
    public PowerManager powerManager;
    public FirstPersonPlayer.PlayerMovement playerMovement;
    [SerializeField] PlayerLook playerLook;

    [Header("Game State")]
    public ShipStats shipStats;
    public bool isPaused;
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

    //acts as a singleton which can be easily referenced with GameManager.Instance

    void Awake()
    {
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
    }

    // Start is called before the first frame update
    void Start()
    {
        coinText.text = "" + coinCount;
        gameTime = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayingArcade && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }



        gameTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        
        

        UpdateTimeDisplay();


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

        // Dramatic Pause
        yield return new WaitForSeconds(4);

        // --  Play Tappings  --
        Debug.Log("TAPTAPTAPTAPTAP");

        yield return new WaitForSeconds(4);

        // Lock Player
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;

        // JUMP SCARE ANIMATION!
        tempLoseAnim.Play("tempLoseAnim");
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

        }
    }


    public void ReloadLevel()
    {
        Time.timeScale = 1;
        
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
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

