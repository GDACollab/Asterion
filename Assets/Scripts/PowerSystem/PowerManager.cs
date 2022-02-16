using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manager for the power system used in the game
public class PowerManager : MonoBehaviour
{
    // Paramaters for the in-game power meter
    [Header("Power Meter")]
    public Transform batteryIndicator;
    private int numSegments = 10;

    // Paramaters to control power depletion
    [Header("Depletion Stats")]
    public const float maxPowerLevel = 100.0f;
    // Measured in percentage of maximum power per 60 seconds
    [SerializeField] float initialRate = 4.0f;
    [SerializeField] float rateMultiplier = 1.5f;
    // Percentage of maximum power regained following a win
    [SerializeField] float winRate = 10.0f;

    // Private values used to modify the power during runtime
    private float barLength;
    private float currentRate;

    [Header("Other Shit I haven't sorted yet")]
    // For the battery cells or stm
    public float powerLevel;
    public bool isDraining = true;
    [SerializeField] Color[] batteryStatusColors;
    [SerializeField] float[] batteryStatus; // Can't think of a better name :(
    private RawImage[] batteryCells;

    // Start is called before the first frame update
    void Awake()
    {
        powerLevel = maxPowerLevel;
        currentRate = initialRate;
        batteryCells = batteryIndicator.GetComponentsInChildren<RawImage>();
        numSegments = batteryCells.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDraining)
        {
            powerLevel -= Time.deltaTime / 60f * currentRate;
        }
        
        BatteryIndicator(powerLevel);
    }

    public void GainPower()
    {
        
        powerLevel += winRate;
        if(powerLevel > 100)
        {
            powerLevel = 100;
        }
        currentRate = initialRate;
    }

    public void IncreaseRate()
    {
        currentRate *= 1.5f;
    }

    // Updates the Battery Indicator
    private void BatteryIndicator(float power)
    {
        Color batteryColor = batteryStatusColors[0];
        for (int i = 0; i < batteryStatusColors.Length; i++)
        {
            if (power < batteryStatus[i]) 
            {
                batteryColor = batteryStatusColors[i];
            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < numSegments; i++)
        {
            float alpha = (power - (maxPowerLevel / numSegments) * i) / 10;
            batteryCells[i].color = new Color(batteryColor.r, batteryColor.g, batteryColor.b, alpha);
        }
    }
}
