using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cinemachine;
using Utility;
using Interactable;


namespace FirstPersonPlayer
{
    public class CameraManager : MonoBehaviour
    {
        private InteractListManager _interactListManager;
        private PlayerManager _playerManager;
        private InteractTextManager _interactTextManager;

        private Camera _playerCamera;
        [SerializeField] private CinemachineVirtualCamera _firstPersonVC;
        private PlayerLook _playerLook;

        [SerializeField] private Animator _cameraStateAnimator;

        [Header("Mouse control")]
        [SerializeField] private float _mouseSensitivity;
        [SerializeField] private LayerMask _interactableLayerMask;
        [SerializeField] private float _interactRange;
        [SerializeField] private Slider sensSlider;

        // TODO refactor interact and camera state change to child classes

        public enum CameraState
        {
            FirstPerson,
            Asterion,
            Astramori,
            TutorialCutscene
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
            _interactTextManager = _playerManager
                .firstPersonUIManager.interactTextManager;

            Cursor.lockState = CursorLockMode.Locked;
            currentCameraState = CameraState.FirstPerson;

            _playerCamera = GetComponent<Camera>();
            _playerLook = GetComponent<PlayerLook>();

            _playerLook.Construct(this, _firstPersonVC);

            OnChangeCameraState.AddListener(OnChangeCameraStateCallback);
        }

        public void UpdateSens()
        {
            _mouseSensitivity = sensSlider.value;
        }

        public void ToggleCameraRotate(bool toSet)
        {
            if (toSet)
            {
                _mouseSensitivity = sensSlider.value;
            }
            else
            {
                _mouseSensitivity = 0;
            }
        }

        private void Start()
        {
            StartCoroutine(FPcameraBob());
        }

        private void Update()
        {
            MouseInteract();
        }

        public IEnumerator FPcameraBob()
        {
            while (true)
            {
                LeanTween.value(gameObject, 0.3f, 0.315f, 5f).setOnUpdate((float val) => {
                    _firstPersonVC.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0,val,0);
                });
                yield return new WaitForSeconds(7f);
                LeanTween.value(gameObject, 0.325f, 0.3f, 5f).setOnUpdate((float val) => {
                    _firstPersonVC.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, val, 0);
                });
                yield return new WaitForSeconds(7f);
            }
            
        }

        private void MouseInteract()
        {
            // Inefficient setting & getting every frame, but works
            RaycastHit hit;
            if (Physics.Raycast(transform.position
                ,transform.TransformDirection(Vector3.forward), out hit
                , _interactRange, _interactableLayerMask))
            {
                if (hit.transform.gameObject.TryGetComponent
                    (out InteractableManager thisInteractable))
                {
                    _interactTextManager.SetTextEnable(true);
                    _interactTextManager.SetTextString(thisInteractable.interactText);

                    if (Input.GetButtonDown("Fire1") && currentCameraState == CameraState.FirstPerson && !GameManager.Instance.isPaused)
                    {
                        thisInteractable.OnInteract.Invoke();
                        _interactTextManager.SetTextEnable(false);
                    }
                    return;
                }
            }
            // Not looking at an interactable
            _interactTextManager.SetTextEnable(false);
        }

        private void OnChangeCameraStateCallback(CameraState state)
        {
            SwitchCameraState(state);
        }

        private void SwitchCameraState(CameraState state)
        {
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
                    StartCoroutine(SetAstramoriVC());
                    break;
                case CameraState.TutorialCutscene:
                    StartCoroutine(SetTutorialCutscene());
                    break;
            }
        }

        private IEnumerator SetFirstPersonVC()
        {
            //ToggleOrthographic(false);
            ToggleCursorLock(true);
            _cameraStateAnimator.Play("FirstPerson");

            float duration = _cameraStateAnimator
                .GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(duration);
            _playerLook._rotateEnabled = true;
            GameManager.Instance.isPlayingArcade = false;
            GameManager.Instance.CheckPlayerIsPlayingArcadeStatus();
            _firstPersonVC.transform.rotation = _playerManager.playerTransform.rotation;
            _playerManager.playerMovement.SetMovementEnabled(true);
            currentCameraState = CameraState.FirstPerson;
        }

        private IEnumerator SetAsterionVC()
        {
            ToggleCursorLock(false);
            _playerLook._rotateEnabled = false;
            _cameraStateAnimator.Play("AsterionArcade");
            float duration = _cameraStateAnimator
                .GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(duration); 

            currentCameraState = CameraState.Asterion;
            //ToggleOrthographic(true);
        }

        private IEnumerator SetAstramoriVC()
        {
            ToggleCursorLock(false);
            _playerLook._rotateEnabled = false;
            _cameraStateAnimator.Play("AstramoriArcade");
            float duration = _cameraStateAnimator
                .GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(duration);

            currentCameraState = CameraState.Astramori;
            //ToggleOrthographic(true);
        }

        private IEnumerator SetTutorialCutscene()
        {
            ToggleCursorLock(false);
            _playerLook._rotateEnabled = false;
            _cameraStateAnimator.Play("TutorialCutscene");
            float duration = _cameraStateAnimator
                .GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(duration);

            currentCameraState = CameraState.TutorialCutscene;
            //ToggleOrthographic(true);
        }

        private void ToggleCursorLock(bool isLock)
        {
            if (isLock)
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

