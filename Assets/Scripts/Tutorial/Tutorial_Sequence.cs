using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstPersonPlayer;
using AsterionArcade;


public class Tutorial_Sequence : MonoBehaviour
{
    public static Tutorial_Sequence Instance;

    public GameObject Lights;
    GameObject AsterionGame;
    GameObject AstramoriGame;
    GameObject Player;
    MonsterManager _MonsterManager;
    GameObject _cameraManager;
    public GameObject AsterionEndTrigger;
    public List<GameObject> TonyModelSpooky;
    public GameObject TonyModelSpookyWalking;

    [SerializeField] private bool skipIntroCutscene;


    public bool hasSeenTony = true;

    [Header("SFX References & Speakers")]
    [SerializeField] FMODUnity.EventReference lightFlicker1;
    [SerializeField] FMODUnity.EventReference lightFlicker2;
    [SerializeField] FMODUnity.EventReference tonySound;
    [SerializeField] GameObject lightSpeaker;
    // We play lightFlicker1 and 2 from lightSpeaker; tonySound is played directly to the listener

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AsterionGame = GameObject.Find("AsterionGame");
        AstramoriGame = GameObject.Find("AstramoriGame");
            _cameraManager = GameObject.Find("Camera");
        Player = GameObject.Find("FirstPersonPlayer");
        _MonsterManager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();
        AsterionGame.GetComponent<AsterionManager>().InteractAction();
    }

    public void LockPlayerAndSlideDoor()
    {
        StartCoroutine(EventOne());

    }

    public void UnlockAstramori()
    {
        StartCoroutine(EventThree());
    }   

    public void EndingMonsterEvent()
    {
        StartCoroutine(EventFourStart());
    }

    IEnumerator EventFourStart()
    {
        if (!skipIntroCutscene)
        {
            Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(false);
            Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(false);
            GameManager.Instance.canPause = false;

            // lights flicker
            FMODUnity.RuntimeManager.PlayOneShotAttached(lightFlicker1.Guid, lightSpeaker);
            yield return new WaitForSeconds(0.05f);
            Lights.SetActive(false);
            yield return new WaitForSeconds(0.05f);
            Lights.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            Lights.SetActive(false);

            yield return new WaitForSeconds(0.2f);
            TonyModelSpookyWalking.SetActive(false);
            TonyModelSpooky[1].SetActive(true);
            yield return new WaitForSeconds(0.2f);
            TonyModelSpooky[1].SetActive(false);

            TonyModelSpooky[2].SetActive(true);
            yield return new WaitForSeconds(0.2f);
            TonyModelSpooky[2].SetActive(false);

            yield return new WaitForSeconds(0.2f);
            TonyModelSpooky[0].SetActive(true);
            FMODUnity.RuntimeManager.PlayOneShot(tonySound.Guid);
            //GameObject.Find("SpookyPlane").transform.rotation = new Quaternion(81f, -90f, 90f, 0f);

            yield return new WaitForSeconds(2f);
            TonyModelSpooky[0].SetActive(false);
            TonyModelSpookyWalking.SetActive(true);
            Lights.SetActive(true);

            // lights flicker again
            FMODUnity.RuntimeManager.PlayOneShotAttached(lightFlicker2.Guid, lightSpeaker);

            yield return new WaitForSeconds(0.05f);

            Lights.SetActive(false);
            yield return new WaitForSeconds(0.05f);
            Lights.SetActive(true);
            yield return new WaitForSeconds(1f);
       
            _cameraManager.GetComponent<CameraManager>().OnChangeCameraState
              .Invoke(CameraManager.CameraState.TutorialCutscene);

            yield return new WaitForSeconds(1f);
        }

        GameObject.Find("POWER MANAGER").GetComponent<PowerManager>().powerLevel -= 10;
        yield return new WaitForSeconds(1f);
        GameObject.Find("POWER MANAGER").GetComponent<PowerManager>().powerLevel -= 10;
        yield return new WaitForSeconds(1f);

        if (!skipIntroCutscene)
        {
            _cameraManager.GetComponent<CameraManager>().OnChangeCameraState
              .Invoke(CameraManager.CameraState.FirstPerson);
        }

        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(true);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(true);
        GameManager.Instance.powerManager.isDraining = true;
        GameManager.Instance.canPause = true;

        this.enabled = false;

    }

    // Handles Event When Leaving Astramori
    public IEnumerator EventThree()
    {
        yield return new WaitForSeconds(1f);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(false);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(false);
        AstramoriGame.GetComponent<AstramoriManager>().ForceDoorOpen();

        GameObject.Find("EmergencyLight (1)").GetComponent<Light>().color = Color.green;

        StartCoroutine(EndEventThree());
    }

    IEnumerator EndEventThree()
    {
        // Delay Should Be Length of Sound
        yield return new WaitForSeconds(1.5f);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(true);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(true);

        //AsterionEndTrigger.transform.GetComponent<BoxCollider>().enabled = true;
    }

    public void TonyBehindAstramori()
    {
        if (!hasSeenTony)
        {
            hasSeenTony = true;
            //StartCoroutine(EventTwo());
        }
       
    }

    //  Work Around as something else renables player movement
    IEnumerator EventOne()
    {
        if (!skipIntroCutscene)
        {
            yield return new WaitForSeconds(1f);
            Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(false);
            Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(false);
        }
        
            AsterionGame.GetComponent<AsterionArcade.AsterionManager>().ForceDoorOpen();

        if (GameManager.Instance.asterionGamesPlayed == 1)
        {
            StartCoroutine(GameManager.Instance.powerManager.asterionLighting.WarningLightsRoutine());

        }

        StartCoroutine(EndEventOne());

    }

    IEnumerator EndEventOne()
    {
        if (!skipIntroCutscene)
        {
            // Delay Should Be Length of Sound
            yield return new WaitForSeconds(2f);
            Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(false);
            Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(false);
            yield return new WaitForSeconds(0f);
        }
        
            GameObject.Find("GameManagerObject").GetComponent<Tutorial_Sequence>().EndingMonsterEvent();
            //Destroy(gameObject);
    }

    IEnumerator EventTwo()
    {
        yield return new WaitForSeconds(1f);
        _MonsterManager.SpawnMonsterBehindAstramoriMachine();
        StartCoroutine(EndEventOne());

    }
    IEnumerator EndEventTwo()
    {

        StartCoroutine(_MonsterManager.DestroyMonstersRoutine(2));
        yield return new WaitForSeconds(1f);
    }

}

