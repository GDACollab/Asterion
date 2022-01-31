//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Text shield;

    [SerializeField]
    private Text attack;

    [SerializeField]
    private Text thruster;

    [SerializeField]
    private Text range;

    private ShipStats stats;

    void Awake()
    {
        stats = ShipStats.instance;
        UpdateValues();
    }
    
    // call when we enable the upgrade menu
    void UpdateValues()
    {
        shield.text = stats.shield.ToString();
        //attack.text = stats.attack.ToString();
    }

    public void UpgradeShield()
    {
        stats.shield += 1;
        UpdateValues();
    }
}
