using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AsterionArcade
{
    public class fighter_enemy_health : BasicDamageable
    {

        [SerializeField] DissolveEffect dissolveFX;
        [SerializeField] Color dissolveColor;
        Collider2D bodyCollider;

        [Header("SFX References")]
        [SerializeField] FMODUnity.EventReference alienDeathSFX;

        public void Awake()
        {
            if (GetComponent<DissolveEffect>() != null)
            {

                dissolveFX = GetComponent<DissolveEffect>();
            }
            
        }
        public override void Start()
        {
            base.Start();
            isAlien = true;
            bodyCollider = GetComponent<Collider2D>();
            if(dissolveFX != null)
            {
                
                dissolveFX.SetEmpty();
                dissolveFX.StopDissolve(2f, dissolveColor);
            }
            
        }

        public override void Death()
        {
            // Play the SFX that plays when the alien ship fucking explodes
            FMODUnity.RuntimeManager.PlayOneShot(alienDeathSFX.Guid);

            

            if (dissolveFX != null)
            {
                dissolveFX.StartDissolve(2f, dissolveColor);
            }

            bodyCollider.enabled = false;
            if(GetComponent<Enemy>() != null)
            {
                GetComponent<Enemy>().enabled = false;
            }
            base.Death();
            Destroy(this.gameObject,1);
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
