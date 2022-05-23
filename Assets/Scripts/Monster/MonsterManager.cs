using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    public PlayerRoomDetection playerRoomDetection;
    public ArcadeTextEyeball arcadeTextEyeball;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private List<Transform> asterionMonsterLocations;
    [SerializeField] private List<Transform> astramoriMonsterLocations;
    [SerializeField] private List<Transform> hallwayMonsterLocations;
    public List<string> possibleArcadeMachineMessages;
    public float messagechance;
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
            //SpawnMonsterBehindCurrentRoomMachine();
        }
    }

    public void UpdatedPlayerPos()
    {
        if(playerRoomDetection.playerLocation == PlayerRoomDetection.Location.Walkway)
        {
            if(Random.Range(0.0f, 100.0f) <= messagechance)
            {
                StartCoroutine(RandomArcadeMachineMessageFlash());
            }
        }
    }

    public IEnumerator RandomArcadeMachineMessageFlash()
    {
        yield return new WaitForSeconds(Random.Range(2.5f,5f));

        arcadeTextEyeball.ToggleEyeballs(false);
        arcadeTextEyeball.SetAllSize(25f);
        arcadeTextEyeball.SetAllText(".");

        yield return new WaitForSeconds(0.3f);
        arcadeTextEyeball.SetAllText("..");

        yield return new WaitForSeconds(0.3f);
        arcadeTextEyeball.SetAllText("...");

        yield return new WaitForSeconds(1f);
        arcadeTextEyeball.SetAllSize(13f);
        arcadeTextEyeball.SetAllText(possibleArcadeMachineMessages[Random.Range(0, possibleArcadeMachineMessages.Count)]);

        yield return new WaitForSeconds(3f);
        arcadeTextEyeball.SetAllSize(25f);
        arcadeTextEyeball.SetAllText("...");

        yield return new WaitForSeconds(0.5f);

        arcadeTextEyeball.ToggleEyeballs(true);
        arcadeTextEyeball.SetAllText("");
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

    public void SpawnMonsterBehindAsterionMachine()
    {
        var monster = Instantiate(monsterPrefab, asterionMonsterLocations[0].position, Quaternion.identity);
        monster.transform.eulerAngles = new Vector3(90, -180, 0);
        monster.transform.parent = transform;
        currentlyActiveMonsterSpawns.Add(monster);
        StartCoroutine(DestroyMonstersRoutine(3));
    }

    public void SpawnMonsterBehindAstramoriMachine()
    {
        var monster = Instantiate(monsterPrefab, astramoriMonsterLocations[0].position, Quaternion.identity);
        monster.transform.eulerAngles = new Vector3(90, -180, 180);
        monster.transform.parent = transform;
        currentlyActiveMonsterSpawns.Add(monster);
        StartCoroutine(DestroyMonstersRoutine(3));
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
