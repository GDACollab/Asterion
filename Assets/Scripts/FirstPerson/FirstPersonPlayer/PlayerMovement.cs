using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FirstPersonPlayer
{
    public class PlayerMovement : MonoBehaviour
    {
        // External references
        private CharacterController _characterController;
        private Transform _playerTransform;
        private CameraManager _cameraManager;

        [Header("Horizontal movement variables")]
        [Tooltip("Acceleration per second")]
        [SerializeField] private float walkAccel;
        [Tooltip("Max speed when walking")]
        [SerializeField] private float maxWalkSpeed;
        [Tooltip("Rate of deceleration if no horizontal movement input")]
        [Range(0, 1)]
        [SerializeField] private float horizontalSlowdown;
        [Tooltip("Speed threshold where player's velocity is set to 0")]
        [SerializeField] private float stopSpeed;
        [SerializeField] float gravity;


        // Internal references
        private Vector2 _inputVector;
        private float _mouseInputX;
        private Vector3 _horizontalVelocity;

        public bool _movementEnabled = false;
        public bool canRotate = true;


        // SFX stuff
        [Header("SFX & SFX Emitters")]
        [SerializeField] FMODUnity.EventReference carpetFootstepsSFX;
        private FMOD.Studio.EventInstance carpetFootstepsSFX_instance;
        [SerializeField] FMODUnity.EventReference catwalkFootstepsSFX;
        private FMOD.Studio.EventInstance catwalkFootstepsSFX_instance;
        private PlayerRoomDetection playerRoomDetection;
        private float currentFootstepDelay = 0.0f;
        private float timeBetweenStepsAugment;
        [Tooltip("Time between footstep SFX in seconds")]
        public float timeBetweenSteps = 0.5f;
        
        



        public void Construct(Transform playerTransform
            , CharacterController characterController
            , CameraManager cameraManager)
        {
            if (playerTransform == null)
            {
                throw new ArgumentNullException(nameof(playerTransform));
            }
            if (characterController == null)
            {
                throw new ArgumentNullException(nameof(characterController));
            }
            if (cameraManager == null)
            {
                throw new ArgumentNullException(nameof(cameraManager));
            }

            _playerTransform = playerTransform;
            _characterController = characterController;
            _cameraManager = cameraManager;

            SetMovementEnabled(true);
        }

        void Start()
        {
            // SFX stuff
            carpetFootstepsSFX_instance = FMODUnity.RuntimeManager.CreateInstance(carpetFootstepsSFX);
            //carpetFootstepsSFX_instance.set3DAttributes(  what is the syntax for this?  );
            catwalkFootstepsSFX_instance = FMODUnity.RuntimeManager.CreateInstance(catwalkFootstepsSFX);
            playerRoomDetection = GetComponent<PlayerRoomDetection>();

        }

        private void Update()
        {
            if (_movementEnabled)
            {
                _inputVector = new Vector2(Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical"));
                _inputVector.Normalize();

                RotatePlayer();
                
                Move();
            }

            if (!_characterController.isGrounded)
            {
                _horizontalVelocity += new Vector3(0, -gravity, 0);
            }
        }

        private void Move()
        {
            Vector3 newMovement = _playerTransform.right * _inputVector.x
                + _playerTransform.forward * _inputVector.y;

            if (newMovement.magnitude == 0)
            {
                _horizontalVelocity *= horizontalSlowdown;
            }
            else
            {
                NewHorizontalMove(newMovement, walkAccel, maxWalkSpeed);

                // Walking SFX
                
                currentFootstepDelay += Time.deltaTime;
                timeBetweenStepsAugment = (UnityEngine.Random.Range(-1,1))/10;
                if (currentFootstepDelay >= (timeBetweenSteps + timeBetweenStepsAugment))
                {    
                    currentFootstepDelay = 0;
                    // SFX
                    if (playerRoomDetection.playerLocation == PlayerRoomDetection.Location.Walkway)
                    {
                        catwalkFootstepsSFX_instance.start();
                    }
                    else carpetFootstepsSFX_instance.start();
            
                }


            }
            // Perform actual movement
            _characterController.Move(_horizontalVelocity * Time.deltaTime);
        }

        private void NewHorizontalMove(Vector3 vector, float accel, float maxSpeed)
        {
            _horizontalVelocity += vector * accel;

            if(_horizontalVelocity.magnitude > maxSpeed)
            {
                _horizontalVelocity.Normalize();
                _horizontalVelocity *= maxSpeed;
            }
            else if (_horizontalVelocity.magnitude <= stopSpeed)
            {
                _horizontalVelocity = Vector3.zero;
            }
        }

        private void RotatePlayer()
        {
            if (canRotate)
            {
                _mouseInputX = Input.GetAxis("Mouse X") * Time.deltaTime;
                _playerTransform.Rotate(Vector3.up * _mouseInputX
                    * _cameraManager.mouseSensitivity);
            }
           
        }

        public void SetMovementEnabled(bool toSet)
        {
            _movementEnabled = toSet;
            _horizontalVelocity = Vector3.zero;
        }

        public void SetTurningEnabled(bool toSet)
        {
            canRotate = toSet;
            _cameraManager.ToggleCameraRotate(toSet);
        }
    }
}