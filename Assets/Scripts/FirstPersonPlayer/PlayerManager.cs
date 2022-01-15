using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Interactable;

namespace FirstPersonPlayer
{
    public class PlayerManager : MonoBehaviour
    {
        private Transform _playerTransform;
        private CharacterController _characterController;
        public PlayerMovement playerMovement { get; private set; }
        public CameraManager cameraManager { get; private set; }
        private InteractListManager _interactListManager;

        public void Construct()
        {
            _playerTransform = GetComponent<Transform>();

            _characterController = GetComponent<CharacterController>();

            _interactListManager = FindObjectOfType<InteractListManager>();
            _interactListManager.Construct();

            cameraManager = GetComponentInChildren<CameraManager>();
            cameraManager.Construct(this, _interactListManager);

            playerMovement = GetComponent<PlayerMovement>();
            playerMovement.Construct(_playerTransform
                , _characterController, cameraManager);
        }
    }
}