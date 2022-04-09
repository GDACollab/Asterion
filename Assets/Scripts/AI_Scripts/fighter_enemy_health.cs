using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AsterionArcade
{
    public class fighter_enemy_health : BasicDamageable
    {
        public override void Start()
        {
            base.Start();
            isAlien = true;
        }

        public override void Death()
        {
            base.Death();
            Destroy(this.gameObject);
        }

        public override void Disable()
        {
            base.Disable();

            rb.velocity = Vector2.zero;

            if (GetComponent<Enemy>() != null)
            {
                GetComponent<Enemy>().enabled = false;
            }
        }
    }
}
