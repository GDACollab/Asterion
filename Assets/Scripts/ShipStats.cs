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

    public int[] modifiedStats = new int[4];

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

    public void ResetModified()
    {
        modifiedStats[0] = modifiedStats[1] = modifiedStats[2] = modifiedStats[3] = 0;
    }

    public void RefundContinue()
    {
        shield += modifiedStats[0];
        attack += modifiedStats[1];
        thruster += modifiedStats[2];
        range += modifiedStats[3];
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
        
    }

    public void UpdateUpgradeDisplayUI()
    {
        foreach(UpgradeDisplay upgradeDisplay in upgradeDisplays)
        {
            upgradeDisplay.upgradeTicks[0].SetText(shield+1+"");
            upgradeDisplay.upgradeTicks[1].SetText(attack + 1 + "");
            upgradeDisplay.upgradeTicks[2].SetText(thruster + 1 + "");
            upgradeDisplay.upgradeTicks[3].SetText(range + 1 + "");
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
