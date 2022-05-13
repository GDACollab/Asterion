using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AsterionArcade
{
    public class fighter_enemy_health : BasicDamageable
    {

        [Header("SFX References")]
        [SerializeField] FMODUnity.EventReference alienDeathSFX;

        public override void Start()
        {
            base.Start();
            isAlien = true;
        }

        public override void Death()
        {
            // Play the SFX that plays when the alien ship fucking explodes
            FMODUnity.RuntimeManager.PlayOneShot(alienDeathSFX.Guid);
            
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
