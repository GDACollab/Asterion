using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // External
    private Rigidbody2D _rigidbody2D;

    // Exposed variables
    [SerializeField] private float bulletSpeed;

    public void Construct(Vector2 direction)
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _rigidbody2D.velocity = direction * bulletSpeed;

        StartCoroutine(BulletLifetime());
    }


    private IEnumerator BulletLifetime()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
