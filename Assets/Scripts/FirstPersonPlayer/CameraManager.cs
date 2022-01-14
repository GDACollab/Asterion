using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Spacefighter;
using Utility;

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

        private enum CameraState
        {
            FirstPerson,
            Asterion,
            Astramori
        }

        private CameraState currentCameraState;

        public float mouseSensitivity
        {
            private set { mouseSensitivity = _mouseSensitivity; }
            get { return _mouseSensitivity; }
        }

        public void Construct()
        {
            Cursor.lockState = CursorLockMode.Locked;
            currentCameraState = CameraState.FirstPerson;

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

            if (currentCameraState != CameraState.Asterion
                && Input.GetKeyDown(KeyCode.E))
            {
                SwitchCameraState(CameraState.Asterion);
            }

            if (currentCameraState != CameraState.FirstPerson
                && Input.GetKeyDown(KeyCode.F))
            {
                SwitchCameraState(CameraState.FirstPerson);
            }
        }

        
        private void SwitchCameraState(CameraState state)
        {
            // Switch statements bad becuase poor scalibility
            // but fine if only ever size 3?
            switch (state)
            {
                case CameraState.FirstPerson:
                    StartCoroutine(SetFirstPersonVC());
                    break;
                case CameraState.Asterion:
                    StartCoroutine(SetAsterionVC());
                    break;
                case CameraState.Astramori:
                    Debug.LogError("Astramori CurrentCameraState no supported");
                    break;
            }
        }

        private IEnumerator SetFirstPersonVC()
        {
            _testArcadePlayer.ToggleCharacterEnable();
            ToggleOrthographic(false);
            ToggleCursorLock();
            _cameraStateAnimator.Play("FirstPerson");

            float duration = _cameraStateAnimator
                .GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(duration);

            currentCameraState = CameraState.FirstPerson;
        }

        private IEnumerator SetAsterionVC()
        {
            ToggleCursorLock();
            _cameraStateAnimator.Play("Arcade");
            float duration = _cameraStateAnimator
                .GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(duration);

            currentCameraState = CameraState.Asterion;
            _testArcadePlayer.ToggleCharacterEnable();
            ToggleOrthographic(true);
        }

        private void ToggleCursorLock()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        private void ToggleOrthographic(bool setOrtho)
        {
            if (setOrtho)
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

