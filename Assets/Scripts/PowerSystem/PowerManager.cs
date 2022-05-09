using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [Tooltip("You DON'T wanna know what Insane Mode does.")]
    [SerializeField] bool INSANEMode = false;
    [SerializeField] float rateMultiplier = 1.5f;
    // Percentage of maximum power regained following a win
    [SerializeField] float winRate = 10.0f;

    // Private values used to modify the power during runtime
    private float barLength;
    public float currentRate;

    [Header("Other Shit I haven't sorted yet")]
    // For the battery cells or stm
    public float powerLevel;
    
    public bool isDraining = true;
    [SerializeField] GameObject tempMonster;
    Vector3 baseMonsterPos;
    [SerializeField] Color[] batteryStatusColors;
    [SerializeField] float[] batteryStatus; // Can't think of a better name :(
    [SerializeField] SanityManager sanityManager;
    private RawImage[] batteryCells;
    [SerializeField] TextMeshProUGUI batteryFPUIText;

    [Header("Tied Lighting Systems")]
    [SerializeField] public LightingGroup asterionLighting;
    [SerializeField] public LightingGroup astramoriLighting;

    [Header("SFX References")]
    [SerializeField] FMODUnity.EventReference batteryChargeSFX;
    [SerializeField] GameObject batteryCube;

    // Start is called before the first frame update
    void Awake()
    {
        powerLevel = maxPowerLevel;
        if (INSANEMode == true)
            {
            initialRate = initialRate * 20;
            }
        currentRate = initialRate;
        batteryCells = batteryIndicator.GetComponentsInChildren<RawImage>();
        numSegments = batteryCells.Length;
        baseMonsterPos = tempMonster.transform.position;
        StartCoroutine(DimRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        //sanityManager.sanity = powerLevel

        if (isDraining)
        {
            powerLevel -= Time.deltaTime / 60f * currentRate;
            batteryFPUIText.text = (int)powerLevel + "%";
        }
        
        BatteryIndicator(powerLevel);
        SetMonsterPosition();
        if(powerLevel <= 0)
        {
            isDraining = false;
            powerLevel = 0;
            sanityManager.sanity = 0;
            StartCoroutine(GameManager.Instance.LoseRoutine());
        }
    }

    private IEnumerator DimRoutine()
    {
        while (true)
        {
            asterionLighting.UpdateLights(powerLevel / 100);
            astramoriLighting.UpdateLights(powerLevel / 100);

            yield return new WaitForSeconds(2f);

            if (powerLevel < 50 && Random.Range(0, 6 * (powerLevel/50)) < 1)
            {
                StartCoroutine(asterionLighting.FlickerRoutine());
                StartCoroutine(astramoriLighting.FlickerRoutine());
            }

            yield return new WaitForSeconds(1f);


        }
        
    }

    public void SetMonsterPosition()
    {
        tempMonster.transform.position = baseMonsterPos - new Vector3(0, 0, ((100 -powerLevel) / 100) * 7);
    }

    public void GainPower()
    {
        
        powerLevel += winRate;
        FMODUnity.RuntimeManager.PlayOneShotAttached(batteryChargeSFX.Guid, batteryCube);

        if(powerLevel > 100)
        {
            powerLevel = 100;
        }
        currentRate = initialRate;
    }

    public void IncreaseRate()
    {
        currentRate *= rateMultiplier;
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
