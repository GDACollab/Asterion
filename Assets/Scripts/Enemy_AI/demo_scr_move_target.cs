using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Demo movement controller for ai target.
 *
 * Developer: Jonah Ryan
 */

public class demo_scr_move_target : MonoBehaviour
{

    public int bounds_Top_Y = 9;
    public int bounds_Bot_Y = -7;
    public int bounds_Top_X = 18;
    public int bounds_Bot_X = -18;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Translate(Input.GetAxis("Horizontal")/2f, Input.GetAxis("Vertical")/2f, 0);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bounds_Bot_X, bounds_Top_X), Mathf.Clamp(transform.position.y, bounds_Bot_Y, bounds_Top_Y), 0);
    }
}
