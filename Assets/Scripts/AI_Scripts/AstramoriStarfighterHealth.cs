using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsterionArcade
{


    public class AstramoriStarfighterHealth : BasicDamageable
    {
        [SerializeField] AstramoriManager astramoriManager;
        [SerializeField] GameObject astramoriHealthBarFill;

        public override void Start()
        {
            base.Start();
            isAlien = false;
            UpdateHealthBar();
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            
        }

        public void UpdateHealthBar()
        {
            astramoriHealthBarFill.GetComponent<RectTransform>().localScale = new Vector3(0.9f * ((health + 0.0f) / (baseHealth + ShipStats.instance.shield)), 0.9f, 0.9f);
        }

        public override void Death()
        {
            astramoriManager.GameConcluded(true);
        }
    }

}
