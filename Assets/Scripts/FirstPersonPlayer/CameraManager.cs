using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Spacefighter;

namespace FirstPersonPlayer
{
    public class CameraManager : MonoBehaviour
    {
        private Camera _playerCamera;
        [SerializeField] private CinemachineVirtualCamera _firstPersonVC;
        [SerializeField] private TestArcadePlayer _testArcadePlayer;
        private PlayerLook _playerLook;

        [SerializeField] private Animator _cameraStateAnimator;

        [Header("Mouse control")]
        [SerializeField] private float _mouseSensitivity;
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
            Cursor.visible = true;

            _playerCamera = GetComponent<Camera>();
            _playerLook = GetComponent<PlayerLook>();

            _playerLook.Construct(this, _firstPersonVC);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleCursorLock();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _testArcadePlayer.ToggleCharacterEnable();
                _cameraStateAnimator.Play("Arcade");
                ToggleOrthographic();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                _testArcadePlayer.ToggleCharacterEnable();
                ToggleOrthographic();
                _cameraStateAnimator.Play("FirstPerson");
            }

        }

        private void ToggleCursorLock()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                //Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                //Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        private void ToggleOrthographic()
        {
            bool arcadeEnabled = _testArcadePlayer.characterEnabled;

            if (arcadeEnabled)
            {
                _playerCamera.orthographic = true;
                _playerCamera.orthographicSize = 5;
            }
            else
            {
                _playerCamera.orthographic = false;
            }
        }
    }
}

