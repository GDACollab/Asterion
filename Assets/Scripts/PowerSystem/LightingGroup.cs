using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingGroup : MonoBehaviour
{

    [SerializeField] private List<Light> lights;
    [SerializeField] private Light emergencyLight;
    [SerializeField] private Door tiedDoor;
    public float currentBrightness;
    public float maxBrightness;
    public float minBrightness;
    public float flickerInterval;
    private float test;
    public bool flickering;
    public Doorframe tiedDoorframe;
    [ColorUsageAttribute(true, true)]
    public Color ogColor;
    [ColorUsageAttribute(true, true)]
    public Color flashColor;

    [SerializeField] private float updateTime;

    public void SetAllLightsToDefault()
    {

        currentBrightness = maxBrightness;

    }

    public void SetAllLightsToCurrent()
    {
        foreach (Light l in lights)
        {
            l.intensity = currentBrightness;
        }

    }

    public IEnumerator WarningLightsRoutine()
    {

        while (tiedDoor.doorOpen)
        {
            LeanTween.value(gameObject, 0, 15, 0.4f).setOnUpdate((float val) =>
            {
                emergencyLight.intensity = val;
                tiedDoorframe.UpdateColor(flashColor);

            });
            yield return new WaitForSeconds(0.4f);
            LeanTween.value(gameObject, 15, 0, 0.4f).setOnUpdate((float val) =>
            {
                emergencyLight.intensity = val;
                tiedDoorframe.UpdateColor(ogColor);

            });
            yield return new WaitForSeconds(0.4f);

            yield return new WaitForSeconds(0.2f);
        }

        tiedDoorframe.UpdateColor(ogColor);





    }

    public void UpdateLights(float powerPercentage)
    {

        if (flickering)
        {
            return;
        }

        float newBrightness = ((maxBrightness - minBrightness) * (powerPercentage)) + minBrightness;

        LeanTween.value(gameObject, currentBrightness, newBrightness, updateTime).setOnUpdate((float val) =>
        {
            foreach (Light l in lights)
            {

                l.intensity = val;

            }

        });

        currentBrightness = newBrightness;



    }



    public IEnumerator FlickerRoutine()
    {
        if (!flickering)
        {
            flickering = true;

            yield return new WaitForSeconds(1.5f);

            foreach (Light l in lights)
            {

                if (Random.Range(0, 2) == 1)
                {
                    l.intensity = minBrightness;
                }

            }
            yield return new WaitForSeconds(flickerInterval);

            foreach (Light l in lights)
            {
                l.intensity = currentBrightness;
            }

            yield return new WaitForSeconds(flickerInterval);

            foreach (Light l in lights)
            {

                if (Random.Range(0, 2) == 1)
                {
                    l.intensity = minBrightness;
                }

            }
            yield return new WaitForSeconds(flickerInterval);

            foreach (Light l in lights)
            {
                l.intensity = currentBrightness;
            }

            yield return new WaitForSeconds(flickerInterval);

            foreach (Light l in lights)
            {

                if (Random.Range(0, 2) == 1)
                {
                    l.intensity = minBrightness;
                }

            }
            yield return new WaitForSeconds(flickerInterval);

            foreach (Light l in lights)
            {
                l.intensity = currentBrightness;
            }

            flickering = false;
        }


    }

   

}
