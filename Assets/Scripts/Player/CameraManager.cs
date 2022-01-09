using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstPersonPlayer
{
    public class CameraManager : MonoBehaviour
    {
        private Camera _playerCamera;
        private PlayerLook _playerLook;

        [Header("Mouse control")]
        public float _mouseSensitivity;
        [Header("FOV variables")]
        [SerializeField] private float _defaultFOV;

        public float mouseSensitivity
        {
            private set { mouseSensitivity = _mouseSensitivity; }
            get { return _mouseSensitivity; }
        }

        public void Construct()
        {
            Cursor.lockState = CursorLockMode.Locked;

            _playerCamera = gameObject.GetComponent<Camera>();
            _playerCamera.fieldOfView = _defaultFOV;

            _playerLook = gameObject.GetComponent<PlayerLook>();

            _playerLook.Construct(this, _playerCamera);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleCursorLock();
            }

        }

        private void ToggleCursorLock()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }
}

