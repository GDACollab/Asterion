//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void Start()
    {
        stats = ShipStats.instance;
        Debug.Log(stats);
        UpdateValues();
    }
    
    // call when we enable the upgrade menu
    void UpdateValues()
    {
        shield.text = stats.shield.ToString();
        attack.text = stats.attack.ToString();
        thruster.text = stats.thruster.ToString();
        range.text = stats.range.ToString();
    }

    public void UpgradeShield()
    {
        if (stats.shield < 4)
        {
            stats.shield += 1;
        }

        UpdateValues();
    }

    public void DowngradeShield()
    {
        if (stats.shield > 0)
        {
            stats.shield -= 1;
        }
 
        UpdateValues();
    }

    public void UpgradeGuns()
    {
        if (stats.attack < 4)
        {
            stats.attack += 1;
        }

        UpdateValues();
    }

    public void DowngradeGuns()
    {
        if (stats.attack > 0)
        {
            stats.attack -= 1;
        }
 
        UpdateValues();
    }

    public void UpgradeThrusters()
    {
        if (stats.thruster < 4)
        {
            stats.thruster += 1;
        }

        UpdateValues();
    }

    public void DowngradeThrusters()
    {
        if (stats.thruster > 0)
        {
            stats.thruster -= 1;
        }

        UpdateValues();
    }

    public void UpgradeRange()
    {
        if (stats.range < 4)
        {
            stats.range += 1;
        }

        UpdateValues();
    }

    public void DowngradeRange()
    {
        if (stats.range > 0)
        {
            stats.range -= 1;
        }

        UpdateValues();
    }
}