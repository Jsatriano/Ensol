using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearStats : EnemyStats
{
    [Header("Swipe Attack")]
    public float swipeDamage;    //How much damage the swipe does
    public float swipeCooldown;  //Cooldown on the swipe ability
    public float swipeRange;     //Range at which the swipe attack is triggered
    public float swipeMovement;  //How much the bear moves forward when swiping
    public float swipeRotation;  //How much the bear turns towards the player during the windup
    public SphereCollider swipeHitbox1; //Hitbox of the swipe attack (Has 2 hitboxes, one on each hand)
    public SphereCollider swipeHitbox2; //Hitbox of the swipe attack

    [Header("Junk Attack")]
    public float junkDamage;    //How much damage the junk ball does
    public float junkCooldown;  //Cooldown on the junk attack
    public float junkMinRange;  //Min range at which the bear will use the junk attack
    public float junkMaxRange;  //Max range at which the bear will use the junk attack
    public float junkRotation;  //How well the bear can track the player during windup
    public float junkMaxSpeed;  //Max speed junk can be thrown at
    public float junkMinSpeed;  //Min speed junk can be thrown at
    public float explosionDamage;  //How much damage the explosion does
    public float explosionLength;  //How long the explosion lasts
    public float explosionSize;    //How big the explosion gets

    [Header("2nd Phase")]
    public float angryMaxSpeed;
    public float angryAcceleration;
    public float angryJunkCooldown;

    [Header("References")]
    public BearBT bearBT;
    public GameObject thisBear;
    [SerializeField] private DamageFlash damageFlash;
    [SerializeField] private ButtonGateController buttonGateController = null;
    [SerializeField] private ButtonDoorController buttonDoorController = null;

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
        if (currHP / maxHP <= 0.5f)
        {
            bearBT.root.SetData("belowHalf", true);
            bearBT.root.SetData("junkCooldown", angryJunkCooldown);
        }
        StartCoroutine(damageFlash.FlashRoutine());
        //Sets the enemy to aggro if they aren't yet
        if (bearBT.root.GetData("player") == null)
        {
            bearBT.root.SetData("player", playerTF);
        }
        //print("Did " + damage + " damage to " + nameID);
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
        swipeHitbox1.enabled = false;
        swipeHitbox2.enabled = false;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.deathCut, this.transform.position);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.bearDeath, this.transform.position);

        //Random chance for creating a shield drop on death
        if (Random.Range(0f, 1f) < shieldDropChance)
        {
            Instantiate(shieldDropPrefab, transform.position, transform.rotation);
        }

        // tells door and gate scripts that this bear has died
        if (buttonGateController != null)
        {
            buttonGateController.enemyKilled(thisBear);
        }
        if(buttonDoorController != null)
        {
            buttonDoorController.enemyKilled(thisBear);
        }

        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        //buttonController.enemyKilled(thisBear);      
    }
}
