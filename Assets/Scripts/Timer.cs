using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AsterionArcade
{
    public class Timer : MonoBehaviour
    {
        TextMeshProUGUI timer;
        bool isRunning;
        public float startingTime;
        public float time;
        [SerializeField] AstramoriManager astramoriManager;

        // Start is called before the first frame update
        void Start()
        {
            timer = GetComponent<TextMeshProUGUI>();
            time = startingTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (isRunning)
            {
                if (time > 0) time -= Time.deltaTime;
                timer.text = Mathf.FloorToInt(time) + 1 + "";

                if(isRunning && time <= 0)
                {
                    isRunning = false;
                    astramoriManager.GameConcluded(false);

                }
            }
            
        }

        

        public void StartTimer()
        {
            time = startingTime;
            isRunning = true;
        }

        public void StopTimer()
        {
            isRunning = false;
        }



        


    }
}