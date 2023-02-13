using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerStats : EnemyStats
{

    //A class for the robotic deer concept. Extends EnemyController. -Elizabeth

    [Header("Charge Stats/Components")]
    public float chargeMaxSpeed;      //How fast the charge is\
    public float chargeAccel;         //How fast the deer gets to max speed when chargin
    public float chargeWindupLength;  //How long the windup of charge is
    public float chargeCooldown;      //How long the cooldown for charge is
    public float chargeDamage;        //How much damage the charge does
    public float chargeTurning;       //How much the deer can turn while charging
    public BoxCollider chargeHitbox;  //Hitbox for the charge

    [Header("Basic Attack Stats/Components")]
    public float attackCooldown;           //Cooldown between basic attacks
    public float basicAttackDuration;      //How long the hitbox is active for basic attacks
    public float basicAttackWindup;        //How long the windup is for basic attacks
    public float attackRange;              //How close the player needs to be for the enemy to basic attack
    public BoxCollider basicAttackHitbox;  //The hitbox for the basic attack

    [Header("Other Things")]
    public float distanceFromPlayer; //The distance the deer tries to stay away from the player
    public DeerBT deerBT;
    public DamageFlash damageFlash;
    public ButtonController buttonController;
    public GameObject thisDeer;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); //Calls the parent start function.
        nameID = "EnemyDeer";
        numID = 0; //placeholder, idk if we even want this
        basicAttackHitbox.center = new Vector3(0, 0, 0f);
        basicAttackHitbox.size = new Vector3(attackRange / 2, 1.5f, (attackRange - 2.8f));
        deerBT.isAlive = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); //calls the parent update       
    }

    public override void TakeDamage(float damage)
    {
        if (currHP <= 0)
        {
            return;
        }
        currHP -= damage;
        StartCoroutine(damageFlash.FlashRoutine());
        print("Did " + damage + " damage to " + nameID);
        if (currHP <= 0)
        {
            Die();
        }
        return;
    }

    public override void Die()
    {
        print(nameID + " is dead!");
        deerBT.isAlive = false;
        chargeHitbox.enabled = false;
        basicAttackHitbox.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        buttonController.enemyKilled(thisDeer);      
    }
}
