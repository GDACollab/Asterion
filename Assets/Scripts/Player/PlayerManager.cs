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

        private void Awake()
        {
            _playerTransform = gameObject.GetComponent<Transform>();

            _characterController = gameObject.GetComponent<CharacterController>();

            _cameraManager = gameObject.GetComponentInChildren<CameraManager>();
            _cameraManager.Construct();

            _playerMovement = gameObject.GetComponent<PlayerMovement>();
            _playerMovement.Construct(_playerTransform
                , _characterController, _cameraManager);
        }
    }
}