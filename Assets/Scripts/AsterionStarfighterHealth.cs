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
        public override void TakeDamage(int damage)
        {
            if (asterionManager.isVictory == false)
            {
                Debug.Log("asterion ship take damage");
                base.TakeDamage(damage);
            }
        }
        public override void Death()
        {
            asterionManager.GameConcluded(false);
        }
    }

}
