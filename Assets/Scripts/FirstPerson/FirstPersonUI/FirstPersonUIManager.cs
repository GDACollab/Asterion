using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstPersonPlayer
{
    public class FirstPersonUIManager : MonoBehaviour
    {
        public InteractTextManager interactTextManager { get; private set; }

        public void Construct()
        {
            interactTextManager = GetComponentInChildren<InteractTextManager>();
            interactTextManager.Construct();
        }
    }
}