using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitStats : EnemyStats
{
    [Header("Agro Behavior")]
    public float attackDamage;  //How much damage the rabbit does
    public float agroDuration;  //How long the rabbit stays in the agro behavior
    public SphereCollider attackHitbox;
    public float agroRange;

    [Header("Evade Behavior")]
    public float evadeDuration; //How long the rabbit stays in the evade behavior
    public float evadeDistance; //How far away from the player the rabbit tries to stay when evading

    [Header("Other Scripts")]
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
        rabbitBT.isAlive = false;
        //Turn off hitbox here
        AudioManager.instance.PlayOneShot(FMODEvents.instance.deathCut, this.transform.position);
        //Play rabbit death sound here

        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");

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
