using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstPersonPlayer
{
    public class PlayerLook : MonoBehaviour
    {
        private Camera _playerCamera;
        private CameraManager _cameraManager;
        private Transform _cameraTransform;

        private float _xRotation;

        public void Construct(CameraManager cameraManager
            , Camera playerCamera)
        {
            if (cameraManager == null)
            {
                throw new ArgumentNullException(nameof(cameraManager));
            }
            if (playerCamera == null)
            {
                throw new ArgumentNullException(nameof(playerCamera));
            }

            _cameraManager = cameraManager;
            _playerCamera = playerCamera;

            _cameraTransform = playerCamera.gameObject.GetComponent<Transform>();
        }

        private void Update()
        {
            if(_playerCamera != null && _playerCamera.enabled)
            {
                RotateCamVertical();
            }
        }

        private void RotateCamVertical()
        {
            _xRotation -= Input.GetAxis("Mouse Y") * Time.deltaTime
                * _cameraManager.mouseSensitivity;
            // Don't rotate beyond looking straight up or down
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _cameraTransform.localRotation
                = Quaternion.Euler(_xRotation, 0f, 0f);
        }
    }
}