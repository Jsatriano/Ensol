using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitStats : EnemyStats
{
    [Header("Aggro Behavior")]
    public float attackDamage;  //How much damage the rabbit does
    public SphereCollider attackHitbox;
    public float aggroLeaps;
    public GameObject grinderVFX;
    public float aggroSpeed;

    [Header("Evade Behavior")]
    public float evadeDistance; //How far away from the player the rabbit tries to stay when evading
    public float landingDrag;   //The drag to be applied when the rabbit is landing from a leap
    public float normalDrag;    //The rabbit's normal drag
    public float minSpeed;
    public float rapidAvoidDist;

    [Header("References")]
    [SerializeField] private RabbitBT rabbitBT;
    public DamageFlash damageFlash;
    public ButtonGateController buttonGateController = null;
    public ButtonDoorController buttonDoorController = null;
    [SerializeField] private GameObject thisRabbit;

    protected override void Start()
    {
        base.Start(); //Calls the parent start function.
        nameID = "EnemyRabbit";
        numID = 3; //placeholder, idk if we even want this
        rabbitBT.isAlive = true;
        enemyRB.drag = normalDrag;
        grinderVFX.SetActive(false);
    }

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
        if (rabbitBT.root.GetData("player") == null)
        {
            rabbitBT.root.SetData("player", playerTF);
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
        PlayerData.bunniesKilled++;
        print(nameID + " is dead!");
        rabbitBT.isAlive = false;
        attackHitbox.enabled = false;
        grinderVFX.SetActive(false);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.deathCut, this.transform.position);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.bunnyDeath, this.transform.position);

        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        //Random chance for creating a shield drop on death
        if (Random.Range(0f, 1f) < shieldDropChance)
        {
            Instantiate(shieldDropPrefab, transform.position, transform.rotation);
        }

        // tells door and gate scripts that this rabbit has died
        if (buttonGateController != null)
        {
            buttonGateController.enemyKilled(thisRabbit);
        }
        if (buttonDoorController != null)
        {
            buttonDoorController.enemyKilled(thisRabbit);
        }     
    }
}
