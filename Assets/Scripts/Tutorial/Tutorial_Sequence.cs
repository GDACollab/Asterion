using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstPersonPlayer;
using AsterionArcade;


public class Tutorial_Sequence : MonoBehaviour
{
    public static Tutorial_Sequence Instance;
  

    GameObject AsterionGame;
    GameObject AstramoriGame;
    GameObject Player;
    MonsterManager _MonsterManager;
    GameObject _cameraManager;
    


    public bool hasSeenTony = false;

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
        yield return new WaitForSeconds(1f);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(false);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetTurningEnabled(false);
        Debug.Log("FADELIGHT");
        GameObject.Find("Lights").GetComponent<LightingGroup>().currentBrightness = 0f;


        //_interactableManager.gameObject.SetActive(false);
        _cameraManager.GetComponent<CameraManager>().OnChangeCameraState
                .Invoke(CameraManager.CameraState.TutorialCutscene);
        //GameObject.Find("Lights").GetComponent<LightingGroup>().SetAllLightsToCurrent();
        //GameObject.Find("Lights").GetComponent<LightingGroup>().enabled = false;

    }

    // Handles Event When Leaving Astramori
    IEnumerator EventThree()
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
            StartCoroutine(EventTwo());
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

