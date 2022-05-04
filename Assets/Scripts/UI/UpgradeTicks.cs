using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeTicks : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indicatorText;


    public void SetText(string text)
    {
        indicatorText.text = text;
    }

}
