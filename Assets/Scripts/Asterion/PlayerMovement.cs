using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsterionArcade
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D rb;
        public GameObject playerBulletPrefab;
        public FakeCursor asterionFakeCursor;
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

        [Header("Player State")]
        public bool inputEnabled = false;

        [Header("SFX Events")]
        [SerializeField] FMODUnity.EventReference spaceshipShootSFX; // SFX reference for shooting bullets




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
                bullet.transform.parent = GameManager.Instance.asterionEnemyBullets;
                bullet.GetComponent<BasicBullet>().damage = damage;
                currentCooldown = shootCooldown;

                FMODUnity.RuntimeManager.PlayOneShot(spaceshipShootSFX);

            }
        }

        void MovementCheck()
        {
            mousePos = Input.mousePosition;
            mousePos.x -= Screen.width / 2;
            mousePos.y -= Screen.height / 2;

            //turns ship towards position of asterion's fake cursor
            Vector2 testMousePos = (asterionFakeCursor.transform.GetComponent<RectTransform>().transform.position);
            transform.right = (Vector2)transform.position - testMousePos;
            transform.localEulerAngles += new Vector3(0, 0, 90);


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
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                playerVelocity.x = -moveSpeed;
            }

            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                playerVelocity.x = moveSpeed;
            }

            if (((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))) || (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)))
            {
                playerVelocity.x = 0;
            }

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
            {
                playerVelocity.y = moveSpeed;
            }

            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                playerVelocity.y = -moveSpeed;
            }

            if (((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))) || (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)))
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