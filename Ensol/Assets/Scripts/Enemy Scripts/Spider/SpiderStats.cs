using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderStats : EnemyStats
{
    [Header("Spider Movement")]
    public float minSpeed;
    public float rapidAvoidDist;
    public float idealDist;
    public float sidewaysMult;

    [Header("Web Deploy Attack")]
    public float webDelployCooldown;
    public float webDeployMinRange;
    public float webDeployMaxRange;
    public float webDeployDuration;
    public float webDeployDebuff;
    public float webDeployDebuffLength;
    public GameObject webPrefab;
    public SpiderWebManager webManager;
    public Transform webSpawnPoint;

    [Header("Web Shoot Attack")]

    [Header("Tazer Shot Attack")]
    public float tazerCooldown;
    public float tazerDamage;
    public float tazerMinRange;
    public float tazerMaxRange;
    public float tazerBurstNum;
    public float tazerBurstSpeed;
    public float tazerPower;
    public float tazerRotation;
    public Rigidbody boltPrefab;
    public Transform tazerSpawnPoint;
    public SpiderTazerManager tazerManager;

    [Header("References")]
    [SerializeField] private SpiderBT spiderBT;
    public GameObject thisSpider;
    [SerializeField] private DamageFlash damageFlash;
    [SerializeField] private ButtonGateController buttonGateController = null;
    [SerializeField] private ButtonDoorController buttonDoorController = null;


    protected override void Start()
    {
        base.Start(); //Calls the parent start function.
        nameID = "EnemySpider";
        numID = 1; //placeholder, idk if we even want this
        spiderBT.isAlive = true;
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
        if (spiderBT.root.GetData("player") == null)
        {
            spiderBT.root.SetData("player", playerTF);
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
        spiderBT.isAlive = false;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.deathCut, this.transform.position);
        //-------------> Spider death sound here <----------------

        //Random chance for creating a shield drop on death
        if (Random.Range(0f, 1f) < shieldDropChance)
        {
            Instantiate(shieldDropPrefab, transform.position, transform.rotation);
        }

        // tells door and gate scripts that this bear has died
        if (buttonGateController != null)
        {
            buttonGateController.enemyKilled(thisSpider);
        }
        if (buttonDoorController != null)
        {
            buttonDoorController.enemyKilled(thisSpider);
        }

        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");  
    }
}
