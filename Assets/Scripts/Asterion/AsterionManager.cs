using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FirstPersonPlayer;
using Interactable;

namespace AsterionArcade
{
    public class AsterionManager : InteractableBehaviour
    {
        private CameraManager _cameraManager;
        public PlayerMovement _playerMovement { get; private set; }

        [Header("Objects")]
        [SerializeField] scr_find_player _aiCore;
        [SerializeField] GameObject player;
        [SerializeField] Transform spawnPosition;
        [SerializeField] GameObject gameBounds;
        [SerializeField] GameObject asterionCanvas;
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject upgradeMenu;
        [SerializeField] GameObject lossMenu;
        [SerializeField] AsterionLossScreen lossScreen;
        [SerializeField] VirtualCanvasCursor cursor;
        [SerializeField] Transform enemies;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField] PowerManager powerManager;






        public enum GameState {Disabled, MainMenu, Upgrades, Gameplay, Invalid};
        [Header("Current Game State Info")]
        private GameState currentGameState;
        public bool isLost;
        public List<Vector2> baseEnemyQueue;
        public List<Vector2> enemyQueue;
 

        [Header("Asterion Spawning Rate/Range")]
        public float spawnRate;
        public float minSpawnRange;
        public float maxSpawnRange;
        [SerializeField] float sanityLoss;


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
                && Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.MainMenu)
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
            upgradeMenu.SetActive(true);
            lossMenu.SetActive(false);
            StopCoroutine(CombatRoutine());

            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.FirstPerson);
            _interactableManager.gameObject.SetActive(true);
        }

        public void CloseMainMenu()
        {
            if(GameManager.Instance.coinCount > 0)
            {
                GameManager.Instance.AlterCoins(-1);
                mainMenu.SetActive(false);
                currentGameState = GameState.Upgrades;
            }
            else
            {

            }
            
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
            GameManager.Instance.shipStats.ResetAllStats();
            enemyQueue = new List<Vector2>(baseEnemyQueue);
            cursor.EnableVirtualCursor();
            _aiCore.enabled = true;
            _aiCore.m_Player = player;
            currentGameState = GameState.MainMenu;
            mainMenu.SetActive(true);
            upgradeMenu.SetActive(true);
            lossMenu.SetActive(false);
            
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
                powerManager.isDraining = true;
                powerManager.GainPower();
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
                powerManager.isDraining = true;
                lossScreen.gameStateText.text = "You Lost!";
                cursor.EnableVirtualCursor();
                lossMenu.SetActive(true);
                lossScreen.continueButtonText.text = "Continue? (1 Quarter)";
                _playerMovement.enabled = false;
                StopAllCoroutines();
                _aiCore.enabled = false;
                foreach(BasicDamageable bd in enemies.GetComponentsInChildren<BasicDamageable>())
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
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().baseSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<PlayerMovement>().maxSpeed = player.GetComponent<PlayerMovement>().baseMaxSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<PlayerMovement>().damage = player.GetComponent<PlayerMovement>().baseDamage + GameManager.Instance.shipStats.attack;
            player.GetComponent<AsterionStarfighterHealth>().health = player.GetComponent<AsterionStarfighterHealth>().baseHealth + GameManager.Instance.shipStats.shield;
            virtualCamera.m_Lens.OrthographicSize = 5 + GameManager.Instance.shipStats.range;
        }

        //sets fighter stats to default
        public void ResetStats()
        {
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().baseSpeed;
            player.GetComponent<PlayerMovement>().maxSpeed = player.GetComponent<PlayerMovement>().baseMaxSpeed;
            player.GetComponent<PlayerMovement>().damage = player.GetComponent<PlayerMovement>().baseDamage;
            player.GetComponent<AsterionStarfighterHealth>().health = player.GetComponent<AsterionStarfighterHealth>().baseHealth;
            virtualCamera.m_Lens.OrthographicSize = 5;
        }

        // placeholder enemy spawning system
        IEnumerator CombatRoutine()
        {
            cursor.DisableVirtualCursor();
            yield return new WaitForSeconds(1);

            powerManager.isDraining = false;

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
                    yield return new WaitForSeconds(6f);
                }

                //yield return new WaitForSeconds(enemyQueue[0].y);
                GameObject ship = Instantiate(GameManager.Instance.alienShipPrefabs[(int)enemyQueue[0].x - 1], enemies);
                ship.layer = 7;
                Vector2 randomVector = Random.insideUnitCircle;
                randomVector.Normalize();
                ship.transform.position = (Vector2)player.transform.position + (randomVector * Random.Range(minSpawnRange, maxSpawnRange));
                if (ship.TryGetComponent<scr_fighter_move>(out scr_fighter_move ship1))
                {
                    ship1.seeking = true;
                }
                enemyQueue.RemoveAt(0);
                

                

            }

            yield return new WaitUntil(() => enemies.childCount <= 0);

            GameConcluded(true);

            yield return null;
        }

        //continue current round
        public void Continue()
        {
            if(GameManager.Instance.coinCount > 0)
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