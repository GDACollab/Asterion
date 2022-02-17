using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Missile Cruiser AI shooting.
 *
 * Developer: Jonah Ryan
 */

namespace AsterionArcade
{
    public class scr_frigate_shoot : scr_fighter_shoot
    {

        private int ammo = 3;
 

        void Start()
        {
            StartCoroutine(reload());
        }

        private void FixedUpdate()
        {
            if (readyToShoot)
            {
                Vector2 playerPos = scr_find_player.Get_Player_Pos(Ai_Type);

                // Only shoot when nearby player and not at players position.
                if (Vector2.Distance(playerPos, (Vector2)transform.position) < 14f && Vector2.Distance(playerPos, (Vector2)transform.position) != 0f)
                {
                    ammo = 3;
                    StartCoroutine(shot(playerPos));

                    readyToShoot = false;
                }
            }
        }

        IEnumerator shot(Vector2 playerPos)
        {
            yield return new WaitForSeconds(2f);

            if (ammo > 0)
            {
                GameObject bulletCreated;
                bulletCreated = GameObject.Instantiate(bulletObject, transform.position, transform.rotation);
                playerPos = scr_find_player.Get_Player_Pos(Ai_Type);
                bulletCreated.GetComponent<scr_missile_move>().playerPos = playerPos;
                if (isAstramori)
                {
                    bulletCreated.transform.parent = GameManager.Instance.astramoriEnemyBullets;
                }

                Destroy(bulletCreated, 5f);

                ammo -= 1;
                StartCoroutine(shot(playerPos));
            }
            else
            {
                StartCoroutine(reload());
            }
        }

        IEnumerator reload()
        {
            yield return new WaitForSeconds(Random.Range(shotDelay_Low, shotDelay_High));
            readyToShoot = true;
        }
    }
}