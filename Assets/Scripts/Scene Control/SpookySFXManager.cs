using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookySFXManager : MonoBehaviour
{
    [Tooltip("Base delay, in seconds, between instances of a spooky SFX playing.")]
    public float baseDelay = 20.0f;
    [Tooltip("Variance in delay between spooky SFX. We add a random number in the range (-variance, variance) to our base delay to get the final delay.")]
    public float delayVariance = 5f;
    private float time;

    [Header("Environmental SFX")]
    public List<FMODUnity.EventReference> environmentalSFX;

    [Header("Mechanical SFX")]
    public List<FMODUnity.EventReference> mechanicalSFX;

    [Header("Alien SFX")]
    public List<FMODUnity.EventReference> alienSFX;
    List<FMODUnity.EventReference> soundbankToPlay;

    [Header("Speakers")]
    [SerializeField] PlayerRoomDetection playerRoomDetection;
    [SerializeField] List<GameObject> asterionRoomLightSpeakers;    // Used as locations to play SFX; the light source "speaker", two other lights,
                                                                    // and the emergency light.
    [SerializeField] List<GameObject> astramoriRoomLightSpeakers;   // Used as locations to play SFX. Same deal as above, different room.
    [SerializeField] List<GameObject> catwalkRoomCabinetSpeakers;   // Used as locations to play SFX; 4 arbitrary arcade cabinets.
    private GameObject currentSpeaker;
    
    public SanityManager sanityManager;
    private int[,,] sanityStageProbabilities = new int[8,3,2]
    {
        // So let's explain what the fuck is going on here:
        // For the sake of playing spooky SFX, sanity drain is quantized
        // into 8 stages, separated into 7 intervals of 100%/7 and 1 at 0%.
        
        // For each sanity stage, there are ranges for three categories:
        // one for Environmental SFX, one for Mechanical SFX, and one for Alien SFX.

        // Ultimately, to play a random SFX we'll be randomly generating a number
        // between 0 and 100, and choosing the category depending on which range it
        // falls into.

        // For example:
        //      The sanity is between 6/7 * 100 and 5/7 * 100.
        //      If the RNG lands between 0 and 66, we play an environmental sound.
        //      If the RNG lands between 67 and 100, we play a mechanical sound.
        // etc!

        // A range of (-1,-1) means that category will never be chosen at that stage.

        //  env.        mech.       alien       // Range:
        {   {0,99},     {-1,-1},    {-1,-1}},   // Sanity is [100      , 6/7*100)
        {   {0,66},     {67,99},    {-1,-1}},   // Sanity is [6/7*100  , 5/7*100)
        {   {0,33},     {34,99},    {-1,-1}},   // Sanity is [5/7*100  , 4/7*100)
        {   {-1,-1},    {0,99},     {-1,-1}},   // Sanity is [4/7*100  , 3/7*100)
        {   {-1,-1},    {0,66},     {67,99}},   // Sanity is [3/7*100  , 2/7*100)
        {   {-1,-1},    {0,33},     {34,99}},   // Sanity is [2/7*100  , 1/7*100)
        {   {-1,-1},    {-1,-1},    {0,99}},    // Sanity is [1/7*100  , 0)
        {   {-1,-1},    {0,49},     {50,99}}    // Sanity is 0
    };
    private int probabilityStage;
    private int RNG;

    void Awake()
    {
        sanityManager = GetComponent<SanityManager>();
        StartCoroutine(PlaySoundsCoroutine());
    }
    
    protected IEnumerator PlaySoundsCoroutine()
    {
        while (true)
        {
            // STRETCH GOAL: HAVE EITHER baseDelay or delayVariance change with sanity, so that sounds happen more often
            // closer towards 0 sanity.
            time = baseDelay + Random.Range(-delayVariance, delayVariance);
            yield return new WaitForSeconds(time);
            PlaySpookySFX();
        }

    }

    void PlaySpookySFX()
    {
                                        // Get the probability ranges based of the sanity stage.
        probabilityStage = calculateStageFromSanity(sanityManager.sanity);

        RNG = Random.Range(0,100);  // Generate a random int between 0 and 99.

                                        // For each of our three soundbanks...
        for (int i=0; i<=2; i++)
        {
                                        // If our RNG is within the soundbank's range:
            if (sanityStageProbabilities[probabilityStage,i,0] <= RNG
                & RNG <= sanityStageProbabilities[probabilityStage,i,1])
            {
                
                switch (i)              
                {                       // Set soundbankToPlay to the appropriate bank...
                    case 0:
                        soundbankToPlay = environmentalSFX;
                        break;
                    case 1:
                        soundbankToPlay = mechanicalSFX;
                        break;
                    case 2:
                        soundbankToPlay = alienSFX;
                        break;
                }

            }
            
            //print(playerRoomDetection.playerLocation);

                                        // Pick a location to play the sound...
            switch (playerRoomDetection.playerLocation)
            {
                case PlayerRoomDetection.Location.AsterionRoom:
                    RNG = Random.Range(0, asterionRoomLightSpeakers.Count);
                    currentSpeaker = asterionRoomLightSpeakers[RNG];
                    //print("Player in Asterion");
                    break;


                case PlayerRoomDetection.Location.Walkway:
                                        // We shouldn't play environmentalSFX in the catwalk.
                                        // Play an mechanical one instead, if we're at or above stage 4,
                                        // or play an alien one if we're below.
                    if (soundbankToPlay == environmentalSFX)
                    {
                        if (probabilityStage <= 4){ soundbankToPlay = mechanicalSFX; }
                        else { soundbankToPlay = alienSFX; }
                    }
                    RNG = Random.Range(0, catwalkRoomCabinetSpeakers.Count);
                    currentSpeaker = catwalkRoomCabinetSpeakers[RNG];
                    //print("Player on catwalk");
                    break;


                case PlayerRoomDetection.Location.AstramoriRoom:
                    RNG = Random.Range(0, astramoriRoomLightSpeakers.Count);
                    currentSpeaker = astramoriRoomLightSpeakers[RNG];
                    //print("Player in Astramori");
                    break;
            }

                                        // And play a random sound effect from that bank at the location.
            RNG = Random.Range(0,soundbankToPlay.Count);
            FMODUnity.RuntimeManager.PlayOneShotAttached(soundbankToPlay[RNG].Guid, currentSpeaker);
            print(soundbankToPlay[RNG] + "\nplayed at " + playerRoomDetection.playerLocation);

            break;

        }

        
    }

    int calculateStageFromSanity(float sanity)
    {
        // HELPER FUNCTION FOR READABILITY

        // Sanity percent           Stage
        // [100      , 6/7*100)     0
        // [6/7*100  , 5/7*100)     1
        // [5/7*100  , 4/7*100)     2
        // [4/7*100  , 3/7*100)     3
        // [3/7*100  , 2/7*100)     4
        // [2/7*100  , 1/7*100)     5
        // [1/7*100  , 0)           6
        // 0                        7

        return (7 - (int)Mathf.Ceil(sanity * 7f/100f));
    }
}
