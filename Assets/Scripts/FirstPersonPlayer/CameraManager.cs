using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Spacefighter;
using Utility;
using Interactable;

namespace FirstPersonPlayer
{
    public class CameraManager : MonoBehaviour
    {
        private InteractListManager _interactListManager;
        private PlayerManager _playerManager;

        private Camera _playerCamera;
        [SerializeField] private CinemachineVirtualCamera _firstPersonVC;
        [SerializeField] private TestArcadePlayer _testArcadePlayer;
        private PlayerLook _playerLook;

        [SerializeField] private Animator _cameraStateAnimator;

        [Header("Mouse control")]
        [SerializeField] private float _mouseSensitivity;
        [SerializeField] private LayerMask _interactableLayerMask;
        [SerializeField] private float _interactRange;

        // TODO refactor interact and camera state change to child classes

        public enum CameraState
        {
            FirstPerson,
            Asterion,
            Astramori
        }

        public CameraState currentCameraState;

        public float mouseSensitivity
        {
            private set { mouseSensitivity = _mouseSensitivity; }
            get { return _mouseSensitivity; }
        }

        public UnityEvent<CameraState> OnChangeCameraState;

        public void Construct(PlayerManager playerManager, InteractListManager interactListManager)
        {
            _playerManager = playerManager;
            _interactListManager = interactListManager;

            Cursor.lockState = CursorLockMode.Locked;
            currentCameraState = CameraState.FirstPerson;

            _playerCamera = GetComponent<Camera>();
            _playerLook = GetComponent<PlayerLook>();

            _playerLook.Construct(this, _firstPersonVC);

            OnChangeCameraState.AddListener(OnChangeCameraStateCallback);
        }

        private void Update()
        {
            MouseInteract();
        }

        private void MouseInteract()
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position
                ,transform.TransformDirection(Vector3.forward), out hit
                , _interactRange, _interactableLayerMask))
            {
                InteractableManager thisInteractable;
                hit.transform.gameObject.TryGetComponent(out thisInteractable);

                if(thisInteractable != null)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        thisInteractable.OnInteract.Invoke();
                    }
                }
            }
        }

        private void OnChangeCameraStateCallback(CameraState state)
        {
            SwitchCameraState(state);
        }

        private void SwitchCameraState(CameraState state)
        {
            // Switch statements bad becuase poor scalibility
            // but fine if only ever size 3?

            if(state != CameraState.FirstPerson)
            {
                _playerManager.playerMovement.SetMovementEnabled(false);
            }

            switch (state)
            {
                case CameraState.FirstPerson:
                    StartCoroutine(SetFirstPersonVC());
                    break;
                case CameraState.Asterion:
                    StartCoroutine(SetAsterionVC());
                    break;
                case CameraState.Astramori:
                    Debug.LogError("Astramori CurrentCameraState not supported");
                    break;
            }
        }

        private IEnumerator SetFirstPersonVC()
        {
            _testArcadePlayer.ToggleCharacterEnable(false);
            ToggleOrthographic(false);
            ToggleCursorLock();
            _cameraStateAnimator.Play("FirstPerson");

            float duration = _cameraStateAnimator
                .GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(duration);

            _playerManager.playerMovement.SetMovementEnabled(true);
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
            _testArcadePlayer.ToggleCharacterEnable(true);
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

