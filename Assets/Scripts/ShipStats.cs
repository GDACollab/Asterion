using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public static ShipStats instance;
    public List<UpgradeDisplay> upgradeDisplays;


    public int thruster;
    public int attack;
    public int shield;
    public int range;

    public int oldThrust;
    public int oldAttack;
    public int oldShield;
    public int oldRange;

    // Start is called before the first frame update
    // keep start or awake?
    void Start()
    {
        thruster = 0;
        attack = 0;
        shield = 0;
        range = 0;
    }

    public void UpdateOld()
    {
        oldThrust = thruster;
        oldAttack = attack;
        oldShield = shield;
        oldRange = range;
    }

    public void DowngradeOld()
    {
        thruster = oldThrust;
        attack = oldAttack;
        shield = oldShield;
        range = oldRange;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this; 
        }

        Debug.Log(instance);

        thruster = 0;
        attack = 0;
        shield = 0;
        range = 0;
    }

    private void FixedUpdate()
    {
        UpdateUpgradeDisplayUI();
    }

    void UpdateUpgradeDisplayUI()
    {
        foreach(UpgradeDisplay upgradeDisplay in upgradeDisplays)
        {
            upgradeDisplay.upgradeTicks[0].SetTicks(shield);
            upgradeDisplay.upgradeTicks[1].SetTicks(attack);
            upgradeDisplay.upgradeTicks[2].SetTicks(thruster);
            upgradeDisplay.upgradeTicks[3].SetTicks(range);
        }
    }

    public void ResetAllStats()
    {
        thruster = 0;
        attack = 0;
        shield = 0;
        range = 0;
    }
}
