using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerStats : EnemyStats
{

    //A class for the robotic deer concept. Extends EnemyController. -Elizabeth

    public float chargeSpeed;  //How fast the charge is
    public float windupLength; //How long the windup of charge is
    public float cooldownLength; //Cooldown of charge
    public float chargeTurning; //How much the deer can turn while charging
    public float distanceFromPlayer; //The distance the deer tries to stay away from the player
    

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
