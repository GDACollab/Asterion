using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsterionArcade
{


    public class AstramoriStarfighterHealth : BasicDamageable
    {
        [SerializeField] AstramoriManager astramoriManager;

        public override void Start()
        {
            base.Start();
            isAlien = false;
        }
        public override void Death()
        {
            astramoriManager.GameConcluded(true);
        }
    }

}
