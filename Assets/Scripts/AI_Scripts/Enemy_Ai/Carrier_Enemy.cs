using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Edit this object in the Unity UI at Assets/Prefabs/Enemy_Ai/obj_carrier.prefab
Note: Might want to add a death animation in the future

Created by:
- SalilPT

SFX implementation by Dylan Mahler
*/

namespace AsterionArcade
{
    public class Carrier_Enemy : Enemy
    {
        // Inspector Variables
        [Header("Class-Specific Variables")]
        [Tooltip("The minimum distance this enemy needs to be to the player ship to explode")]
        public double explodeDist = 4;
        [Tooltip("The number of fighters to spawn when this enemy is destroyed.")]
        public uint numFightersToSpawn = 5;
        // Used to determine whether or not this object was destroyed because it was close enough to the player
        private bool inRangeOfPlayer = false;

        [Header("SFX References")]
        [SerializeField] FMODUnity.EventReference carrierExplodeSFX;

        // Update is called every frame
        void Update()
        {
            Vector2 myPos = transform.position;
            // Check if this object is in range of the player to destroy itself. If it is, then destroy self.
            // Player position comes from Enemy.cs
            if (Vector2.Distance(myPos, player.transform.position) <= explodeDist)
            {
                inRangeOfPlayer = true;

                // Play the SFX that plays when the carrier ship explodes IF we haven't lost the whole game by battery hitting 0%.
                if (!GameManager.Instance.gameLost){ FMODUnity.RuntimeManager.PlayOneShot(carrierExplodeSFX.Guid); }

                Destroy(this.gameObject);
            }
        }

        // Method to determine the spawn positions of the fighters that this object spawns
        // You can change this to create different spawn patterns
        private List<Vector3> GenerateListOfSpawnPositions()
        {
            List<Vector3> result = new List<Vector3>();
            Vector3 myPos = (Vector3)transform.position;

            Quaternion rotationQuat = transform.rotation;
            // Anonymous function which returns the provided point rotated about the center of this object (on the play area)
            // Adapted from the answer here: https://forum.unity.com/threads/rotate-a-point-around-a-second-point-using-a-quaternion.504996/
            System.Func<Vector3, Vector3> GenerateRotatedPoint = (p) => {return (rotationQuat * (p - myPos) + myPos);};

            // Spawn 1 fighter at the center of this object and 4 fighters equidistant to the center
            result.Add(myPos);
            result.Add(GenerateRotatedPoint(myPos + new Vector3(-0.5f, -0.5f, 0)));
            result.Add(GenerateRotatedPoint(myPos + new Vector3(-0.5f, 0.5f, 0)));
            result.Add(GenerateRotatedPoint(myPos + new Vector3(0.5f, -0.5f, 0)));
            result.Add(GenerateRotatedPoint(myPos + new Vector3(0.5f, 0.5f, 0)));

            // If numFightersToSpawn is greater than 5, spawn the remaining fighters randomly around this object
            for (int i = 5; i < numFightersToSpawn; i++) {
                result.Add(myPos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.0f, 0.5f), 0));
            }

            return result;
        }

        // Spawns a fighter as a child of shipsTransform. Similar to the SpawnShip() method in Spawning.cs .
        private void SpawnFighter(Vector3 position, Transform shipsTransform) {
            int fighterShipID = 0;
            GameObject newFighter = Instantiate(GameManager.Instance.alienShipPrefabs[fighterShipID], shipsTransform);

            // Spawn the new fighter on the same layer as this object
            newFighter.layer = this.gameObject.layer;

            newFighter.transform.position = position * Vector2.one;

            if (newFighter.TryGetComponent<Enemy>(out Enemy ship1))
            {
                ship1.lookingForPlayer = true;
            }

            newFighter.GetComponent<Enemy>().isAstramori = isAstramori;
        }

        // When this object is destroyed, call this function
        void OnDestroy()
        {
            // Get the health of this object through the fighter_enemy_health script attached to it
            int myHealth = this.GetComponent<fighter_enemy_health>().health;

            // Need to check cause of destruction
            if (myHealth > 0 && !inRangeOfPlayer) {
                return;
            }

            List<Vector3> listOfSpawnPositions = GenerateListOfSpawnPositions();
            // Set the transform component that fighters will spawn as a child of to be the parent of this object
            Transform shipsTransform = transform.parent;

            for (int i = 0; i < numFightersToSpawn; i++)
            {
                this.SpawnFighter(listOfSpawnPositions[i], shipsTransform);
            }
        }
    }
}
