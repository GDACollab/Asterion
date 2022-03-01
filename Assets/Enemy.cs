using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base Class for all enemy AI.
 *
 * Developer: Jonah Ryan
 */

namespace AsterionArcade
{
    public class Enemy : MonoBehaviour
    {

        // Inspector Vars
        public int Ai_Type;
        public bool isAstramori;

        [Header("Movement")]
        public float movementSpeed;
        [Tooltip("How fast the enemy turns towards the new FP. (0 is instant)")]
        public float turnSpeed;

        [Header("Bullet Stats")]
        public GameObject bulletPrefab;
        public float bulletSpeed;

        [Header("Shooting Stats")]
        public float reloadSpeed;
        public float weaponRange;
        [HideInInspector]
        public bool lookingForPlayer;

        // Private Vars

        private GameObject player;
        private Rigidbody2D rigidBody;
        protected bool readyToShoot;


        // Start is called before the first frame update
        void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            StartCoroutine(reload());
        }

        void FixedUpdate()
        {
            if (lookingForPlayer) { enemyMovement(); }

            if (readyToShoot && checkDistanceToPlayer())
            {
                enemyShoot();
            }
        }

        // All enemies will handle shooting differently 
        public virtual void enemyShoot() { }

        private void enemyMovement()
        {

            Vector2 playerPos = scr_find_player.Get_Player_Pos(Ai_Type);
            Vector2 dir = playerPos - rigidBody.position;

            // Move and rotate towards player
            rigidBody.MovePosition(Vector2.MoveTowards(rigidBody.position, playerPos, Time.deltaTime * movementSpeed));

            if (Ai_Type == 0)
            {
                rigidBody.rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            }
            else
            {
                rigidBody.rotation = Mathf.Lerp(rigidBody.rotation, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, turnSpeed);
            }
        }

        // Checks if the player is in range
        private bool checkDistanceToPlayer()
        {
            Vector2 playerPos = scr_find_player.Get_Player_Pos(Ai_Type);
            Vector2 currentPos = transform.position;
            float distanceToPlayer = Vector2.Distance(playerPos, currentPos);

            // Only shoot when nearby player and not at players position.
            return (distanceToPlayer < weaponRange) && (Vector2.Distance(playerPos, currentPos) != 0f);
        }

        // Handle Enemy collisions
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Damage player and destory alien shit on collision with player
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<BasicDamageable>().TakeDamage(1);
                Destroy(this.gameObject);
            }

            // Ignore boundry
            if (collision.gameObject.tag == "Ai_GameBoundry")
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            }

            // Enemies will only collide with their cousins 
            if (collision.gameObject.tag == "AlienShip" && collision.gameObject.name != gameObject.name)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            }
        }

        protected IEnumerator reload()
        {
            yield return new WaitForSeconds(reloadSpeed);
            readyToShoot = true;
        }

    }
}