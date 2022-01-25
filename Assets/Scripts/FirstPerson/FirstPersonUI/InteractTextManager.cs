using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FirstPersonPlayer
{
    public class InteractTextManager : MonoBehaviour
    {
        private TextMeshProUGUI _interactText;
        private Transform _rootTransform;
        private GameObject _textGameObject;

        private bool textEnabled = false;

        public void Construct()
        {
            _interactText = GetComponentInChildren<TextMeshProUGUI>();
            _textGameObject = _interactText.gameObject;
            _rootTransform = GetComponent<Transform>();

            SetTextEnable(textEnabled);
        }

        public void SetTextEnable(bool enable)
        {
            textEnabled = enable;
            _textGameObject.SetActive(textEnabled);
        }

        public void SetTextString(string newText)
        {
            _interactText.text = newText;
        }
    }
}