using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fighter_enemy_bullet : BasicBullet
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        isAlien = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
