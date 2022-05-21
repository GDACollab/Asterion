using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FirstPersonPlayer;
using Interactable;
using TMPro;

namespace AsterionArcade
{
    public class AsterionManager : InteractableBehaviour
    {
        private CameraManager _cameraManager;
        public PlayerMovement _playerMovement { get; private set; }

        [Header("Objects")]
        //[SerializeField] scr_find_player _aiCore;
        [SerializeField] GameObject player;
        [SerializeField] Transform cameraSpawnPosition;
        [SerializeField] Transform spawnPosition;
        [SerializeField] GameObject gameBounds;
        [SerializeField] GameObject asterionCanvas;
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject tutorialMenu;
        [SerializeField] GameObject upgradeMenu;
        [SerializeField] GameObject lossMenu;
        [SerializeField] GameObject asterionCabinet;
        [SerializeField] AsterionLossScreen lossScreen;
        [SerializeField] VirtualCanvasCursor cursor;
        [SerializeField] Transform enemies;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField] PowerManager powerManager;
        [SerializeField] TextMeshProUGUI insufficientFundsText;
        [SerializeField] TextMeshProUGUI shipStatusText;
        [SerializeField] List<TextMeshProUGUI> pretexts;
        [SerializeField] Door asterionDoor;

        public enum GameState {Disabled, MainMenu, Upgrades, Gameplay, Invalid};
        [Header("Current Game State Info")]
        private GameState currentGameState;
        public bool isLost;
        public bool isVictory;
        public bool isPlaying;
        public List<Vector2> baseEnemyQueue;
        public List<Vector2> enemyQueue;
        public float batteryEarned;
        public int timesWon;
        //Added Variable to skip Upgrades on first play
        private bool firstTime = true;


        [Header("Asterion Spawning Rate/Range")]
        public float spawnRate;
        public float minSpawnRange;
        public float maxSpawnRange;
        [SerializeField] float sanityLoss;


        [Header("SFX References")]
        [SerializeField] FMODUnity.EventReference coinInsertSFX;
        private FMOD.Studio.EventInstance coinInsertSFX_instance;
        [SerializeField] FMODUnity.EventReference playerDiesSFX;
        [SerializeField] FMODUnity.EventReference victoryFanfareSFX;
        [SerializeField] AsterionMusicManager asterionMusicManager;


        [Header("Pretext")]
        [SerializeField] List<string> pretextFirst;
        [SerializeField] List<string> pretextSecond;
  


        void Start()
        {
            // SFX stuff
            coinInsertSFX_instance = FMODUnity.RuntimeManager.CreateInstance(coinInsertSFX);

        }

        public new void Construct(CameraManager cameraManager)
        {
            base.Construct(cameraManager);

            _cameraManager = cameraManager;

            _playerMovement = GetComponentInChildren<PlayerMovement>();

            if (_playerMovement == null)
            {
                Debug.LogError(_playerMovement
                    + " must be defined as child of " + this);
            }

            _playerMovement.enabled = false;
        }

        private void Update()
        {
            // TODO This is messy, only the CameraManager
            // should have input that effect it
            if (_cameraManager.currentCameraState
                == CameraManager.CameraState.Asterion
                && Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.MainMenu && firstTime == false)
            {
                _interactableManager.OnStopInteract.Invoke();
            }


        }

        public override void InteractAction()
        {
            
            _interactableManager.gameObject.SetActive(false);

            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.Asterion);

            GameManager.Instance.isPlayingArcade = true;
            GameManager.Instance.CheckPlayerIsPlayingArcadeStatus();

            StartFreshGame();

        }
        
        public override void StopInteractAction()
        {

            // Music
            asterionMusicManager.PlayMusic("idle");

            _playerMovement.enabled = false;
            

            
            //_aiCore.enabled = false;
            currentGameState = GameState.Disabled;
            //GameManager.Instance.isPlayingArcade = false;
            GameManager.Instance.CheckPlayerIsPlayingArcadeStatus();

            mainMenu.SetActive(true);
            upgradeMenu.SetActive(false);
            
            insufficientFundsText.enabled = false;
           
            StopCoroutine(CombatRoutine());
           
            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.FirstPerson);

            if (_interactableManager.gameEnding == false)
            {
                _interactableManager.gameObject.SetActive(true);
            }
            
            lossMenu.SetActive(false);
            cursor.DisableVirtualCursor();
        }

        public void CloseMainMenu()
        {
            Debug.Log("closing main menu asterion");

            if(GameManager.Instance.coinCount > 0)
            {
                mainMenu.SetActive(false);
                GameManager.Instance.AlterCoins(-1);
                
                // SFX
                coinInsertSFX_instance.start();
                asterionMusicManager.PlayMusic("menu");

                upgradeMenu.SetActive(true);
                ShipStats.instance.UpdateOld();
                upgradeMenu.GetComponent<UpgradeMenu>().upgradePoints = 1;
                upgradeMenu.GetComponent<UpgradeMenu>().UpdateButtonVisibility();
                currentGameState = GameState.Upgrades;
                insufficientFundsText.enabled = false;
                //GameManager.Instance.shipStats.ResetAllStats();
                if (firstTime)
                {
                    CloseUpgradeScreen();
                    firstTime = false;
                }
                else
                {
                    firstTime = false;
                }

            }
            else
            {
                insufficientFundsText.enabled = true;
            }
            
        }

        public void CloseUpgradeScreen()
        {
            player.transform.position = spawnPosition.position;
            //ResetStats();
            ApplyBonusStats();
            isPlaying = true;
            currentGameState = GameState.Gameplay;
            
            Debug.Log("closing upgrade screen for asterion, starting game!");
            StartCoroutine(CombatRoutine());
            upgradeMenu.SetActive(false);
            asterionMusicManager.PlayMusic("main");
            batteryEarned = 0;

            //GameManager.Instance.AlterCoins(-1);
        }

        public void StartFreshGame()
        {
            
            mainMenu.SetActive(true);
            isLost = false;
            isVictory = false;
            asterionMusicManager.PlayMusic("idle");

            enemyQueue = new List<Vector2>(baseEnemyQueue);
            cursor.EnableVirtualCursor();
            //_aiCore.enabled = true;
            //_aiCore.m_Player = player;
            currentGameState = GameState.MainMenu;
            
            upgradeMenu.SetActive(false);
            ShipStats.instance.ResetModified();
            lossMenu.SetActive(false);
            insufficientFundsText.enabled = false;

            lossScreen.insufficientFundsText.enabled = false;
            
        }

        public void OpenTutorial()
        {
            mainMenu.SetActive(false);
            tutorialMenu.SetActive(true);
        }

        public void CloseTutorial()
        {
            mainMenu.SetActive(true);
            tutorialMenu.SetActive(false);
        }

        public void ContinueCurrentGame()
        {
            isLost = false;
            isPlaying = true;
            //_aiCore.enabled = true;
            //_aiCore.m_Player = player;

            // SFX
            coinInsertSFX_instance.start();
            asterionMusicManager.PlayMusic("main");

            upgradeMenu.SetActive(false);
            lossMenu.SetActive(false);
            lossScreen.insufficientFundsText.enabled = false;
            player.transform.position = spawnPosition.position;
            ApplyBonusStats();
            currentGameState = GameState.Gameplay;
            _playerMovement.enabled = true;
            GameManager.Instance.AlterCoins(-1);
            mainMenu.SetActive(false);
            StartCoroutine(CombatRoutine());
            
        }

        public void GameConcluded(bool isWin)
        {
            if (isPlaying)
            {
                isPlaying = false;

                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Debug.Log("games concluded");

                asterionMusicManager.PlayMusic("stop all");

                if (isWin)
                {

                    // Music
                    FMODUnity.RuntimeManager.PlayOneShotAttached(victoryFanfareSFX.Guid, asterionCabinet);
                    timesWon++;
                    powerManager.GainPowerIncrement(batteryEarned);
                    batteryEarned = 0;
                    GameManager.Instance.sanityManager.BoostSanity();
                    lossScreen.gameStateText.text = "You Win!";
                    cursor.EnableVirtualCursor();
                    lossScreen.continueButtonText.text = "Start Again? (1 Quarter)";
                    lossMenu.SetActive(true);
                    //_aiCore.enabled = false;
                    _playerMovement.enabled = false;
                    isVictory = true;
                    ShipStats.instance.UpdateOld();

                    GameManager.Instance.asterionGamesPlayed++;

                    foreach (BasicDamageable bd in enemies.GetComponentsInChildren<BasicDamageable>())
                    {
                        bd.Death();
                    }

                    foreach (Transform bullet in GameManager.Instance.asterionEnemyBullets)
                    {
                        Destroy(bullet.gameObject);
                    }

                    isLost = false;

                    ShipStats.instance.ResetModified();



                }
                else
                {
                    
                    // Play the SFX that plays when the alien ship fucking explodes
                    FMODUnity.RuntimeManager.PlayOneShot(playerDiesSFX.Guid);
                    
                    GameManager.Instance.asterionGamesPlayed++;

                    powerManager.IncreaseRate();
                    lossScreen.gameStateText.text = "You Lost!";
                    cursor.EnableVirtualCursor();
                    lossMenu.SetActive(true);
                    lossScreen.continueButtonText.text = "Continue? (1 Quarter)";
                    _playerMovement.enabled = false;
                    ShipStats.instance.DowngradeOld();
                    StopAllCoroutines();
                    
                    //_aiCore.enabled = false;

                    foreach (Enemy fighterMove in enemies.GetComponentsInChildren<Enemy>())
                    {
                        enemyQueue.Add(new Vector2(fighterMove.AIType, 3));
                    }

                    foreach (BasicDamageable bd in enemies.GetComponentsInChildren<BasicDamageable>())
                    {
                        bd.Death();
                    }

                    foreach (Transform bullet in GameManager.Instance.asterionEnemyBullets)
                    {
                        Destroy(bullet.gameObject);
                    }

                    GameManager.Instance.sanityManager.UpdateSanity(-sanityLoss);
                    isLost = true;



                }
            }
            

            
        }

        //sets fighter stats to base + chosen upgrades
        public void ApplyBonusStats()
        {
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().baseSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<PlayerMovement>().maxSpeed = player.GetComponent<PlayerMovement>().baseMaxSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<PlayerMovement>().damage = player.GetComponent<PlayerMovement>().baseDamage + GameManager.Instance.shipStats.attack;
            player.GetComponent<AsterionStarfighterHealth>().health = player.GetComponent<AsterionStarfighterHealth>().baseHealth + GameManager.Instance.shipStats.shield;
            virtualCamera.m_Lens.OrthographicSize = 6 + (GameManager.Instance.shipStats.range / 2.3f);
        }

        //sets fighter stats to default
        public void ResetStats()
        {
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().baseSpeed;
            player.GetComponent<PlayerMovement>().maxSpeed = player.GetComponent<PlayerMovement>().baseMaxSpeed;
            player.GetComponent<PlayerMovement>().damage = player.GetComponent<PlayerMovement>().baseDamage;
            player.GetComponent<AsterionStarfighterHealth>().health = player.GetComponent<AsterionStarfighterHealth>().baseHealth;
            virtualCamera.m_Lens.OrthographicSize = 6;
        }

        // placeholder enemy spawning system
        IEnumerator CombatRoutine()
        {
            //cursor.DisableVirtualCursor();
            virtualCamera.transform.position = cameraSpawnPosition.position;
            int totalShips = enemyQueue.Count;
            shipStatusText.text = "Ships Remaining\nto be deployed: (" + totalShips + "/" + totalShips + ")";
            yield return new WaitForSeconds(1f);
            
            if(timesWon == 0)
            {
                pretexts[0].text = "This is the enemy wave.";
            }
            else
            {
                int id = Random.Range(0, pretextFirst.Count-1);
                if(powerManager.powerLevel <= 25)
                {
                    id = pretextFirst.Count - 1;
                }
                pretexts[0].text = pretextFirst[id];
                pretexts[1].text = pretextSecond[id];

            }
            pretexts[0].enabled = true;

            yield return new WaitForSeconds(2f);

            pretexts[1].enabled = true;

            yield return new WaitForSeconds(2f);

            pretexts[0].enabled = false;
            pretexts[1].enabled = false;

            _playerMovement.enabled = true;

            yield return new WaitForSeconds(1);

            int numFighters = 3;

            while (enemyQueue.Count > 0)
            {
                
                if(enemyQueue[0].x == 1 && numFighters > 0)
                {
                    numFighters--;
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    if(numFighters <= 0)
                    {
                        numFighters = 3;
                    }
                    yield return new WaitForSeconds(3f);
                }

                //yield return new WaitForSeconds(enemyQueue[0].y);
                GameObject ship = Instantiate(GameManager.Instance.alienShipPrefabs[(int)enemyQueue[0].x - 1], enemies);
                ship.layer = 7;
                Vector2 randomVector = Random.insideUnitCircle;
                randomVector.Normalize();
                ship.transform.position = (Vector2)player.transform.position + (randomVector * Random.Range(minSpawnRange, maxSpawnRange));
                if (ship.TryGetComponent<Enemy>(out Enemy ship1))
                {
                    ship1.lookingForPlayer = true;
                }
                enemyQueue.RemoveAt(0);
                shipStatusText.text = "Ships Remaining\nto be deployed: (" + enemyQueue.Count + "/" + totalShips + ")";




            }

            yield return new WaitUntil(() => enemies.childCount <= 0);

            GameConcluded(true);

            yield return null;
        }

        public void ForceDoorOpen()
        {
            asterionDoor.locked = false;
            asterionDoor.openDoor();
        }

        //continue current round
        public void Continue()
        {
            if(GameManager.Instance.coinCount > 0)
            {
                if (isLost)
                {
                    ContinueCurrentGame();
                    ShipStats.instance.RefundContinue();
                    
                }
                else
                {
                    StartFreshGame();
                }
            }
            else
            {
                lossScreen.insufficientFundsText.enabled = true;
            }
            
        }

        public void ExitMachine()
        {
            
            
            if (_cameraManager.currentCameraState == CameraManager.CameraState.Asterion)
            {
                _interactableManager.OnStopInteract.Invoke();
                if(GameManager.Instance.asterionGamesPlayed == 1)
                {
                    GameObject.Find("GameManagerObject").GetComponent<Tutorial_Sequence>().LockPlayerAndSlideDoor();
                }
                
            }
            
            //StopInteractAction();
        }



    }
}