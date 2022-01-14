using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneControl;
using FirstPersonPlayer;
using Spacefighter;

public class SingleSceneGameManager : MonoBehaviour
{
    private PlayerManager _playerManager;
    private SpacefighterGameManager _spacefighterGameManager;


    private void Awake()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
        _playerManager.Construct();

        _spacefighterGameManager = FindObjectOfType<SpacefighterGameManager>();
        _spacefighterGameManager.Construct(_playerManager);
    }
}
