using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsterionArcade
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D rb;
        public GameObject playerBulletPrefab;
        [SerializeField] GameObject playerBarrel;
        [Header("Base Player Stats")]
        public float baseSpeed;
        public float baseMaxSpeed;
        public int baseDamage;
        [Header("Player Stats")]
        public float moveSpeed;
        public float maxSpeed;
        public float projectileSpeed;
        public int damage;
        public float shootCooldown;
        float currentCooldown;

        [Header("Player Movement")]
        public Vector2 playerVelocity;
        public Vector3 mousePos;
        public Vector3 playerPos;
        public Camera gameCamera;
        public LineRenderer testLine;

        [Header("Player State")]
        public bool inputEnabled = false;

        private void OnEnable()
        {
            inputEnabled = true;
        }

        private void OnDisable()
        {
            playerVelocity = Vector2.zero;
            inputEnabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            if(!rb)
            {
                rb = GetComponent<Rigidbody2D>();
            }

        }

        // Update is called once per frame
        void Update()
        {
            if(inputEnabled)
            {
                MovementCheck();
                ShootCheck();
            }
        }

        void ShootCheck()
        {
            if(currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Mouse0) && currentCooldown <= 0)
            {
                var bullet = Instantiate(playerBulletPrefab, playerBarrel.transform.position, Quaternion.identity);
                bullet.GetComponent<AsterionPlayerBullet>().rb.AddForce(transform.up * projectileSpeed, ForceMode2D.Impulse);
                currentCooldown = shootCooldown;
            }
        }

        void MovementCheck()
        {
            mousePos = Input.mousePosition;
            mousePos.x /= Screen.width;
            mousePos.x -= 0.5f;
            mousePos.y /= Screen.height;
            mousePos.y -= 0.5f;

            playerPos = gameCamera.WorldToViewportPoint(transform.position);
            playerPos.x -= 0.5f;
            playerPos.y -= 0.5f;

            // Calculates the distance between the mouse point and the player
            Vector3 difference = mousePos - playerPos;
            difference.Normalize();

            // Rotates the ship to face the mouse
            float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, rotation - 90);
            //transform.localEulerAngles = new Vector3(0, 0, (Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg) - 90);


            NonRelativeForceMovement();
        


            /*
        
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity += (Vector2)(-transform.right * moveSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity += (Vector2)(transform.right * moveSpeed);
            }

            if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) || (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
            {
                playerVelocity.x = 0;
            }

            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity += (Vector2)(transform.up * moveSpeed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.velocity += (Vector2)(-transform.up * moveSpeed);
            }

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) || (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)))
            {
                playerVelocity.y = 0;
            }

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            */

            //rb.AddForce((mousePos.normalized) * (playerVelocity.normalized) * moveSpeed);
        }

        public void NonRelativeForceMovement()
        {
            if (Input.GetKey(KeyCode.A))
            {
                playerVelocity.x = -moveSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                playerVelocity.x = moveSpeed;
            }

            if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) || (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
            {
                playerVelocity.x = 0;
            }

            if (Input.GetKey(KeyCode.W))
            {
                playerVelocity.y = moveSpeed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                playerVelocity.y = -moveSpeed;
            }

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) || (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)))
            {
                playerVelocity.y = 0;
            }

            rb.AddForce(playerVelocity);

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

        }
        public void NonRelativeMovement()
        {
            if (Input.GetKey(KeyCode.A))
            {
                playerVelocity.x = -moveSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                playerVelocity.x = moveSpeed;
            }

            if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) || (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
            {
                playerVelocity.x = 0;
            }

            if (Input.GetKey(KeyCode.W))
            {
                playerVelocity.y = moveSpeed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                playerVelocity.y = -moveSpeed;
            }

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) || (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)))
            {
                playerVelocity.y = 0;
            }


            rb.velocity = playerVelocity;





            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }


        }
    }
}