using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stat : MonoBehaviour
{
    [SerializeField]
    private int value;

    public int getValue()
    {
        return value;
    }

    public void incrementValue()
    {
        value += 1;
    }

    public void decrementValue()
    {
        value -= 1;
    }
}
