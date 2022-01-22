using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class InteractListManager : MonoBehaviour
    {
        private List<InteractableManager> _interactableManagerList
            = new List<InteractableManager>();

        public void Construct()
        {
            InteractableManager[] interactionManagerArray
                = FindObjectsOfType<InteractableManager>();

            foreach (InteractableManager interactable in interactionManagerArray)
            {
                _interactableManagerList.Add(interactable);
            }
        }

        public void SetShowTextAll(bool show)
        {
            foreach(InteractableManager interactable in _interactableManagerList)
            {
                interactable.OnShowText.Invoke(show);
            }
        }
    }
}