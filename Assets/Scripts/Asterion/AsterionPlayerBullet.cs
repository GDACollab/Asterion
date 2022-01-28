using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsterionArcade
{
    public class AsterionPlayerBullet : MonoBehaviour
    {
        public float lifespan;
        public Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
            Destroy(this.gameObject, lifespan);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

        }
    }
}
