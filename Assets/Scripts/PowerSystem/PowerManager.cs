using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manager for the power system used in the game
public class PowerManager : MonoBehaviour
{
    // Paramaters for the in-game power meter
    [Header("Power Meter")]
    public Slider powerMeter;
    public const int numSegments = 10;

    // Paramaters to control power depletion
    [Header("Depletion Stats")]
    public const double maxPowerLevel = 100.0;
    // Measured in percentage of maximum power per 60 seconds
    public const double initialRate = 4.0;
    public const double rateMultiplier = 1.5;
    // Percentage of maximum power regained following a win
    public const double winRate = 10.0;

    // Private values used to modify the power during runtime
    private double barLength;
    private double currentRate;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
