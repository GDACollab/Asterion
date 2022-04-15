using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public enum Location {Asterion, Astramori, Hallway, MiddleHallway };
    public Location playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerPos(int posIndex)
    {
        switch (posIndex)
        {
            case 0:
                break;
            default:
                break;
        }
    }
}
