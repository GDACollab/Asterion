using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public float sanity;
    public int sanityStage;
    [SerializeField] float sanityRate;
    [SerializeField] int sanityStage1Sanity;
    [SerializeField] int sanityStage2Sanity;
    [SerializeField] Volume volume;
    [SerializeField] Vignette vignette;

    // Start is called before the first frame update
    void Start()
    {
        if (volume.sharedProfile.TryGet<Vignette>(out var vig))
        {
            vignette = vig;
            vignette.intensity.overrideState = true;
            vignette.intensity.Override(0);
        }

        
        sanity = 100f;
        sanityStage = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(sanity > 0)
        {
            sanity -= sanityRate;
        }
        
        if(sanity < sanityStage1Sanity && sanityStage == 0)
        {
            sanityStage = 1;
            StartCoroutine(SanityStage1());
        }

        if (sanity < sanityStage2Sanity && sanityStage == 1)
        {
            sanityStage = 2;
            StartCoroutine(SanityStage2());
        }

    }

    public void UpdateSanity(float val)
    {
        sanity += val;
        if(sanity < 0)
        {
            sanity = 0;
        }
    }



    IEnumerator SanityStage1()
    {
        LeanTween.value(gameObject, vignette.intensity.value, 0.4f, 5f).setOnUpdate((float val) => {
            vignette.intensity.overrideState = true;
            vignette.intensity.Override(val);
        });

        yield return new WaitForSeconds(5);

        yield return null;
    }


    IEnumerator SanityStage2()
    {
        LeanTween.value(gameObject, vignette.intensity.value, 0.55f, 5f).setOnUpdate((float val) => {
            vignette.intensity.overrideState = true;
            vignette.intensity.Override(val);
        });

        yield return new WaitForSeconds(5);

        yield return null;
    }


}
