using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AsterionLossScreen : MonoBehaviour
{

    public TextMeshProUGUI insufficientFundsText;
    public TextMeshProUGUI continueButtonText;
    public TextMeshProUGUI gameStateText;
    public GameObject resetButton;
    public GameObject exitButton;
    // Time in seconds
    public float buttonDelay;

    void OnEnable()
    {
        // Delay activation of buttons
        resetButton.SetActive(false);
        exitButton.SetActive(false);
        Invoke("DelayButtons", buttonDelay);
    }

    void DelayButtons()
    {
        resetButton.SetActive(true);
        exitButton.SetActive(true);
    }
}
