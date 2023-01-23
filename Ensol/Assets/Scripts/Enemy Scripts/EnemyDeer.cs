using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeer : EnemyController
{

    //A class for the robotic deer concept. Extends EnemyController. -Elizabeth

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); //Calls the parent start function.
        nameID = "EnemyDeer";
        numID = 0; //placeholder, idk if we even want this
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); //calls the parent update
        
    }
}
