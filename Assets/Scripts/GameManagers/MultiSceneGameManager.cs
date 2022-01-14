using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneControl;
using FirstPersonPlayer;
using Spacefighter;

public class MultiSceneGameManager : MonoBehaviour
{
    private PlayerManager _playerManager;
    private SceneManager _sceneManager;
    private SpacefighterGameManager _spacefighterGameManager;


    private void Awake()
    {
        _sceneManager = FindObjectOfType<SceneManager>();
        _sceneManager.Construct();

        _sceneManager.OnLoadSceneGroupComplete.AddListener(() =>
        {
            _playerManager = FindObjectOfType<PlayerManager>();
            _playerManager.Construct();

            _spacefighterGameManager = FindObjectOfType<SpacefighterGameManager>();
            _spacefighterGameManager.Construct(_playerManager);
        });
    }
}
