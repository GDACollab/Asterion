using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using TMPro;

namespace AsterionArcade
{

    public class Starfighter : MonoBehaviour
    {
        Rigidbody2D rb;

        [SerializeField] Vector2 randomPositionConstraints;
        [SerializeField] Transform ships;
        [SerializeField] Transform bullets;
        [SerializeField] AstramoriManager astramoriManager;
        [SerializeField] GameObject bullet;
        int layerMask;

        float reloadTime;
        public float shotDelay;
        public float shotSpeed;
        public List<Vector2> shipInfos = new List<Vector2>();

        [Header("AI Movement")]
        public float speed = 1;
        public float baseSpeed;
        public float damage;
        public float baseDamage;
        [SerializeField] float scanFrequency = 0.25f;
        [SerializeField] float distanceTresholdFactor = 1.3f;
        [SerializeField] float wallMultiplier = 1.0f;

        [Header("AI Scoring Controls")]
        [SerializeField] float angleFactor = 0.05f;
        [SerializeField] float angleExpBase = 2;
        [SerializeField] float distanceFactor = 1;
        [SerializeField] float distanceExpBase = 2;

        float time;
        bool active;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            layerMask = (LayerMask.GetMask("AsterionAI"));
        }

        void Update()
        {



            if (Input.GetKeyDown(KeyCode.Space)) RandomizePosition();
            if (Input.GetKeyDown(KeyCode.S)) ScanForClearing();
            if (Input.GetKeyDown(KeyCode.F)) active = !active;

            if (active)
            {
                time += Time.deltaTime;
                if (time > scanFrequency)
                {
                    ScanForClearing();
                    time = 0;
                }

                if (reloadTime <= 0)
                {
                    ShootClosestEnemy();
                    reloadTime = shotDelay;
                }
                else
                {
                    reloadTime -= Time.deltaTime;
                }
            }
            else rb.velocity = Vector2.zero;
        }

        void ShootClosestEnemy()
        {
            
            Transform target = null;
            if (astramoriManager.enemies.childCount >= 1)
            {
                target = astramoriManager.enemies.GetChild(0);
            }
            foreach (Transform enemy in astramoriManager.enemies)
            {
                if (Mathf.Abs((transform.position - enemy.transform.position).magnitude) < Mathf.Abs((transform.position - target.transform.position).magnitude))
                {
                    target = enemy;
                }
            }
            if (astramoriManager.enemies.childCount >= 1)
            {
                GameObject firedBullet = Instantiate(bullet, transform.position, transform.rotation);
                transform.up = (Vector2)target.transform.position - (Vector2)transform.position;
                firedBullet.GetComponent<Rigidbody2D>().velocity = ((Vector2)target.transform.position - (Vector2)transform.position).normalized * shotSpeed;
                firedBullet.transform.parent = bullets;
                Debug.Log(firedBullet.transform.position);
            }
        }


        void ScanForClearing()
        {
            // Checks all ships' angle from the starfighter and distance from starfighter

            float shortestDistance = 100f;
            RaycastHit2D[] raycastList = new RaycastHit2D[8];

            raycastList[0] = Physics2D.Raycast(transform.position, Vector2.up, 100, layerMask);
            raycastList[1] = Physics2D.Raycast(transform.position, -Vector2.up, 100, layerMask);
            raycastList[2] = Physics2D.Raycast(transform.position, Vector2.right, 100, layerMask);
            raycastList[3] = Physics2D.Raycast(transform.position, -Vector2.right, 100, layerMask);
            raycastList[4] = Physics2D.Raycast(transform.position, new Vector2(1, 1), 100, layerMask);
            raycastList[5] = Physics2D.Raycast(transform.position, new Vector2(1, -1), 100, layerMask);
            raycastList[6] = Physics2D.Raycast(transform.position, new Vector2(-1, 1), 100, layerMask);
            raycastList[7] = Physics2D.Raycast(transform.position, new Vector2(-1, -1), 100, layerMask);


            foreach (RaycastHit2D hit in raycastList)
            {
                if (hit.collider != null)
                {
                    //Debug.Log(hit.distance);

                    Vector2 difference = (new Vector3(hit.point.x, hit.point.y, 0) - transform.position) * wallMultiplier;
                    float angle = Vector2.Angle(difference.y < 0 ? Vector2.left : Vector2.right, difference) + (difference.y < 0 ? 180 : 0);
                    // ship.GetChild(0).GetComponent<TextMeshPro>().text = "" + angle;
                    shipInfos.Add(new Vector2(angle, difference.magnitude));

                    if (difference.magnitude < shortestDistance) shortestDistance = difference.magnitude;
                }
            }


            foreach (Transform ship in ships)
            {
                Vector2 difference = ship.transform.position - transform.position;
                float angle = Vector2.Angle(difference.y < 0 ? Vector2.left : Vector2.right, difference) + (difference.y < 0 ? 180 : 0);
                // ship.GetChild(0).GetComponent<TextMeshPro>().text = "" + angle;
                shipInfos.Add(new Vector2(angle, difference.magnitude));

                if (difference.magnitude < shortestDistance) shortestDistance = difference.magnitude;
            }

            foreach (Transform ship in bullets)
            {
                Vector2 difference = ship.transform.position - transform.position;
                float angle = Vector2.Angle(difference.y < 0 ? Vector2.left : Vector2.right, difference) + (difference.y < 0 ? 180 : 0);
                // ship.GetChild(0).GetComponent<TextMeshPro>().text = "" + angle;
                shipInfos.Add(new Vector2(angle, difference.magnitude));

                if (difference.magnitude < shortestDistance) shortestDistance = difference.magnitude;
            }

            // Filters out any ships within a certain distance
            float distanceTreshold = shortestDistance * distanceTresholdFactor;
            for (int i = 0; i < shipInfos.Count; i++)
            {
                if (shipInfos[i].y > distanceTreshold)
                {
                    shipInfos.RemoveAt(i);
                    i--;
                }
            }

            // Sorts ships based on angle and determine largest angle between ships to go through
            List<Vector2> sortedShipInfos = shipInfos.OrderBy(i => i.x).ToList();
            float bestScore = -1;
            float targetAngle = 0;

            for (int i = 0; i < sortedShipInfos.Count; i++)
            {
                Vector2[] shipPair = new Vector2[] { sortedShipInfos[i], sortedShipInfos[(i == 0 ? sortedShipInfos.Count : i) - 1] };
                float angleDiffence = shipPair[0].x - shipPair[1].x;
                if (angleDiffence < 0) angleDiffence += 360;

                float score = Mathf.Pow(angleExpBase, angleFactor * angleDiffence) + Mathf.Pow(distanceExpBase, distanceFactor * Mathf.Min(shipPair[0].y, shipPair[1].y));

                if (score > bestScore)
                {
                    bestScore = score;
                    targetAngle = shipPair[0].x - 0.5f * angleDiffence;
                    if (angleDiffence <= 0) targetAngle += 180;
                }
            }
            if (bestScore <= 0) targetAngle += 180;

            // Show direction of Starfighter and moves if active
            Vector3 direction = new Vector3(Mathf.Cos(targetAngle * Mathf.Deg2Rad), Mathf.Sin(targetAngle * Mathf.Deg2Rad));
            //DebugDirection(direction);
            if (active) rb.velocity = direction * speed;



            shipInfos.Clear();
        }

        void DebugDirection(Vector3 direction)
        {
            LineRenderer lr = GetComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + direction * 20f);
        }

        void RandomizePosition()
        {
            transform.position = new Vector3(Random.Range(-randomPositionConstraints.x, randomPositionConstraints.x), Random.Range(-randomPositionConstraints.y, randomPositionConstraints.y));
            ScanForClearing();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            //SceneManager.LoadScene(0);
        }

        public void SetActive()
        {
            active = true;
        }

        public void Deactivate()
        {
            active = false;
        }
    }
}