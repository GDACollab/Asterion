using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstPersonPlayer;
using AsterionArcade;

public class Tutorial_Sequence : MonoBehaviour
{
    public static Tutorial_Sequence Instance;
    GameObject AsterionGame;
    GameObject Player;
    MonsterManager _MonsterManager;

    public bool hasSeenTony = false;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AsterionGame = GameObject.Find("AsterionGame");
        Player = GameObject.Find("FirstPersonPlayer");
        _MonsterManager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();
        AsterionGame.GetComponent<AsterionManager>().InteractAction();
    }

    public void LockPlayerAndSlideDoor()
    {
        StartCoroutine(EventOne());
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

