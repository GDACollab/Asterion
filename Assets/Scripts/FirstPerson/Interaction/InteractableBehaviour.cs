using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FirstPersonPlayer;

namespace Interactable
{
    public abstract class InteractableBehaviour : MonoBehaviour
    {
        [SerializeField] protected InteractableManager _interactableManager;

        public void Construct(CameraManager cameraManager)
        {
            _interactableManager.Construct(cameraManager, this);
        }

        public abstract void InteractAction();

        public abstract void StopInteractAction();
    }
}

