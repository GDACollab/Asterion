using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SceneControl;
using FirstPersonPlayer;
using AsterionArcade;

public class SingleSceneGameManager : MonoBehaviour
{
    private PlayerManager _playerManager;
    private FirstPersonUIManager _firstPersonUIManager;

    private AsterionManager _asterionManager;

    private void Awake()
    {
        _firstPersonUIManager = FindObjectOfType<FirstPersonUIManager>();
        _firstPersonUIManager.Construct();

        _playerManager = FindObjectOfType<PlayerManager>();
        _playerManager.Construct(_firstPersonUIManager);

        _asterionManager = FindObjectOfType<AsterionManager>();
        _asterionManager.Construct(_playerManager.cameraManager);
    }
}
