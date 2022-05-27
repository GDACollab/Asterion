using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AsterionArcade
{
    public class fighter_enemy_health : BasicDamageable
    {
        public float batteryAmount;
        [SerializeField] DissolveEffect dissolveFX;
        [SerializeField] Color dissolveColor;
        bool doBattery = true;
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

            if(GetComponent<Enemy>() != null)
            {
                if (!GetComponent<Enemy>().isAstramori)
                {
                    doBattery = true;
                }
                else
                {
                    doBattery = false;
                }
            }
            
        }

        public override void Death()
        {
            // Play the SFX that plays when the alien ship explodes IF we haven't lost the whole game by battery hitting 0%.
            if (!GameManager.Instance.gameLost){ FMODUnity.RuntimeManager.PlayOneShot(alienDeathSFX.Guid); }

            

            if (dissolveFX != null)
            {
                dissolveFX.StartDissolve(2f, dissolveColor);
            }

            bodyCollider.enabled = false;
            if(GetComponent<Enemy>() != null)
            {
                GetComponent<Enemy>().enabled = false;
            }

            if (isAlien)
            {
                GameManager.Instance.asterionManager.batteryEarned += batteryAmount;
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
