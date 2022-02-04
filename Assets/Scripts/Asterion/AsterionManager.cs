using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] Transform enemies;
        



        public enum GameState {Disabled, MainMenu, Upgrades, Gameplay, Invalid};
        [Header("Current Game State Info")]
        public GameState currentGameState;
        public List<int> enemyQueue;

        [Header("Asterion Spawning Rate/Range")]
        public float spawnRate;
        public float minSpawnRange;
        public float maxSpawnRange;


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
                && Input.GetKeyDown(KeyCode.Escape))
            {
                _interactableManager.OnStopInteract.Invoke();
            }


        }

        public override void InteractAction()
        {
            
            _interactableManager.gameObject.SetActive(false);

            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.Asterion);

            _aiCore.enabled = true;
            _aiCore.m_Player = player;
            currentGameState = GameState.MainMenu;
            mainMenu.SetActive(true);
            upgradeMenu.SetActive(true);
            lossMenu.SetActive(false);
            GameManager.Instance.shipStats.ResetAllStats();

        }

        public override void StopInteractAction()
        {
            _playerMovement.enabled = false;
            _interactableManager.gameObject.SetActive(true);

            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.FirstPerson);

            _aiCore.enabled = false;
            currentGameState = GameState.Disabled;
            mainMenu.SetActive(true);
            upgradeMenu.SetActive(true);
            lossMenu.SetActive(false);
            StopCoroutine(CombatRoutine());
        }

        public void CloseMainMenu()
        {
            mainMenu.SetActive(false);
            currentGameState = GameState.Upgrades;
        }

        public void CloseUpgradeScreen()
        {
            player.transform.position = spawnPosition.position;
            ResetStats();
            ApplyBonusStats();
            upgradeMenu.SetActive(false);
            currentGameState = GameState.Gameplay;
            _playerMovement.enabled = true;
            
            StartCoroutine(CombatRoutine());

            GameManager.Instance.AlterCoins(-1);
        }

        public void GameConcluded(bool isWin)
        {
            if (isWin)
            {
                _playerMovement.enabled = false;
            }
            else
            {
                lossMenu.SetActive(true);
                _playerMovement.enabled = false;
                StopAllCoroutines();
                _aiCore.enabled = false;
            }
        }

        public void ApplyBonusStats()
        {
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().baseSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<PlayerMovement>().maxSpeed = player.GetComponent<PlayerMovement>().baseMaxSpeed + GameManager.Instance.shipStats.thruster;
            player.GetComponent<PlayerMovement>().damage = player.GetComponent<PlayerMovement>().baseDamage + GameManager.Instance.shipStats.attack;
            player.GetComponent<AsterionStarfighterHealth>().health = player.GetComponent<AsterionStarfighterHealth>().baseHealth + GameManager.Instance.shipStats.shield;
        }

        public void ResetStats()
        {
            player.GetComponent<PlayerMovement>().moveSpeed = player.GetComponent<PlayerMovement>().baseSpeed;
            player.GetComponent<PlayerMovement>().maxSpeed = player.GetComponent<PlayerMovement>().baseMaxSpeed;
            player.GetComponent<PlayerMovement>().damage = player.GetComponent<PlayerMovement>().baseDamage;
            player.GetComponent<AsterionStarfighterHealth>().health = player.GetComponent<AsterionStarfighterHealth>().baseHealth;
        }

        IEnumerator CombatRoutine()
        {
            yield return new WaitForSeconds(1);

            while (enemyQueue.Count > 0)
            {
                GameObject ship = Instantiate(GameManager.Instance.alienShipPrefabs[enemyQueue[0] - 1], enemies);
                ship.layer = 7;
                Vector2 randomVector = Random.insideUnitCircle;
                randomVector.Normalize();
                ship.transform.position = (Vector2)player.transform.position + (randomVector * Random.Range(minSpawnRange, maxSpawnRange));
                if (ship.TryGetComponent<scr_fighter_move>(out scr_fighter_move ship1))
                {
                    ship1.seeking = true;
                }
                enemyQueue.RemoveAt(0);
                yield return new WaitForSeconds(spawnRate);

                

            }

            GameConcluded(true);

            yield return null;
        }


    }
}