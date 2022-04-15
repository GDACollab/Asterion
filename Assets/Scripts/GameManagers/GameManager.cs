using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using AsterionArcade;
using FirstPersonPlayer;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> alienShipPrefabs;

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Volume postProcessingVolume;
    [SerializeField] Vignette vignette;
    [SerializeField] private GameObject FPupgradesDisplay;
    [SerializeField] private Animator tempLoseAnim;
    public GameObject pauseUI;
    public Transform astramoriEnemyBullets;
    public Transform asterionEnemyBullets;
    public AsterionManager asterionManager;
    public AstramoriManager astramoriManager;
    public SanityManager sanityManager;
    public FirstPersonPlayer.PlayerMovement playerMovement;
    [SerializeField] PlayerLook playerLook;

    [Header("Game State")]
    public ShipStats shipStats;
    public bool isPaused;
    public bool isPlayingArcade;
    public bool gameLost;
    public int coinCount;
    public float gameTime;


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
        
        if (!isPlayingArcade)
        {
            FPupgradesDisplay.SetActive(true);
        }
        else
        {
            FPupgradesDisplay.SetActive(false);
        }

        UpdateTimeDisplay();


    }

    public IEnumerator LoseRoutine()
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

