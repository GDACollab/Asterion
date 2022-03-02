using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Missile Frigate enemy class.
 *
 * Developer: Jonah Ryan
 */

namespace AsterionArcade
{
    public class Frigate_Enemy : Enemy
    {

        private int numOfMissiles = 3;

        public override void enemyShoot()
        {
            numOfMissiles = 3;
            Vector2 playerPos = knownPlayerPos;
            StartCoroutine(shot(playerPos));

            readyToShoot = false;
        }

        IEnumerator shot(Vector2 playerPos)
        {
            yield return new WaitForSeconds(2f);

            if (numOfMissiles > 0)
            {
                GameObject bulletCreated;

                bulletCreated = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
                bulletCreated.GetComponent<scr_missile_move>().playerPos = knownPlayerPos;
                bulletCreated.GetComponent<scr_missile_move>().player = player;

                if (isAstramori)
                {
                    bulletCreated.transform.parent = GameManager.Instance.astramoriEnemyBullets;
                }

                Destroy(bulletCreated, 5f);

                // Post Shot Updates

                numOfMissiles -= 1;
                StartCoroutine(shot(playerPos));
            }
            else
            {
                StartCoroutine(reload());
            }
        }
    }
}