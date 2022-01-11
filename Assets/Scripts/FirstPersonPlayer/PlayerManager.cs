using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstPersonPlayer
{
    public class PlayerManager : MonoBehaviour
    {
        private Transform _playerTransform;
        private CharacterController _characterController;
        private PlayerMovement _playerMovement;
        private CameraManager _cameraManager;

        public void Construct()
        {
            _playerTransform = GetComponent<Transform>();

            _characterController = GetComponent<CharacterController>();

            _cameraManager = GetComponentInChildren<CameraManager>();
            _cameraManager.Construct();

            _playerMovement = GetComponent<PlayerMovement>();
            _playerMovement.Construct(_playerTransform
                , _characterController, _cameraManager);
        }
    }
}