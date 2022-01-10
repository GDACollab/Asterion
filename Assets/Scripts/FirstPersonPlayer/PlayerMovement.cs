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

        // Internal references
        private Vector2 _inputVector;
        private float _mouseInputX;
        private Vector3 _horizontalVelocity;

        private bool _movementEnabled = false;

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

            _movementEnabled = true;
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
            _mouseInputX = Input.GetAxis("Mouse X") * Time.deltaTime;
            _playerTransform.Rotate(Vector3.up * _mouseInputX
                * _cameraManager.mouseSensitivity);
        }
    }
}