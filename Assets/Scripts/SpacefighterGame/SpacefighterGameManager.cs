using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstPersonPlayer;
using Interactable;

namespace Spacefighter
{
    public class SpacefighterGameManager : InteractableBehaviour
    {
        private PlayerManager _playerManager;
        private TestArcadePlayer _testArcadePlayer;
        private CameraManager _cameraManager;

        public void Construct(PlayerManager playerManager, CameraManager cameraManager)
        {
            base.Construct(cameraManager);

            _playerManager = playerManager;
            _cameraManager = cameraManager;

            _testArcadePlayer = GetComponentInChildren<TestArcadePlayer>();
            _testArcadePlayer.Construct();
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
            _interactableManager.OnShowText.Invoke(false);

            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.Asterion);
        }

        public override void StopInteractAction()
        {
            _interactableManager.gameObject.SetActive(true);

            _cameraManager.OnChangeCameraState
                .Invoke(CameraManager.CameraState.FirstPerson);
        }
    }
}