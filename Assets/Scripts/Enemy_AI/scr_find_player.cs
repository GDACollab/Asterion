using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Delayed location detection for Ai.
 *
 * Developer: Jonah Ryan
 */

public class scr_find_player : MonoBehaviour
{
    // Seconds till next Update, each item in list is a new delay type

    [Tooltip("Add new number to create a new AI Type, number = second delay till next position update.")]
    public List<float> m_All_AI_Type_Delays = new List<float>();

    // Players position, according to last update

    public static List<Vector3> m_FindPlayer = new List<Vector3>();

    public GameObject m_Player;


    private void Start()
    {
        // Set AI_Types position defaults 
        for (int i = 0; i < m_All_AI_Type_Delays.Count; i++)
        {
            m_FindPlayer.Add(m_Player.transform.position);

            if(m_All_AI_Type_Delays[i] != 0)
            {
                StartCoroutine(UpdatePosition(i)); 
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        // Instantly update 0 second delay AI_Types.
        for (int i = 0; i < m_All_AI_Type_Delays.Count; i++)
        {
            if(m_All_AI_Type_Delays[i] == 0)
            {
                m_FindPlayer[i] = m_Player.transform.position;
            }
        }
    }

    public void UpdateAITargets()
    {

    }

    IEnumerator UpdatePosition(int Ai_Type)
    {
        yield return new WaitForSeconds(m_All_AI_Type_Delays[Ai_Type]);
        m_FindPlayer[Ai_Type] = m_Player.transform.position;
        StartCoroutine(UpdatePosition(Ai_Type));
    }

    // Get Players position, according to last update of specified Ai_Type.
    public static Vector3 Get_Player_Pos(int Ai_Type)
    {
        return m_FindPlayer[Ai_Type];
    }

}


