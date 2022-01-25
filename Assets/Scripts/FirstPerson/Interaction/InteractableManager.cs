using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

using FirstPersonPlayer;

namespace Interactable
{
    public class InteractableManager : MonoBehaviour
    {
        private CameraManager _cameraManager;
        private InteractableBehaviour _interactable;
        private TextMeshPro _textMeshPro;
        [SerializeField] private string _interactText;
        public string interactText
        {
            get { return _interactText; }
            private set { interactText = _interactText; }
        }

        public UnityEvent OnInteract;
        public UnityEvent OnStopInteract;
        public UnityEvent<bool> OnShowText;

        public void Construct(CameraManager cameraManager, InteractableBehaviour interactable)
        {
            _cameraManager = cameraManager;
            _textMeshPro = GetComponentInChildren<TextMeshPro>();
            _interactable = interactable;

            _textMeshPro.enabled = false;

            OnInteract.AddListener(OnInteractCallback);
            OnShowText.AddListener(OnShowTextCallback);
            OnStopInteract.AddListener(OnStopInteractCallback);
        }

        // Temporary easy 3D text toggling
        //private void OnMouseOver()
        //{
        //    OnShowText.Invoke(true);
        //}

        //private void OnMouseExit()
        //{
        //    OnShowText.Invoke(false);
        //}

        private void OnInteractCallback()
        {
            _interactable.InteractAction();
        }

        private void OnStopInteractCallback()
        {
            _interactable.StopInteractAction();
        }

        private void OnShowTextCallback(bool showing)
        {
            _textMeshPro.enabled = showing;
        }
    }
}