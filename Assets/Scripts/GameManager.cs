using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneControl;
using FirstPersonPlayer;

public class GameManager : MonoBehaviour
{
    private PlayerManager _playerManager;
    private SceneManager _sceneManager;

    private void Awake()
    {
        _sceneManager = FindObjectOfType<SceneManager>();
        _sceneManager.Construct();

        _sceneManager.OnLoadSceneGroupComplete.AddListener(() =>
        {
            _playerManager = FindObjectOfType<PlayerManager>();
            _playerManager.Construct();
        });
    }
}
