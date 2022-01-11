using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstPersonPlayer;

namespace Spacefighter
{
    public class SpacefighterGameManager : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private TestArcadePlayer _testArcadePlayer;

        public void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;

            _testArcadePlayer = GetComponentInChildren<TestArcadePlayer>();
            _testArcadePlayer.Construct();
        }
    }
}