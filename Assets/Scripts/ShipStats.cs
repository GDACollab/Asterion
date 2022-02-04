using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStats : MonoBehaviour
{
    public static ShipStats instance;

    public int thruster;
    public int attack;
    public int shield;
    public int range;

    // Start is called before the first frame update
    // keep start or awake?
    void Start()
    {
        thruster = 0;
        attack = 0;
        shield = 0;
        range = 0;
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

    public void ResetAllStats()
    {
        thruster = 0;
        attack = 0;
        shield = 0;
        range = 0;
    }
}
