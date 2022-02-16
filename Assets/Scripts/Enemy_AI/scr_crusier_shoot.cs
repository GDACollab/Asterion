using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Missile Cruiser AI shooting.
 *
 * Developer: Jonah Ryan
 */

public class scr_crusier_shoot : MonoBehaviour
{
    public GameObject bulletObject;
    public float shotDelay_Low;
    public float shotDelay_High;
    public float bulletSpeed;

    public int Ai_Type;

    private int ammo = 3;
    private bool readyToShoot = false;

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
        yield return new WaitForSeconds(1f);

        if (ammo > 0)
        {
            GameObject bulletCreated;
            bulletCreated = GameObject.Instantiate(bulletObject, transform.position, transform.rotation);
            bulletCreated.GetComponent<scr_missile_move>().playerPos = playerPos;
            //bulletCreated.GetComponent<Rigidbody2D>().velocity = (playerPos - (Vector2)transform.position).normalized * bulletSpeed;
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
