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
        [SerializeField] GameObject asterionCanvas;
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject upgradeMenu;

        public enum GameState {Disabled, MainMenu, Upgrades, Gameplay};
        public GameState currentGameState;

        
        

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
            _playerMovement.enabled = true;
            _interactableManager.gameObject.SetActive(false);

            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.Asterion);

            _aiCore.enabled = true;
            _aiCore.m_Player = player;
            currentGameState = GameState.MainMenu;
            mainMenu.SetActive(true);
            upgradeMenu.SetActive(true);
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
        }

        public void CloseMainMenu()
        {
            mainMenu.SetActive(false);
        }

        public void CloseUpgradeScreen()
        {
            upgradeMenu.SetActive(false);
        }


    }
}