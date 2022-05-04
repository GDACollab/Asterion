using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] List<GameObject> upgradeButtons;

    [SerializeField] private TextMeshProUGUI upgradePointText;

    public ShipStats stats;

    public int upgradePoints;

    

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
        upgradePointText.text = "Available Points: " + upgradePoints;
        ShipStats.instance.UpdateUpgradeDisplayUI();
    }

    

    public void UpdateButtonVisibility()
    {
        if(upgradePoints > 0)
        {
            foreach(GameObject g in upgradeButtons)
            {
                g.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject g in upgradeButtons)
            {
                g.SetActive(false);
            }
        }
    }

    public void UpgradeShield()
    {
        
        if (upgradePoints > 0)
        {
            stats.shield += 1;
            upgradePoints--;
            ShipStats.instance.modifiedStats[0]++;
            UpdateValues();
            UpdateButtonVisibility();
        }

        

    }


    public void DowngradeShield()
    {
        if (stats.shield > 0)
        {
            stats.shield -= 1;
            upgradePoints++;
            ShipStats.instance.modifiedStats[0]--;
            UpdateValues();
            UpdateButtonVisibility();
        }
        
    }

    public void UpgradeGuns()
    {
        if (upgradePoints > 0)
        {
            stats.attack += 1;
            upgradePoints--;
            ShipStats.instance.modifiedStats[1]++;
            UpdateValues();
            UpdateButtonVisibility();
        }
        
    }

    public void DowngradeGuns()
    {
        if (stats.attack > 0)
        {
            stats.attack -= 1;
            upgradePoints++;
            ShipStats.instance.modifiedStats[1]--;
            UpdateValues();
            UpdateButtonVisibility();
        }
        
    }

    public void UpgradeThrusters()
    {
        if (upgradePoints > 0)
        {
            stats.thruster += 1;
            ShipStats.instance.modifiedStats[2]++;
            upgradePoints--;
            UpdateValues();
            UpdateButtonVisibility();
        }
        
    }

    public void DowngradeThrusters()
    {
        if (stats.thruster > 0)
        {
            stats.thruster -= 1;
            upgradePoints++;
            ShipStats.instance.modifiedStats[2]--;
            UpdateValues();
            UpdateButtonVisibility();
        }
        
    }

    public void UpgradeRange()
    {
        if (stats.range < 4 && upgradePoints > 0)
        {
            stats.range += 1;
            upgradePoints--;
            ShipStats.instance.modifiedStats[3]++;
            UpdateValues();
            UpdateButtonVisibility();
        }
        
    }

    public void DowngradeRange()
    {
        if (stats.range > 0)
        {
            stats.range -= 1;
            upgradePoints++;
            ShipStats.instance.modifiedStats[3]--;
            UpdateValues();
            UpdateButtonVisibility();
        }
        
    }
}
