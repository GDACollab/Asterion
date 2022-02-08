using System.Collections;
using UnityEngine;

/*
 * Fighter AI shooting.
 *
 * Developer: Jonah Ryan
 */

public class scr_fighter_shoot : MonoBehaviour
{
    public GameObject bulletObject;
    public float shotDelay_Low;
    public float shotDelay_High;
    public float bulletSpeed;

    public int Ai_Type;

    protected bool readyToShoot = false;

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
            if (Vector2.Distance(playerPos,(Vector2) transform.position) < 14f && Vector2.Distance(playerPos, (Vector2)transform.position) != 0f)
            {
                GameObject bulletCreated;
                bulletCreated = GameObject.Instantiate(bulletObject, transform.position, transform.rotation);
                bulletCreated.GetComponent<Rigidbody2D>().velocity = (playerPos - (Vector2)transform.position).normalized * bulletSpeed;
                Destroy(bulletCreated, 5f);

                readyToShoot = false;

                //StartCoroutine(reload());
            }
        }
    }
    IEnumerator reload()
    {
        yield return new WaitForSeconds(Random.Range(shotDelay_Low, shotDelay_High));
        readyToShoot = true;
    }
}
