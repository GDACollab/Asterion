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

        [Header("Movement")]
        public float movementSpeed;
        [Tooltip("How many seconds till the next FindPlayer Update. (0 is instant)")]
        public int timeTillNextFP;
        public int AIType;
        [Tooltip("How fast the enemy turns towards the new FP. (0 is instant)")]
        public float turnSpeed;


        [Header("Bullet Stats")]
        public GameObject bulletPrefab;
        public float bulletSpeed;

        [Header("Shooting Stats")]
        public float reloadSpeed;
        public float weaponRange;
        
        [HideInInspector]
        public bool isAstramori;
        [HideInInspector]
        public bool lookingForPlayer;

        // Private Vars

        protected GameObject player;
        private Rigidbody2D rigidBody;
        protected Vector2 knownPlayerPos;
        protected bool readyToShoot;

        // Start is called before the first frame update
        void Start()
        {
            player = FindClosestGameObjecctWithTag("Player");
            knownPlayerPos = player.transform.position;

            rigidBody = GetComponent<Rigidbody2D>();
            StartCoroutine(reload());

            if (timeTillNextFP != 0)
            {
                StartCoroutine(updateFP(timeTillNextFP));
            }
        }

        void FixedUpdate()
        {
            if (timeTillNextFP == 0) { knownPlayerPos = player.transform.position; }

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

            Vector2 dir = knownPlayerPos - rigidBody.position;

            // Move and rotate towards player
            rigidBody.MovePosition(Vector2.MoveTowards(rigidBody.position, knownPlayerPos, Time.deltaTime * movementSpeed));

            if (turnSpeed == 0)
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
            Vector2 currentPos = transform.position;
            float distanceToPlayer = Vector2.Distance(knownPlayerPos, knownPlayerPos);

            // Only shoot when nearby player and not at players position.
            return (distanceToPlayer < weaponRange) && (Vector2.Distance(knownPlayerPos, currentPos) != 0f);
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

        protected IEnumerator updateFP(float time)
        {
            yield return new WaitForSeconds(time);
            knownPlayerPos = player.transform.position;
            StartCoroutine(updateFP(timeTillNextFP));
        }
        private GameObject FindClosestGameObjecctWithTag(string tag)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag(tag);
            float currentCloseDist = 10000000;
            int indexOfClose = 0;

            for (int i = 0; i < players.Length; i++)
            {
                float distance = Vector2.Distance(transform.position, players[i].transform.position);
                if (distance < currentCloseDist)
                {
                    currentCloseDist = distance;
                    indexOfClose = i;
                }
            }

            return players[indexOfClose];
        }

        public Vector2 getKnownPlayerPos()
        {
            return knownPlayerPos;
        }

    }
}