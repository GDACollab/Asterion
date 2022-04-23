using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Edit this object in the Unity UI at Assets/Prefabs/Enemy_Ai/obj_carrier.prefab
Note: Might want to add a death animation in the future

BUGS:
Fighters spawned in the same exact position are bad

Created by:
- SalilPT
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
        // The transform component for ships to spawn under.
        private Transform shipsTransform;
        // Used to determine whether or not this object was destroyed because it was close enough to the player
        private bool inRangeOfPlayer = false;

        // Is this object currently in Astramori?
        private bool inAstramori;

        void Awake()
        {
            inAstramori = this.GetComponent<Enemy>().isAstramori;
            // Set the transform component that fighters will spawn as a child of to be the parent of this object
            shipsTransform = transform.parent;
        }

        // Update is called every frame
        void Update()
        {
            
            Vector2 myPos = transform.position;
            // Check if this object is in range of the player to destroy itself. If it is, then destroy self.
            // Player position comes from Enemy.cs
            if (Vector2.Distance(myPos, player.transform.position) <= explodeDist)
            {
                Debug.Log("CARRIER IN RANGE");
                inRangeOfPlayer = true;
                Destroy(this.gameObject);
            }
            
        }
        
        // Method to determine the spawn positions of the fighters that this object spawns
        List<Vector3> populateListOfSpawnPositions()
        {
            List<Vector3> result = new List<Vector3>();

            result.Add((Vector3)transform.position);
            result.Add((Vector3)transform.position);
            result.Add((Vector3)transform.position);
            result.Add((Vector3)transform.position);
            result.Add((Vector3)transform.position);
            return result;
        }
        
        // Spawns a fighter. Similar to the SpawnShip method in Spawning.cs .
        private void SpawnFighter(Vector3 position) {
            int fighterShipID = 0;
            GameObject newFighter = Instantiate(GameManager.Instance.alienShipPrefabs[fighterShipID], shipsTransform);

            // Spawn the new fighter on the correct layer
            newFighter.layer = inAstramori ? 12 : 7;

            newFighter.transform.position = position * Vector2.one;

            if (newFighter.TryGetComponent<Enemy>(out Enemy ship1))
            {
                ship1.lookingForPlayer = true;
            }

            newFighter.GetComponent<Enemy>().isAstramori = true;
        }

        // When this object is destroyed, call this function
        void OnDestroy()
        {
            // Get the health of this object through the fighter_enemy_health script attached to it
            int myHealth = this.GetComponent<fighter_enemy_health>().health;
            Debug.Log(myHealth);
            // Need to check cause of destruction
            if ((myHealth <= 0) || inRangeOfPlayer)
                {
                List<Vector3> listOfSpawnPositions = populateListOfSpawnPositions();
                for (int i = 0; i < numFightersToSpawn; i++)
                {
                    this.SpawnFighter(listOfSpawnPositions[i]);
                }
            }
        }

    }
}