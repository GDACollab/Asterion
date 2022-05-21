using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookySFXManager : MonoBehaviour
{
    [Header("Environmental SFX")]
    public List<FMODUnity.EventReference> environmentalSFX;

    [Header("Mechanical SFX")]
    public List<FMODUnity.EventReference> mechanicalSFX;

    [Header("Alien SFX")]
    public List<FMODUnity.EventReference> alienSFX;


    public SanityManager sanityManager;
    private ((int,int), (int,int), (int,int))[] batteryStageProbabilities =
    {
        // So let's explain what the fuck is going on here:
        // For the sake of playing spooky SFX, battery drain is quantized
        // into 8 stages, separated into 7 intervals of 100%/7 and 1 at 0%.
        
        // For each battery stage, there are ranges for three categories:
        // one for Environmental SFX, one for Mechanical SFX, and one for Alien SFX.

        // Ultimately, to play a random SFX we'll be randomly generating a number
        // between 0 and 100, and choosing the category depending on which range it
        // falls into.

        // For example:
        //      The battery is between 6/7 * 100 and 5/7 * 100.
        //      If the RNG lands between 0 and 66, we play an environmental sound.
        //      If the RNG lands between 67 and 100, we play a mechanical sound.
        // etc!

        // A range of (-1,-1) means that category will never be chosen at that stage.

        //  env.        mech.       alien       // Range:
        (   (0,100),    (-1,-1),    (-1,-1)),   // Battery is [100      , 6/7*100)
        (   (0,66),     (67,100),   (-1,-1)),   // Battery is [6/7*100  , 5/7*100)
        (   (0,33),     (34,100),   (-1,-1)),   // Battery is [5/7*100  , 4/7*100)
        (   (-1,-1),    (0,100),    (-1,-1)),   // Battery is [4/7*100  , 3/7*100)
        (   (-1,-1),    (0,66),     (67,100)),  // Battery is [3/7*100  , 2/7*100)
        (   (-1,-1),    (0,33),     (34,100)),  // Battery is [2/7*100  , 1/7*100)
        (   (-1,-1),    (-1,-1),    (0,100)),   // Battery is [1/7*100  , 0)
        (   (-1,-1),    (-1,-1),    (-1,-1)),   // Battery is 0
    };

    // Start is called before the first frame update
    void Start()
    {
        sanityManager = GetComponent<SanityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(calculateStageFromBattery(sanityManager.sanity));
    }

    void PlaySpookySFX()
    {

    }

    int calculateStageFromBattery(float battery)
    {
        // HELPER FUNCTION FOR READABILITY

        //  Battery percent         Stage
        // [100      , 6/7*100)     0
        // [6/7*100  , 5/7*100)     1
        // [5/7*100  , 4/7*100)     2
        // [4/7*100  , 3/7*100)     3
        // [3/7*100  , 2/7*100)     4
        // [2/7*100  , 1/7*100)     5
        // [1/7*100  , 0)           6
        // 0                        7

        return (7 - (int)Mathf.Ceil(battery * 7f/100f));
    }
}
