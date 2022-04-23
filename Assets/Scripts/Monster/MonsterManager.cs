using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    public PlayerRoomDetection playerRoomDetection;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private List<Transform> asterionMonsterLocations;
    [SerializeField] private List<Transform> astramoriMonsterLocations;
    [SerializeField] private List<Transform> hallwayMonsterLocations;
    public List<GameObject> currentlyActiveMonsterSpawns;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SpawnMonsterBehindCurrentRoomMachine();
        }
    }

    public void UpdatedPlayerPos()
    {

    }

    public void SpawnMonsterBehindCurrentRoomMachine()
    {
        if (playerRoomDetection.playerLocation == PlayerRoomDetection.Location.AsterionRoom)
        {
            var monster = Instantiate(monsterPrefab, asterionMonsterLocations[0].position, Quaternion.identity);
            monster.transform.eulerAngles = new Vector3(90, -180, 0);
            monster.transform.parent = transform;
            currentlyActiveMonsterSpawns.Add(monster);
            StartCoroutine(DestroyMonstersRoutine(3));
        }
        else if (playerRoomDetection.playerLocation == PlayerRoomDetection.Location.AstramoriRoom)
        {
            var monster = Instantiate(monsterPrefab, astramoriMonsterLocations[0].position, Quaternion.identity);
            monster.transform.eulerAngles = new Vector3(90, -180, 180);
            monster.transform.parent = transform;
            currentlyActiveMonsterSpawns.Add(monster);
            StartCoroutine(DestroyMonstersRoutine(3));
        }
    }

    public IEnumerator DestroyMonstersRoutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        foreach(GameObject g in currentlyActiveMonsterSpawns)
        {
            currentlyActiveMonsterSpawns.Remove(g);
            Destroy(g);
        }
    }
    
}
