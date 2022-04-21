//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI shield;

    [SerializeField]
    private TextMeshProUGUI attack;

    [SerializeField]
    private TextMeshProUGUI thruster;

    [SerializeField]
    private TextMeshProUGUI range;

    public ShipStats stats;

    void Start()
    {
        stats = ShipStats.instance;
        Debug.Log(stats);
        UpdateValues();
    }

    private void FixedUpdate()
    {
        UpdateValues();
    }

    // call when we enable the upgrade menu
    void UpdateValues()
    {
        shield.text = (stats.shield + 1).ToString();
        attack.text = (stats.attack + 1).ToString();
        thruster.text = (stats.thruster + 1).ToString();
        range.text = (stats.range + 1).ToString();


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
