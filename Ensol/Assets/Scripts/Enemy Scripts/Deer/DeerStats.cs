using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerStats : EnemyStats
{

    //A class for the robotic deer concept. Extends EnemyController. -Elizabeth

    [Header("Charge Stats/Components")]
    public float chargeMaxSpeed;      //How fast the charge is
    public float chargeAccel;         //How fast the deer gets to max speed when chargin
    public float chargeCooldown;      //How long the cooldown for charge is
    public float windupRotation;      //How much the deer rotates to face the player during windup
    public float chargeDamage;        //How much damage the charge does
    public float chargeTurning;       //How much the deer can turn while charging
    public float chargeRange;         //How close the player needs to be for the enemy to charge
    public BoxCollider chargeHitbox;  //Hitbox for the charge

    [Header("Basic Attack Stats/Components")]
    public float attackCooldown; //Cooldown between basic attacks
    public float basicDamage;    //How much damage the basic attack does
    public float attackRange;    //How close the player needs to be for the enemy to basic attack
    public float windupTurning;  //How much the deer can turn to face the player during windup
    public BoxCollider basicAttackHitbox; //Hibox for the attack

    [Header("References")]
    public DeerBT deerBT;
    public DamageFlash damageFlash;
    public ButtonGateController buttonGateController = null;
    public ButtonDoorController buttonDoorController = null;
    public GameObject thisDeer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); //Calls the parent start function.
        nameID = "EnemyDeer";
        numID = 0; //placeholder, idk if we even want this
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
        //Sets the enemy to aggro if they aren't yet
        if (deerBT.root.GetData("player") == null)
        {
            deerBT.root.SetData("player", playerTF);
        }
        //print("Did " + damage + " damage to " + nameID);
        if (currHP <= 0) // If deer takes damage and dies, it plays final sound effect, otherwise, it plays a regular sfx
        {
            Die();
        }
        return;
    }

    public override void Die()
    {
        PlayerData.deerKilled++;
       // print(nameID + " is dead!");
        deerBT.isAlive = false;
        chargeHitbox.enabled = false;
        basicAttackHitbox.enabled = false;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.deathCut, this.transform.position);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.deerDeath, this.transform.position);

        //Random chance for creating a shield drop on death
        if (Random.Range(0f, 1f) < shieldDropChance)
        {
            Instantiate(shieldDropPrefab, transform.position, transform.rotation);
        }

        // tells door and gate scripts that this deer has died
        if (buttonGateController != null)
        {
            buttonGateController.enemyKilled(thisDeer);
        }
        if(buttonDoorController != null)
        {
            buttonDoorController.enemyKilled(thisDeer);
        }
        
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
    }

}
