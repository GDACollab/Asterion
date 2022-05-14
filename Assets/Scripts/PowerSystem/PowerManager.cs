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
    [SerializeField] FMODUnity.EventReference batteryDrainSFX;
    [SerializeField] GameObject batteryCube;
    [SerializeField] FMODUnity.EventReference lightsOffSFX;
    [SerializeField] GameObject asterionSpotlightSpeaker;
    [SerializeField] GameObject astramoriSpotlightSpeaker;
    private bool playedLightsOffSFX;

    private IEnumerator dimRoutine;
    private IEnumerator asterionFlicker;
    private IEnumerator astramoriFlicker;

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

        playedLightsOffSFX = false;
        dimRoutine = DimRoutine();
        StartCoroutine(dimRoutine);
        StartCoroutine(BatteryDrainSFXRoutine());
        asterionFlicker = asterionLighting.FlickerRoutine();
        astramoriFlicker = astramoriLighting.FlickerRoutine();
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
        if(powerLevel <= 0 && isDraining)
        {
            isDraining = false;
            powerLevel = 0;
            sanityManager.sanity = 0;

            if (!playedLightsOffSFX)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached(lightsOffSFX.Guid, asterionSpotlightSpeaker);
                FMODUnity.RuntimeManager.PlayOneShotAttached(lightsOffSFX.Guid, astramoriSpotlightSpeaker);
                playedLightsOffSFX = true;
            }

            StartCoroutine(GameManager.Instance.LoseRoutine());
            StopCoroutine(dimRoutine);
            StopCoroutine(asterionFlicker);
            StopCoroutine(astramoriFlicker);
        }
    }

    private IEnumerator DimRoutine()
    {
        while (true)
        {
            if (powerLevel > 0)
            {
                asterionLighting.UpdateLights(powerLevel / 100);
                astramoriLighting.UpdateLights(powerLevel / 100);
            }

            yield return new WaitForSeconds(2f);

            if (powerLevel < 50 && powerLevel > 0 && Random.Range(0, 6 * (powerLevel/50)) < 1)
            {
                Debug.Log("flickering");
                asterionFlicker = asterionLighting.FlickerRoutine();
                astramoriFlicker = astramoriLighting.FlickerRoutine();
                StartCoroutine(asterionFlicker);
                StartCoroutine(astramoriFlicker);
            }

            yield return new WaitForSeconds(1f);


        }
        
    }

    private IEnumerator BatteryDrainSFXRoutine()
    {
        for (int i = 9; i >= 1; i--)
        {
            float threshold = ((i*10) + 0.5f);
            while (powerLevel > threshold)
            {
                yield return null;
            }
            FMODUnity.RuntimeManager.PlayOneShotAttached(batteryDrainSFX.Guid, batteryCube);
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
