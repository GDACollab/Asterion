using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsterionArcade {

    public class Spawning : MonoBehaviour
    {


        [Header("Objects")]
        [SerializeField] GameObject[] shipPrefabs;
        [SerializeField] Transform ships;
        [SerializeField] Transform preview;
        [SerializeField] Transform starfighter;
        [SerializeField] FakeCursor fc;
        [SerializeField] Camera astramoriCamera;
        [SerializeField] AstramoriManager astramoriManager;

        [Header("Main Controls")]
        [SerializeField] bool ship1Active;
        [SerializeField] int selectedShip = 1;
        [SerializeField] float invalidRange = 1;
        [SerializeField] float selectionSens = 0.5f;

        [Header("Preview Controls")]
        [SerializeField] float previewAlpha = 0.25f;
        [SerializeField] float previewInvalidAlpha = 0.1f;


        // Start is called before the first frame update
        void Start()
        {
            SelectShip(1);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1)) ActivateShip1();

            UpdateHotKeys();
            UpdateSpawn();
        }

        // Changes selected ship based on Hot Keys
        void UpdateHotKeys()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectShip(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectShip(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectShip(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SelectShip(4);
            }
        }

        void UpdateSpawn()
        {
            // Preview Spawn
            /*
            preview.localPosition = (Input.mousePosition) * Vector2.one;
            Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            preview.localPosition -= center;
            preview.localPosition *= selectionSens;
            */

            preview.position = (fc.transform.GetComponent<RectTransform>().transform.position);


            //Debug.Log(preview.localPosition);
            preview.localPosition = new Vector3(preview.localPosition.x, preview.localPosition.y, 0);
            preview.GetComponent<SpriteRenderer>().color *= new Color(1, 1, 1, 0);
            preview.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, CanPlace() ? previewAlpha : previewInvalidAlpha);

            // Spawn Ship
            if (Input.GetKeyDown(KeyCode.Mouse0) && CanPlace() && fc.collidingObjects.Count <= 0)
            {
                Vector3 position = preview.position;
                SpawnShip(selectedShip, position);
            }
        }

        // Change Selected Ship
        public void SelectShip(int shipID)
        {
            selectedShip = shipID;
            preview.GetComponent<SpriteRenderer>().color = shipPrefabs[shipID - 1].GetComponent<SpriteRenderer>().color * new Color(1, 1, 1, previewAlpha);
        }

        void SpawnShip(int shipID, Vector3 position)
        {
            GameObject ship = Instantiate(GameManager.Instance.alienShipPrefabs[shipID - 1], ships);
            ship.layer = 12;
            ship.transform.position = position * Vector2.one;
            if (ship.TryGetComponent<scr_fighter_move>(out scr_fighter_move ship1))
            {

                ship1.seeking = true;
            }

            astramoriManager.enemyQueue.Add(shipID);
        }

        void ActivateShip1()
        {
            ship1Active = !ship1Active;
            foreach (Transform ship in ships)
            {
                if (ship.TryGetComponent<Ship1>(out Ship1 ship1)) ship1.Activate(ship1Active);
            }
        }

        bool CanPlace()
        {
            return (preview.position - starfighter.position).magnitude > invalidRange;
        }
    }
}