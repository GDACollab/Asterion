using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AsterionArcade
{
    public class scr_cruiser_shoot : scr_fighter_shoot
    {

        void Start()
        {
            StartCoroutine(reload());
        }

        private void FixedUpdate()
        {
            if (readyToShoot)
            {
                //Vector2 playerPosCrui = scr_find_player.Get_Player_Pos(Ai_Type);
                Vector2 playerPosFrig = scr_find_player.Get_Player_Pos(1);
                Vector2 playerPosReal = scr_find_player.Get_Player_Pos(0);

                // Only shoot when nearby player and not at players position.
                if (Vector2.Distance(playerPosReal, (Vector2)transform.position) < 14f && Vector2.Distance(playerPosReal, (Vector2)transform.position) != 0f)
                {
                    GameObject bulletCreated;
                    bulletCreated = GameObject.Instantiate(bulletObject, transform.position, transform.rotation);
                    bulletCreated.GetComponent<Rigidbody2D>().velocity = (playerPosReal - (Vector2)transform.position).normalized * bulletSpeed;
                    if (isAstramori)
                    {
                        bulletCreated.transform.parent = GameManager.Instance.astramoriEnemyBullets;
                    }
                    Destroy(bulletCreated, 5f);

                    GameObject bulletFrigate;
                    bulletFrigate = GameObject.Instantiate(bulletObject, transform.position, transform.rotation);
                    bulletFrigate.GetComponent<Rigidbody2D>().velocity = (playerPosFrig - (Vector2)transform.position).normalized * bulletSpeed;
                    if (isAstramori)
                    {
                        bulletFrigate.transform.parent = GameManager.Instance.astramoriEnemyBullets;
                    }
                    Destroy(bulletFrigate, 5f);

                    GameObject bulletOppFrig;
                    bulletOppFrig = GameObject.Instantiate(bulletObject, transform.position, transform.rotation);
                    Vector2 cruiFrigPosDif = playerPosFrig - playerPosReal;
                    Vector2 cruiFrigPosFlip = new Vector2(cruiFrigPosDif.y, cruiFrigPosDif.x);
                    bulletOppFrig.GetComponent<Rigidbody2D>().velocity = (playerPosReal + cruiFrigPosFlip - (Vector2)transform.position).normalized * bulletSpeed;
                    if (isAstramori)
                    {
                        bulletOppFrig.transform.parent = GameManager.Instance.astramoriEnemyBullets;
                    }
                    Destroy(bulletOppFrig, 5f);

                    readyToShoot = false;

                    StartCoroutine(reload());
                }
            }
        }
        IEnumerator reload()
        {
            yield return new WaitForSeconds(Random.Range(shotDelay_Low, shotDelay_High));
            readyToShoot = true;
        }
    }
}