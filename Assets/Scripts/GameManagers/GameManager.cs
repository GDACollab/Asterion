using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AsterionArcade
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public List<GameObject> alienShipPrefabs;
        public ShipStats shipStats;
        [SerializeField] TextMeshProUGUI coinText;
        public AsterionManager asterionManager;
        public AstramoriManager astramoriManager;
        public int coinCount;
        public int sanity;

        //acts as a singleton which can be easily referenced with GameManager.Instance

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);

            }
            else
            {
                if (Instance != this)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            coinText.text = "" + coinCount;
            sanity = 100;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AlterCoins(int diff)
        {
            coinCount += diff;
            if (coinCount < 0)
            {
                coinCount = 0;
            }

            coinText.text = "" + coinCount;
        }
    }
}
