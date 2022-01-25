using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Interactable;

namespace FirstPersonPlayer
{
    public class PlayerManager : MonoBehaviour
    {
        public FirstPersonUIManager firstPersonUIManager { get; private set; }
        public Transform playerTransform { get; private set; }
        public CharacterController _characterController { get; private set; }
        public PlayerMovement playerMovement { get; private set; }
        public CameraManager cameraManager { get; private set; }
        public InteractListManager _interactListManager { get; private set; }

        public void Construct(FirstPersonUIManager firstPersonUIManager)
        {
            this.firstPersonUIManager = firstPersonUIManager;
            playerTransform = GetComponent<Transform>();

            _characterController = GetComponent<CharacterController>();

            _interactListManager = FindObjectOfType<InteractListManager>();
            _interactListManager.Construct();

            cameraManager = GetComponentInChildren<CameraManager>();
            cameraManager.Construct(this, _interactListManager);

            playerMovement = GetComponent<PlayerMovement>();
            playerMovement.Construct(playerTransform
                , _characterController, cameraManager);
        }
    }
}