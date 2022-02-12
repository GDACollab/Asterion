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

    [Header("Other Shit I haven't sorted yet. Sorry :(")]
    // For the battery cells or stm
    private RawImage[] batteryCells;
    [SerializeField] float powerLevel;

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
        powerLevel -= Time.deltaTime / 60f * currentRate;
        BatteryIndicator(powerLevel);
    }

    public void GainPower()
    {
        powerLevel += winRate;
        currentRate = initialRate;
    }

    public void IncreaseRate()
    {
        currentRate *= 1.5f;
    }

    // Updates the Battery Indicator
    private void BatteryIndicator(float power)
    {
        for (int i = 0; i < numSegments; i++)
        {
            float alpha = (power - (maxPowerLevel / numSegments) * i) / 10;
            batteryCells[i].color = new Color(1, 1, 1, alpha);
        }
    }
}
