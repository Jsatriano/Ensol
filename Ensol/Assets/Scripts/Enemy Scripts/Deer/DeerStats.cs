using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerStats : EnemyStats
{

    //A class for the robotic deer concept. Extends EnemyController. -Elizabeth

    public Transform playerTF;
    public Transform deerTF;   
    public BoxCollider hitZone;
    public Rigidbody deerRB;
    public float visionRange;
    public float chargeSpeed;  
    public float windupLength;
    public float cooldownLength;
    public float chargeTurning;
    public float rotationSpeed;

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
