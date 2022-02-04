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

       // public scr_find_player _aiCore;

        //public GameObject player;
        [Header("Objects")]
        [SerializeField] scr_find_player _aiCore;
        [SerializeField] GameObject player;
        [SerializeField] Transform spawnPosition;
        [SerializeField] GameObject gameBounds;
        [SerializeField] GameObject astramoriCanvas;
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject upgradeMenu;
        [SerializeField] GameObject lossMenu;
        [SerializeField] Transform enemies;
        [SerializeField] AsterionLossScreen lossScreen;
        [SerializeField] VirtualCanvasCursor cursor;
        [SerializeField] Timer timer;
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
                && Input.GetKeyDown(KeyCode.Escape))
            {
                //_interactableManager.OnStopInteract.Invoke();
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
            lossScreen.insufficientFundsText.enabled = false;
        }

        public void ContinueCurrentGame()
        {
            isLost = false;
            _aiCore.enabled = true;
            _aiCore.m_Player = player;
            mainMenu.SetActive(false);
            upgradeMenu.SetActive(false);
            lossMenu.SetActive(false);
            lossScreen.insufficientFundsText.enabled = false;
            player.transform.position = spawnPosition.position;
            ApplyBonusStats();
            currentGameState = GameState.Gameplay;
            _playerMovement.enabled = true;

            StartCoroutine(CombatRoutine());
            GameManager.Instance.AlterCoins(-1);
        }

        public void GameConcluded(bool isWin)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (isWin)
            {
                lossScreen.gameStateText.text = "You Win!";
                cursor.EnableVirtualCursor();
                lossScreen.continueButtonText.text = "Start Again? (1 Quarter)";
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
                lossScreen.continueButtonText.text = "Continue? (1 Quarter)";
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
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().baseSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<PlayerMovement>().maxSpeed = player.GetComponent<PlayerMovement>().baseMaxSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<PlayerMovement>().damage = player.GetComponent<PlayerMovement>().baseDamage + GameManager.Instance.shipStats.attack;
            player.GetComponent<AsterionStarfighterHealth>().health = player.GetComponent<AsterionStarfighterHealth>().baseHealth + GameManager.Instance.shipStats.shield;
        }

        //sets fighter stats to default
        public void ResetStats()
        {
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().baseSpeed;
            player.GetComponent<PlayerMovement>().maxSpeed = player.GetComponent<PlayerMovement>().baseMaxSpeed;
            player.GetComponent<PlayerMovement>().damage = player.GetComponent<PlayerMovement>().baseDamage;
            player.GetComponent<AsterionStarfighterHealth>().health = player.GetComponent<AsterionStarfighterHealth>().baseHealth;
        }

        // placeholder enemy spawning system
        IEnumerator CombatRoutine()
        {
            cursor.DisableVirtualCursor();
            yield return new WaitForSeconds(1);

            while (enemyQueue.Count > 0)
            {
                GameObject ship = Instantiate(GameManager.Instance.alienShipPrefabs[enemyQueue[0] - 1], enemies);
                ship.layer = 7;
                Vector2 randomVector = Random.insideUnitCircle;
                randomVector.Normalize();
                //ship.transform.position = (Vector2)player.transform.position + (randomVector * Random.Range(minSpawnRange, maxSpawnRange));
                if (ship.TryGetComponent<scr_fighter_move>(out scr_fighter_move ship1))
                {
                    ship1.seeking = true;
                }
                enemyQueue.RemoveAt(0);
                //yield return new WaitForSeconds(spawnRate);



            }

            yield return new WaitUntil(() => enemies.childCount <= 0);

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
            else
            {
                lossScreen.insufficientFundsText.enabled = true;
            }

        }

        //exit this arcade machine and return to first person view
        public void ExitMachine()
        {
            cursor.DisableVirtualCursor();
            if (_cameraManager.currentCameraState == CameraManager.CameraState.Asterion)
            {
                _interactableManager.OnStopInteract.Invoke();
            }

            //StopInteractAction();
        }


    }
}