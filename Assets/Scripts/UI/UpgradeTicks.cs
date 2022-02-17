using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTicks : MonoBehaviour
{
    public List<GameObject> ticks;

    public void SetTicks(int numTicks)
    {
        for(int i = 0; i < 4; i++)
        {
            if(i < numTicks)
            {
                ticks[i].SetActive(true);
            }
            else
            {
                ticks[i].SetActive(false);
            }
        }
    }

}
