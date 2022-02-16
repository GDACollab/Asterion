using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_bullet : BasicBullet
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        isAlien = false;
    }


}
