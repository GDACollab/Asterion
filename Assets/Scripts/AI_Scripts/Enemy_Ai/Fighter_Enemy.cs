using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Fighter enemy class.
 *
 * Developer: Jonah Ryan
 * SFX Iplementation: Dylan Mahler
 */

namespace AsterionArcade
{
    public class Fighter_Enemy : Enemy
    {

        [Header("SFX References")]
        [SerializeField] FMODUnity.EventReference laserShootSFX;

        public override void enemyShoot()
        {
            GameObject bulletCreated;
            Vector2 playerPos = knownPlayerPos;

            // Create Bullet

            bulletCreated = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
            bulletCreated.GetComponent<Rigidbody2D>().velocity = (playerPos - (Vector2)transform.position).normalized * bulletSpeed;
            Destroy(bulletCreated, 5f);

            // Play the funky sound effect :O
            FMODUnity.RuntimeManager.PlayOneShot(laserShootSFX.Guid);

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