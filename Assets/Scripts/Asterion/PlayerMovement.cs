using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsterionArcade
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D rb;
        public GameObject playerBulletPrefab;
        public Transform playerBarrel;
        [Header("Player Stats")]
        public float moveSpeed;
        public float maxSpeed;

        [Header("Movement")]
        public Vector2 playerVelocity;
        public Vector3 mousePos;

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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var newBullet = Instantiate(playerBulletPrefab, playerBarrel.position, Quaternion.identity);
                newBullet.GetComponent<AsterionPlayerBullet>().rb.AddForce(transform.up * 5, ForceMode2D.Impulse);
            }
        }

        //checks and performs movement based on mouse position + keyboard inputs
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