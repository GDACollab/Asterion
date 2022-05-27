using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsterionArcade
{


    public class AsterionStarfighterHealth : BasicDamageable
    {
        [SerializeField] AsterionManager asterionManager;
        [SerializeField] GameObject asterionHealthBarFill;

        public override void Start()
        {
            base.Start();
            isAlien = false;
            asterionHealthBarFill.transform.localScale = new Vector3(0.9f * (health / baseHealth), 0.9f, 0.9f);
        }
        public override void TakeDamage(int damage)
        {
            if (asterionManager.isVictory == false)
            {
                Debug.Log("asterion ship take damage");
                base.TakeDamage(damage);
                asterionHealthBarFill.transform.localScale = new Vector3(0.9f * (health/baseHealth),0.9f,0.9f);
            }


        }
        public override void Death()
        {
            asterionManager.GameConcluded(false);
        }
    }

}
