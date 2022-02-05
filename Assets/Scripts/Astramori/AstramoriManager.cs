using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FirstPersonPlayer;
using Interactable;

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
        [SerializeField] GameObject upgradeMenu;
        [SerializeField] GameObject lossMenu;
        [SerializeField] Transform enemies;
        [SerializeField] AstramoriLossScreen lossScreen;
        [SerializeField] VirtualCanvasCursor cursor;
        [SerializeField] AstramoriStarfighterHealth astramoriStarfighterHealth;
        [SerializeField] Timer timer;
        [SerializeField] Spawning spawningSystem;
        //public GameObject astramoriCanvas;

        public enum GameState { Disabled, MainMenu, Upgrades, Gameplay, Invalid };
        [Header("Current Game State Info")]
        public GameState currentGameState;
        public bool isLost;
        public List<int> enemyQueue;


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

            StartFreshGame();

        }

        public override void StopInteractAction()
        {
            _playerMovement.enabled = false;

            cursor.DisableVirtualCursor();
            _aiCore.enabled = false;
            currentGameState = GameState.Disabled;
            mainMenu.SetActive(true);
            upgradeMenu.SetActive(true);
            lossMenu.SetActive(false);
            StopCoroutine(CombatRoutine());

            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.FirstPerson);
            _interactableManager.gameObject.SetActive(true);
        }

        public void CloseMainMenu()
        {
            mainMenu.SetActive(false);
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
            upgradeMenu.SetActive(true);
            lossMenu.SetActive(false);
            GameManager.Instance.shipStats.ResetAllStats();
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

        public void GameConcluded(bool isWin)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            timer.StopTimer();
            starfighterAI.Deactivate();
            spawningSystem.isActive = false;

            if (isWin)
            {
                lossScreen.gameStateText.text = "You Win!";
                cursor.EnableVirtualCursor();
                lossScreen.fundsRewardedText.text = "" + (int)(((timer.time / timer.startingTime) * maxCoinRewardBonus)) + 1;
                GameManager.Instance.AlterCoins((int)(((timer.time / timer.startingTime) * maxCoinRewardBonus)) + 1);
                lossScreen.fundsRewardedText.enabled = true;
                lossMenu.SetActive(true);
                _aiCore.enabled = false;
                _playerMovement.enabled = false;
                foreach (BasicDamageable bd in enemies.GetComponentsInChildren<BasicDamageable>())
                {
                    bd.Death();
                }
                isLost = false;
            }
            else
            {
                lossScreen.gameStateText.text = "You Lost!";
                cursor.EnableVirtualCursor();
                lossMenu.SetActive(true);
                lossScreen.fundsRewardedText.enabled = false;
                _playerMovement.enabled = false;
                StopAllCoroutines();
                _aiCore.enabled = false;
                foreach (BasicDamageable bd in enemies.GetComponentsInChildren<BasicDamageable>())
                {
                    bd.Death();
                }
                isLost = true;
            }
        }

        //sets fighter stats to base + chosen upgrades
        public void ApplyBonusStats()
        {
            /*
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().baseSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<PlayerMovement>().maxSpeed = player.GetComponent<PlayerMovement>().baseMaxSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<PlayerMovement>().damage = player.GetComponent<PlayerMovement>().baseDamage + GameManager.Instance.shipStats.attack;
            */
            player.GetComponent<AstramoriStarfighterHealth>().health = player.GetComponent<AstramoriStarfighterHealth>().baseHealth + GameManager.Instance.shipStats.shield;
        }

        //sets fighter stats to default
        public void ResetStats()
        {
            /*
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().baseSpeed;
            player.GetComponent<PlayerMovement>().maxSpeed = player.GetComponent<PlayerMovement>().baseMaxSpeed;
            player.GetComponent<PlayerMovement>().damage = player.GetComponent<PlayerMovement>().baseDamage;
            */
            player.GetComponent<AstramoriStarfighterHealth>().health = player.GetComponent<AstramoriStarfighterHealth>().baseHealth;

        }

        // placeholder enemy spawning system
        IEnumerator CombatRoutine()
        {
            spawningSystem.isActive = true;
            starfighterAI.SetActive();
            //cursor.DisableVirtualCursor();
            timer.StartTimer();
            yield return new WaitForSeconds(1);



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