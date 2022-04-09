using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Fighter enemy class.
 *
 * Developer: Jonah Ryan
 */

namespace AsterionArcade
{
    public class Fighter_Enemy : Enemy
    {
        public override void enemyShoot()
        {
            GameObject bulletCreated;
            Vector2 playerPos = knownPlayerPos;

            // Create Bullet

            bulletCreated = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
            bulletCreated.GetComponent<Rigidbody2D>().velocity = (playerPos - (Vector2)transform.position).normalized * bulletSpeed;
            Destroy(bulletCreated, 5f);

            if (isAstramori)
            {
                bulletCreated.transform.parent = GameManager.Instance.astramoriEnemyBullets;
            }

            // Post Shot Updates

            readyToShoot = false;
            StartCoroutine(reload());
        }
    }
}