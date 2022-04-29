using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using FirstPersonPlayer;
using AsterionArcade;

public class Tutorial_Sequence : MonoBehaviour
{

    GameObject AsterionGame;
    GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        AsterionGame = GameObject.Find("AsterionGame");
        Player = GameObject.Find("FirstPersonPlayer");
        AsterionGame.GetComponent<AsterionManager>().InteractAction();
    }

    public void LockPlayerAndSlideDoor()
    {
        StartCoroutine(EventOne());
    }

    //  Work Around as something else renables player movement
    IEnumerator EventOne()
    {
        yield return new WaitForSeconds(1f);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(false);
        //Play Sliding Door SFX
        StartCoroutine(EndEventOne());

    }
    IEnumerator EndEventOne()
    {
        yield return new WaitForSeconds(6f);
        Player.GetComponent<FirstPersonPlayer.PlayerMovement>().SetMovementEnabled(true);
    }

  
}

