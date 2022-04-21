using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoomDetection : MonoBehaviour
{
    public enum Location {AsterionRoom, AstramoriRoom, Walkway};
    public Location playerLocation;
    [SerializeField] private MonsterManager monsterManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AsterionRoom")
        {
            playerLocation = Location.AsterionRoom;
        }
        else if (other.tag == "AstramoriRoom")
        {
            playerLocation = Location.AstramoriRoom;
        }
        else if (other.tag == "Walkway")
        {
            playerLocation = Location.Walkway;
        }

        monsterManager.UpdatedPlayerPos();


    }
}
