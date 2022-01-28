using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace FirstPersonPlayer
{
    public class PlayerLook : MonoBehaviour
    {
        private CinemachineVirtualCamera _firstPersonVC;
        private CameraManager _cameraManager;
        private Transform _cameraTransform;
        public bool _rotateEnabled;

        private float _xRotation;
        private float _yRotation;

        public void Construct(CameraManager cameraManager
            , CinemachineVirtualCamera firstPersonVC)
        {
            if (cameraManager == null)
            {
                throw new ArgumentNullException(nameof(cameraManager));
            }
            if (firstPersonVC == null)
            {
                throw new ArgumentNullException(nameof(firstPersonVC));
            }

            _cameraManager = cameraManager;
            _firstPersonVC = firstPersonVC;

            _cameraTransform = _firstPersonVC.gameObject.GetComponent<Transform>();
        }

        private void Update()
        {
            if(_rotateEnabled && _firstPersonVC != null && _firstPersonVC.enabled)
            {
                RotateCam();
            }
        }

        private void RotateCam()
        {
            _yRotation += Input.GetAxis("Mouse X") * Time.deltaTime
                * _cameraManager.mouseSensitivity;

            _xRotation -= Input.GetAxis("Mouse Y") * Time.deltaTime
                * _cameraManager.mouseSensitivity;
            // Don't rotate beyond looking straight up or down
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _cameraTransform.localRotation
                = Quaternion.Euler(_xRotation, _yRotation, 0f);
        }
    }
}