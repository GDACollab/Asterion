using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FirstPersonPlayer;
using Interactable;
using TMPro;

namespace AsterionArcade
{
    public class AstramoriManager : InteractableBehaviour
    {
        private CameraManager _cameraManager;
        public Spawning _playerMovement { get; private set; }

        public int maxCoinRewardBonus;
        // public scr_find_player _aiCore;
        
        //public GameObject player;
        [Header("Objects")]
        [SerializeField] scr_find_player _aiCore;
        [SerializeField] GameObject player;
        private Starfighter starfighterAI;
        [SerializeField] Transform spawnPosition;
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
        bool canReward;
        public int shipsDeployed;
        //public GameObject astramoriCanvas;

        public enum GameState { Disabled, MainMenu, Upgrades, Gameplay, Invalid };
        [Header("Current Game State Info")]
        private GameState currentGameState;
        public bool isLost;
        public List<Vector2> enemyQueue;

        [SerializeField] float sanityLoss;


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

            StartFreshGame();

        }

        public override void StopInteractAction()
        {
            _playerMovement.enabled = false;

            cursor.DisableVirtualCursor();
            _aiCore.enabled = false;
            currentGameState = GameState.Disabled;
            GameManager.Instance.isPlayingArcade = false;
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
            _aiCore.enabled = true;
            _aiCore.m_Player = player;
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
            _aiCore.enabled = true;
            _aiCore.m_Player = player;
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
                }
                
                canReward = false;
                lossScreen.fundsRewardedText.enabled = true;
                lossMenu.SetActive(true);
                _aiCore.enabled = false;
                _playerMovement.enabled = false;
                foreach (BasicDamageable bd in enemies.GetComponentsInChildren<BasicDamageable>())
                {
                    bd.Death();
                }
                isLost = false;
                GameManager.Instance.asterionManager.baseEnemyQueue = new List<Vector2>(enemyQueue);
            }
            else
            {
                lossScreen.gameStateText.text = "Game Over";
                cursor.EnableVirtualCursor();
                lossMenu.SetActive(true);
                lossScreen.fundsRewardedText.enabled = false;
                timeText.enabled = false;
                _playerMovement.enabled = false;
                StopAllCoroutines();
                _aiCore.enabled = false;
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
            virtualCamera.m_Lens.OrthographicSize = 7 + GameManager.Instance.shipStats.range;
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
            }

            //StopInteractAction();
        }


    }
}