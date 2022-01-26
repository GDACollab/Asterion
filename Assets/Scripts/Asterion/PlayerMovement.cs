using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsterionArcade
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D rb;

        // General parameters for the player
        // Base speeds
        public float baseMoveSpeed = 5;
        public float baseMaxSpeed = 5;

        // Starting health
        public int health = 1;

        // Base gun damage
        public int baseDamage = 1;

        // Initial bullet damage
        public int fireDamage = 1;

        /// Power up levels
        // Shields
        public int shieldLevel;
        // Guns
        public int gunLevel;
        // Thrusters
        public int thrusterLevel;
        // Range
        public int rangeLevel;

        // Variables for how much power ups change the player
        // How much extra health each shield provides
        public int shieldHealth = 1;

        // How much extra damage every gun upgrade provides
        public int gunPower = 1;

        // How much each thruster level will increase the movement speed
        public float thrusterIncrease = 0.25f;

        // Varaibles to store data after upgrade levels have been set
        // Actual health of player after shields have been added
        private int hitPoints;

        // Actual amout of damage the player does to enemies
        private int damagePoints;

        // Actual speeds after the thrusters have been accounted for
        private float moveSpeed;
        private float maxSpeed;

        public Vector2 playerVelocity;
        public Vector3 mousePos;

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

            // Initialization of local upgrade variables
            hitPoints = health + (shieldHealth * shieldLevel);
            damagePoints = baseDamage + (gunPower * gunLevel);
            moveSpeed = baseMoveSpeed * (1 + (thrusterIncrease * thrusterLevel));
            maxSpeed = baseMoveSpeed * (1 + (thrusterIncrease * thrusterLevel));
        }

        // Update is called once per frame
        void Update()
        {
            if(inputEnabled)
            {
                // Debug
                moveSpeed = baseMoveSpeed * (1 + (0.25f * thrusterLevel));
                maxSpeed = baseMaxSpeed * (1 + (0.25f * thrusterLevel));
                // End debug
                
                MovementCheck();
                ShootCheck();
            }
        }

        void ShootCheck()
        {
        
        }

        void MovementCheck()
        {
            mousePos = Input.mousePosition;
            mousePos.x -= Screen.width / 2;
            mousePos.y -= Screen.height / 2;

            transform.localEulerAngles = new Vector3(0, 0, (Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg) - 90);


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
            setPlayerMovement();

            rb.AddForce(playerVelocity);

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

        }
        public void NonRelativeMovement()
        {
            setPlayerMovement();

            rb.velocity = playerVelocity;

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }

        // Sets the velocity of the player
        private void setPlayerMovement() {
            // Horizonal movement
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                if(!Input.GetKey(KeyCode.A)) {
                    // Right
                    playerVelocity.x = moveSpeed;
                } else if (!Input.GetKey(KeyCode.D)) {
                    // Left
                    playerVelocity.x = -moveSpeed;
                } else {
                    // Both are pressed
                    playerVelocity.x = 0;
                }
            } else {
                playerVelocity.x = 0;
            }

            // Vertical movement
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) {
                if(!Input.GetKey(KeyCode.W)) {
                    // Down
                    playerVelocity.y = -moveSpeed;
                } else if (!Input.GetKey(KeyCode.S)) {
                    // Up
                    playerVelocity.y = moveSpeed;
                } else {
                    // Both are pressed
                    playerVelocity.y = 0;
                }
            } else {
                playerVelocity.y = 0;
            }
        }
    }
}