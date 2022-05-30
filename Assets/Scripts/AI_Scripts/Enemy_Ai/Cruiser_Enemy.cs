using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AsterionArcade
{
    public class Cruiser_Enemy : Enemy
    {

        // Smaller numbers for a wider spread, larger for narrower
        [Range(0,2)]
        public float bulletSpread;

        private Vector2 feuxFrigateEnemyPos;
        private bool feuxFlag;
        protected static int feuxWait;

        [Header("SFX References")]
        [SerializeField] FMODUnity.EventReference laserShoot3_SFX;

        public override void enemyShoot()
        {
            // Play the shooting SFX IF the game hasn't been lost.
            if (!GameManager.Instance.gameLost){ FMODUnity.RuntimeManager.PlayOneShot(laserShoot3_SFX.Guid); }

            Vector2 playerPosFrig = feuxFrigateEnemyPos;

            // Finds KnownPlayerPos of Frigate, if none, defaults to current player position
            // GameObject findFrigateEnemy = GameObject.Find("obj_missile_frigate(Clone)");
            // if(findFrigateEnemy != null)
            // {
            //     playerPosFrig = findFrigateEnemy.GetComponent<Frigate_Enemy>().getKnownPlayerPos();
            // }
            Vector2 playerPosReal = player.transform.position;

            GameObject bulletCreated;
            bulletCreated = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
            Vector2 straightVNorm = (playerPosReal - (Vector2)transform.position).normalized;
            bulletCreated.GetComponent<Rigidbody2D>().velocity = straightVNorm * bulletSpeed;
            bulletCreated.transform.eulerAngles = straightVNorm;
            if (isAstramori)
            {
                bulletCreated.transform.parent = GameManager.Instance.astramoriEnemyBullets;
            }
            Destroy(bulletCreated, 5f);

            GameObject bulletFrigate;
            bulletFrigate = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
            Vector2 frigVNorm = (playerPosFrig - (Vector2)transform.position).normalized;
            bulletFrigate.GetComponent<Rigidbody2D>().velocity = frigVNorm * bulletSpeed;
            if (isAstramori)
            {
                bulletFrigate.transform.parent = GameManager.Instance.astramoriEnemyBullets;
            }
            bulletCreated.transform.eulerAngles = frigVNorm;
            Destroy(bulletFrigate, 5f);

            GameObject bulletOppFrig;
            bulletOppFrig = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
            float k = 2*(Vector2.Dot(straightVNorm, frigVNorm)/Vector2.Dot(frigVNorm, frigVNorm));
            Vector2 mirrorVNorm = ((k*frigVNorm) - straightVNorm).normalized;
            bulletOppFrig.GetComponent<Rigidbody2D>().velocity = mirrorVNorm * bulletSpeed;
            if (isAstramori)
            {
                bulletOppFrig.transform.parent = GameManager.Instance.astramoriEnemyBullets;
            }
            bulletCreated.transform.eulerAngles = mirrorVNorm;
            Destroy(bulletOppFrig, 5f);

            readyToShoot = false;
            
            Debug.Log("Direct   = (" + bulletCreated.GetComponent<Rigidbody2D>().velocity.x + bulletCreated.GetComponent<Rigidbody2D>().velocity.y);
            Debug.Log("Frigate  = (" + bulletFrigate.GetComponent<Rigidbody2D>().velocity.x + bulletFrigate.GetComponent<Rigidbody2D>().velocity.y);
            Debug.Log("Opposite = (" + bulletOppFrig.GetComponent<Rigidbody2D>().velocity.x + bulletOppFrig.GetComponent<Rigidbody2D>().velocity.y);
            StartCoroutine(reload());
            
        }

        protected IEnumerator feuxFrigate(float delay) 
        {
            yield return new WaitForSeconds(delay);
            feuxFrigateEnemyPos = player.transform.position;
            StartCoroutine(feuxFrigate(feuxWait));
        }

        protected override IEnumerator updateFP(float time)
        {
            if (!feuxFlag) 
            {
                feuxFrigateEnemyPos = player.transform.position;
                feuxWait = (GameManager.Instance.alienShipPrefabs[1]).GetComponent<Frigate_Enemy>().timeTillNextFP;
                StartCoroutine(feuxFrigate(bulletSpread));
                feuxFlag = true;
            }
            yield return new WaitForSeconds(time);
            knownPlayerPos = player.transform.position;
            StartCoroutine(updateFP(timeTillNextFP));
        }

    }
}