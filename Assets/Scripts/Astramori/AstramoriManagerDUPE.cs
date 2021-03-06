using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FirstPersonPlayer;
using Interactable;
using TMPro;

namespace AsterionArcade
{
    public class AstramoriManagerDUPE : InteractableBehaviour
    {
        private CameraManager _cameraManager;
        public Spawning _playerMovement { get; private set; }

        public int maxCoinRewardBonus;
        // public scr_find_player _aiCore;
        
        //public GameObject player;
        [Header("Objects")]
        [SerializeField] GameObject player;
        private Starfighter starfighterAI;
        [SerializeField] Transform spawnPosition;
        [SerializeField] Transform cameraSpawnPosition;
        [SerializeField] GameObject gameBounds;
        [SerializeField] GameObject astramoriCanvas;
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject tutorialMenu;
        [SerializeField] GameObject upgradeMenu;
        [SerializeField] GameObject lossMenu;
        public Transform enemies;
        [SerializeField] AstramoriLossScreen lossScreen;
        [SerializeField] VirtualCanvasCursor cursor;
        [SerializeField] AstramoriStarfighterHealth astramoriStarfighterHealth;
        [SerializeField] Timer timer;
        [SerializeField] Spawning spawningSystem;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField] UpgradeDisplay upgradeDisplay;
        [SerializeField] TextMeshProUGUI shipStatusText;
        [SerializeField] List<TextMeshProUGUI> pretexts;
        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] TextMeshProUGUI fpShipCountText;
        [SerializeField] TextMeshProUGUI tutorialText;
        [SerializeField] Door astramoriDoor;
        bool canReward;
        public int shipsDeployed;
        //public GameObject astramoriCanvas;
        // Randy: Attempt at fixing Starfighter from leaving gray box, see ApplyBonusStats()
        [SerializeField] GameObject PlacementZone;
        public Vector3 zoneBaseSize;

        public enum GameState { Disabled, MainMenu, Upgrades, Gameplay, Invalid };
        [Header("Current Game State Info")]
        private GameState currentGameState;
        public bool isLost;
        public List<Vector2> enemyQueue;

        [SerializeField] float sanityLoss;

        [Header("SFX Emitters")]
        [SerializeField] FMODUnity.EventReference coinDispenseManySFX;
        private FMOD.Studio.EventInstance coinDispenseManySFX_instance;

        private bool tutorialTrigger = false;

        void Start()
        {
            // SFX stuff
            coinDispenseManySFX_instance = FMODUnity.RuntimeManager.CreateInstance(coinDispenseManySFX);

        }

        public new void Construct(CameraManager cameraManager)
        {
            base.Construct(cameraManager);

            _cameraManager = cameraManager;

            _playerMovement = GetComponentInChildren<Spawning>();
            starfighterAI = player.transform.GetComponent<Starfighter>();

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
                == CameraManager.CameraState.Astramori
                && Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.MainMenu)
            {
                _interactableManager.OnStopInteract.Invoke();
            }
        }

        public override void InteractAction()
        {

            _interactableManager.gameObject.SetActive(false);

            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.Astramori);

            GameManager.Instance.isPlayingArcade = true;
            GameManager.Instance.CheckPlayerIsPlayingArcadeStatus();

            StartFreshGame();

        }

        public override void StopInteractAction()
        {
            _playerMovement.enabled = false;
            
            cursor.DisableVirtualCursor();
            //_aiCore.enabled = false;
            currentGameState = GameState.Disabled;
            
            
            mainMenu.SetActive(true);
            upgradeMenu.SetActive(false);
            lossMenu.SetActive(false);
            StopCoroutine(CombatRoutine());

            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.FirstPerson);
            _interactableManager.gameObject.SetActive(true);

            
        }

        public void CloseMainMenu()
        {
            mainMenu.SetActive(false);
            upgradeMenu.SetActive(true);
            Debug.Log("closing main menu on astramori");
            currentGameState = GameState.Upgrades;
        }

        public void CloseUpgradeScreen()
        {
            
            player.transform.position = spawnPosition.position;
            virtualCamera.transform.position = spawnPosition.position;
            virtualCamera.ForceCameraPosition(spawnPosition.position, Quaternion.identity);
            //ResetStats();
            ApplyBonusStats();
            upgradeMenu.SetActive(false);
            currentGameState = GameState.Gameplay;
            _playerMovement.enabled = true;

            StartCoroutine(CombatRoutine());

            //GameManager.Instance.AlterCoins(-1);
        }

        public void StartFreshGame()
        {
            
            isLost = false;
            enemyQueue.Clear();
            cursor.EnableVirtualCursor();
           // _aiCore.enabled = true;
            //_aiCore.m_Player = player;
            currentGameState = GameState.MainMenu;
            mainMenu.SetActive(true);
            upgradeMenu.SetActive(false);
            lossMenu.SetActive(false);
            //GameManager.Instance.shipStats.ResetAllStats();
            lossScreen.fundsRewardedText.enabled = false;
        }

        public void ContinueCurrentGame()
        {
            isLost = false;
            //_aiCore.enabled = true;
            //_aiCore.m_Player = player;
            mainMenu.SetActive(false);
            upgradeMenu.SetActive(false);
            lossMenu.SetActive(false);
            lossScreen.fundsRewardedText.enabled = false;
            player.transform.position = spawnPosition.position;
            ApplyBonusStats();
            currentGameState = GameState.Gameplay;
            _playerMovement.enabled = true;

            StartCoroutine(CombatRoutine());

        }

        public void OpenTutorial()
        {
            mainMenu.SetActive(false);
            tutorialMenu.SetActive(true);
            tutorialText.text = "Select enemies to send against the starship\n\nSelect area outside border to spawn enemies\n\nContinue sending enemies until the starship is destroyed\n\nDestroying ships before time runs out earns quarters";
        }

        public void TutorialNext()
        {
            tutorialText.text = "Fighter: The fastest, smallest, and weakest of all enemy types.\n\nMissile Frigate: If the player is in range, they fire three heat-seeking missiles in sequence.\n\nCruiser: These capital ships are very slow, require five hits to destroy, and have three cannons";
        }

        public void CloseTutorial()
        {
            mainMenu.SetActive(true);
            tutorialMenu.SetActive(false);
        }
        public void GameConcluded(bool isWin)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            timer.StopTimer();
            starfighterAI.Deactivate();
            spawningSystem.isActive = false;

            if (isWin)
            {
                lossScreen.gameStateText.text = "Victory";
                cursor.EnableVirtualCursor();
                int quarters = ((int)(((timer.time / timer.startingTime)) * maxCoinRewardBonus) + 1);
                lossScreen.fundsRewardedText.text = "Quarters Recieved: " + quarters;
                timeText.enabled = true;
                timeText.text = "Time Remaining:\n" + (int)(timer.time) + "s";
                if (canReward)
                {
                    GameManager.Instance.AlterCoins(quarters);

                    // SFX
                    coinDispenseManySFX_instance.start();
                }
                
                canReward = false;
                lossScreen.fundsRewardedText.enabled = true;
                lossMenu.SetActive(true);
                GameManager.Instance.astramoriGamesPlayed++;
                //_aiCore.enabled = false;
                _playerMovement.enabled = false;
                foreach (BasicDamageable bd in enemies.GetComponentsInChildren<BasicDamageable>())
                {
                    bd.Death();
                }
                isLost = false;
                GameManager.Instance.asterionManager.baseEnemyQueue = new List<Vector2>(enemyQueue);
                fpShipCountText.text = "Ships: " + shipsDeployed;
                
                
            }
            else
            {
                lossScreen.gameStateText.text = "Game Over";
                cursor.EnableVirtualCursor();
                lossMenu.SetActive(true);
                lossScreen.fundsRewardedText.enabled = false;
                timeText.enabled = false;
                _playerMovement.enabled = false;
                GameManager.Instance.astramoriGamesPlayed++;
                StopAllCoroutines();
               // _aiCore.enabled = false;
                foreach (BasicDamageable bd in enemies.GetComponentsInChildren<BasicDamageable>())
                {
                    bd.Death();
                }
                GameManager.Instance.sanityManager.UpdateSanity(-sanityLoss);
                isLost = true;
            }
        }

        //sets fighter stats to base + chosen upgrades
        public void ApplyBonusStats()
        {
            
            player.GetComponent<Starfighter>().speed = player.GetComponent<Starfighter>().baseSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<Starfighter>().damage = player.GetComponent<Starfighter>().baseDamage + GameManager.Instance.shipStats.attack;
            
            player.GetComponent<AstramoriStarfighterHealth>().health = player.GetComponent<AstramoriStarfighterHealth>().baseHealth + GameManager.Instance.shipStats.shield;
            // Randy: Scale Placement zone size to account for change in range upgrade applied to camera
            PlacementZone.transform.localScale = zoneBaseSize + new Vector3((GameManager.Instance.shipStats.range * 2.56f),
                                                                            (GameManager.Instance.shipStats.range * 2.56f),
                                                                            (GameManager.Instance.shipStats.range * 2.56f));
            virtualCamera.m_Lens.OrthographicSize = 7 + (GameManager.Instance.shipStats.range / 2.3f);
        }

        //sets fighter stats to default
        public void ResetStats()
        {
            
            player.GetComponent<Starfighter>().speed = player.GetComponent<Starfighter>().baseSpeed;
            player.GetComponent<Starfighter>().damage = player.GetComponent<Starfighter>().baseDamage;
            virtualCamera.m_Lens.OrthographicSize = 7;
            player.GetComponent<AstramoriStarfighterHealth>().health = player.GetComponent<AstramoriStarfighterHealth>().baseHealth;

        }

        // placeholder enemy spawning system
        IEnumerator CombatRoutine()
        {
            player.transform.position = spawnPosition.position;
            virtualCamera.transform.position = spawnPosition.position;
            shipStatusText.text = "Ship Count: (" + enemies.childCount + "/" + shipsDeployed + ")";
            yield return new WaitForSeconds(1f);
            pretexts[0].enabled = true;

            yield return new WaitForSeconds(2f);

            pretexts[1].enabled = true;

            yield return new WaitForSeconds(2f);

            pretexts[0].enabled = false;
            pretexts[1].enabled = false;

            shipsDeployed = 0;
            spawningSystem.isActive = true;

            starfighterAI.SetActive();
            //cursor.DisableVirtualCursor();
            timer.StartTimer();
            canReward = true;
            yield return new WaitForSeconds(1);

            while(astramoriStarfighterHealth.health > 0)
            {
                shipStatusText.text = "Ship Count: (" + enemies.childCount + "/" + shipsDeployed + ")";
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitUntil(() => astramoriStarfighterHealth.health <= 0);

            GameConcluded(true);

            yield return null;
        }

        //continue current round
        public void Continue()
        {
            if (GameManager.Instance.coinCount > 0)
            {
                if (isLost)
                {
                    ContinueCurrentGame();
                }
                else
                {
                    StartFreshGame();
                }
            }


        }

        //exit this arcade machine and return to first person view
        public void ExitMachine()
        {
            cursor.DisableVirtualCursor();
            if (_cameraManager.currentCameraState == CameraManager.CameraState.Astramori)
            {
                _interactableManager.OnStopInteract.Invoke();
                ForceDoorOpen();
                if (tutorialTrigger == false)
                {
                    tutorialTrigger = true;
                    GameObject.Find("GameManagerObject").GetComponent<Tutorial_Sequence>().StartCoroutine("EventThree");
                }

            }

            //StopInteractAction();
        }

        // Opens the Astramori Door
        public void ForceDoorOpen()
        {
            astramoriDoor.locked = false;
            astramoriDoor.openDoor();
        }
    }
}