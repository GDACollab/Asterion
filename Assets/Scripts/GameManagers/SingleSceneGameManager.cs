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

    private FirstPersonUIManager _firstPersonUIManager;

    private void Awake()
    {
        _firstPersonUIManager = FindObjectOfType<FirstPersonUIManager>();
        _firstPersonUIManager.Construct();

        _playerManager = FindObjectOfType<PlayerManager>();
        _playerManager.Construct(_firstPersonUIManager);

        _spacefighterGameManager = FindObjectOfType<SpacefighterGameManager>();
        _spacefighterGameManager.Construct(_playerManager, _playerManager.cameraManager);
    }
}
