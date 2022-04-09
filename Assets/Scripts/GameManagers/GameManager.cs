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
    [SerializeField] Volume postProcessingVolume;
    [SerializeField] Vignette vignette;
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
    public int coinCount;


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

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayingArcade && Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
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
        }
    }

    public void ReloadLevel()
    {
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

