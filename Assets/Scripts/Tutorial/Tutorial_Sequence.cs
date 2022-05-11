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

    


    public bool hasSeenTony = true;

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

        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(false);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(false);

        yield return new WaitForSeconds(0.05f);
        Lights.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        Lights.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        Lights.SetActive(false);

        GameObject.Find("SpookyPlaneTutorial").GetComponent<SpriteRenderer>().enabled = true;
        //GameObject.Find("SpookyPlane").transform.rotation = new Quaternion(81f, -90f, 90f, 0f);

        yield return new WaitForSeconds(2f);
        GameObject.Find("SpookyPlaneTutorial").GetComponent<SpriteRenderer>().enabled = false;
        Lights.SetActive(true);

        yield return new WaitForSeconds(0.05f);
        Lights.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        Lights.SetActive(true);
        yield return new WaitForSeconds(1f);
     
       
        
        _cameraManager.GetComponent<CameraManager>().OnChangeCameraState
          .Invoke(CameraManager.CameraState.TutorialCutscene);

        yield return new WaitForSeconds(1f);

        GameObject.Find("POWER MANAGER").GetComponent<PowerManager>().powerLevel -= 10;
        yield return new WaitForSeconds(1f);
        GameObject.Find("POWER MANAGER").GetComponent<PowerManager>().powerLevel -= 10;
        yield return new WaitForSeconds(0.5f);

        _cameraManager.GetComponent<CameraManager>().OnChangeCameraState
          .Invoke(CameraManager.CameraState.FirstPerson);

        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(true);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(true);

        this.enabled = false;
    }

    // Handles Event When Leaving Astramori
    IEnumerator EventThree()
    {
        yield return new WaitForSeconds(1f);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(false);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(false);
        AstramoriGame.GetComponent<AstramoriManager>().ForceDoorOpen();

        //GameObject.Find("EmergencyLight (1)").GetComponent<Light>().color = Color.green;

        StartCoroutine(EndEventThree());
    }

    IEnumerator EndEventThree()
    {
        // Delay Should Be Length of Sound
        yield return new WaitForSeconds(2f);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(true);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(true);

        GameObject.Find("AsterionTutorialEndTrigger").GetComponent<BoxCollider>().enabled = true;
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
        yield return new WaitForSeconds(1f);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(false);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(false);
        AsterionGame.GetComponent<AsterionArcade.AsterionManager>().ForceDoorOpen();

        if (GameManager.Instance.asterionGamesPlayed == 1)
        {
            StartCoroutine(GameManager.Instance.powerManager.asterionLighting.WarningLightsRoutine());

        }

        StartCoroutine(EndEventOne());

    }

    IEnumerator EndEventOne()
    {
        // Delay Should Be Length of Sound
        yield return new WaitForSeconds(2f); 
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(true);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(true);
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

