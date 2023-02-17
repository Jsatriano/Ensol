using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearStats : EnemyStats
{
    [Header("Swipe Attack")]
    public float swipeDamage;      //How much damage the swipe does
    public float swipeCooldown;     //Cooldown on the swipe ability
    public float swipeRange;        //Range at which the swipe attack is triggered
    public float swipeMovement;     //How much the bear moves forward when swiping
    public float windupTurning;     //How much the bear turns towards the player during the windup
    public SphereCollider swipeHitbox1; //Hitbox of the swipe attack (Has 2 hitboxes, one on each hand)
    public SphereCollider swipeHitbox2; //Hitbox of the swipe attack

    [Header("Junk Attack")]
    public float junkDamage;
    public float junkCooldown;
    public float junkRange;


    [Header("Other Things")]
    public BearBT bearBT;
    public DamageFlash damageFlash;
    public GameObject thisBear;
    public ButtonGateController buttonController;

    protected override void Start()
    {
        base.Start(); //Calls the parent start function.
        nameID = "EnemyBear";
        numID = 1; //placeholder, idk if we even want this
        bearBT.isAlive = true;
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
        bearBT.isAlive = false;
        /*
        chargeHitbox.enabled = false;
        basicAttackHitbox.enabled = false;
        */
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        buttonController.enemyKilled(thisBear);      
    }
}
