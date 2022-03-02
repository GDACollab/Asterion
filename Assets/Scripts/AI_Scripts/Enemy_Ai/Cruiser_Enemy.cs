using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AsterionArcade
{
    public class Cruiser_Enemy : Enemy
    {

        public override void enemyShoot()
        {
            Vector2 playerPosFrig = player.transform.position;

            // Finds KnownPlayerPos of Frigate, if none, defaults to current player position
            GameObject findFrigateEnemy = GameObject.Find("obj_missile_frigate");
            if(findFrigateEnemy != null)
            {
                playerPosFrig = findFrigateEnemy.GetComponent<Frigate_Enemy>().getKnownPlayerPos();
            }
            Vector2 playerPosReal = player.transform.position;

                GameObject bulletCreated;
                bulletCreated = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
                bulletCreated.GetComponent<Rigidbody2D>().velocity = (playerPosReal - (Vector2)transform.position).normalized * bulletSpeed;
                bulletCreated.transform.eulerAngles = (playerPosReal - (Vector2)transform.position).normalized;
                if (isAstramori)
                {
                    bulletCreated.transform.parent = GameManager.Instance.astramoriEnemyBullets;
                }
                Destroy(bulletCreated, 5f);

                GameObject bulletFrigate;
                bulletFrigate = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
                bulletFrigate.GetComponent<Rigidbody2D>().velocity = (playerPosFrig - (Vector2)transform.position).normalized * bulletSpeed;
                if (isAstramori)
                {
                    bulletFrigate.transform.parent = GameManager.Instance.astramoriEnemyBullets;
                }
                bulletCreated.transform.eulerAngles = (playerPosFrig - (Vector2)transform.position).normalized;
                Destroy(bulletFrigate, 5f);

                GameObject bulletOppFrig;
                bulletOppFrig = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
                Vector2 cruiFrigPosDif = playerPosFrig - playerPosReal;
                Vector2 cruiFrigPosFlip = new Vector2(cruiFrigPosDif.y, cruiFrigPosDif.x);
                bulletOppFrig.GetComponent<Rigidbody2D>().velocity = (playerPosReal + cruiFrigPosFlip - (Vector2)transform.position).normalized * bulletSpeed;
                if (isAstramori)
                {
                    bulletOppFrig.transform.parent = GameManager.Instance.astramoriEnemyBullets;
                }
                bulletCreated.transform.eulerAngles = (playerPosReal + cruiFrigPosFlip - (Vector2)transform.position).normalized;
                Destroy(bulletOppFrig, 5f);

                readyToShoot = false;

                StartCoroutine(reload());
            
        }

    }
}