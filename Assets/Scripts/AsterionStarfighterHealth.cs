using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsterionArcade
{


    public class AsterionStarfighterHealth : BasicDamageable
    {
        [SerializeField] AsterionManager asterionManager;

        public override void Start()
        {
            base.Start();
            isAlien = false;
        }
        public override void Death()
        {
            asterionManager.GameConcluded(false);
        }
    }

}
