using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using TMPro;

public class Starfighter : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] Vector2 randomPositionConstraints;
    [SerializeField] Transform ships;

    [Header("AI Movement")]
    [SerializeField] float speed = 1;
    [SerializeField] float scanFrequency = 0.25f;
    [SerializeField] float distanceTresholdFactor = 1.3f;

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
        }
        else rb.velocity = Vector2.zero;
    }

    void ScanForClearing()
    {
        // Checks all ships' angle from the starfighter and distance from starfighter
        List<Vector2> shipInfos = new List<Vector2>();
        float shortestDistance = 100f;
        foreach (Transform ship in ships)
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
        DebugDirection(direction);
        if (active) rb.velocity = direction * speed;
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
}
