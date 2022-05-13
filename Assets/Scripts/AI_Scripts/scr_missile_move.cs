using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Missile heat seeking movement.
 *
 * Developer: Jonah Ryan
 * SFX Implementation: Dylan Mahler
 */


public class scr_missile_move : MonoBehaviour
{
    public Vector3 playerPos;
    public float bulletSpeed;
    private Rigidbody2D m_Rigidbody;
    private SpriteRenderer sprite_rend;
    private int rotations = 3;
    public GameObject player;
    private bool StartCountdown = false;
    private bool flashA = false;

    [Header("SFX References")]
    [SerializeField] FMODUnity.EventReference missileDestroyedSFX;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        sprite_rend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StarshipBullet")
        {
            // Play the SFX that plays when the missile is shot down
            FMODUnity.RuntimeManager.PlayOneShot(missileDestroyedSFX.Guid);

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dir = (Vector2)playerPos - m_Rigidbody.position;
        m_Rigidbody.MovePosition(Vector2.MoveTowards(m_Rigidbody.position, playerPos, Time.deltaTime * bulletSpeed));
        m_Rigidbody.rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (rotations == 2)
        {
            sprite_rend.color = Color.white;
        }

        if (Vector2.Distance(playerPos, (Vector2)m_Rigidbody.position) < 1f && Vector2.Distance(playerPos, (Vector2)m_Rigidbody.position) != 0f)
        {
            playerPos = player.transform.position;
            rotations -= 1;
        }
    }
}
