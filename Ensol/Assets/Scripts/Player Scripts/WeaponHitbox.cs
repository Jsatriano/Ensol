using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public PlayerController player;
    public DamageFlash damageFlash;
    private bool isTriggered = false;
    [HideInInspector] public GameObject[] players;
    [HideInInspector] public bool isProjectile = false;
    private bool isMoving = true;
    public GameObject damagePulseVFX;
    public GameObject weaponThrowVFX;
    [HideInInspector] public GameObject weaponHitVFX;
    public GameObject electricHitVFX;
    public GameObject noElectricHitVFX;
    private Queue<GameObject> activeHitVFX = new Queue<GameObject>();
    private int rabbitCombo = 0;

    void Awake()
    {
        gameObject.tag = "Weapon";

        if(isProjectile) {
            gameObject.tag = "WeaponProjectile";
        }

        SearchForPlayer();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if(isProjectile) {
            gameObject.tag = "WeaponProjectile";
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerData.currentlyHasSolar) {
            weaponHitVFX = electricHitVFX;
        }
        else {
            weaponHitVFX = noElectricHitVFX;
        }
        if(isProjectile && gameObject.tag != "WeaponProjectile") {
            gameObject.tag = "WeaponProjectile";
        }
        if(player == null) {
            SearchForPlayer();
        }

        if(isProjectile && !isMoving) {
            gameObject.GetComponent<Collider>().enabled = false;
            weaponThrowVFX.SetActive(false);
        }
        else if(isProjectile && isMoving){
            gameObject.GetComponent<Collider>().enabled = true;
            weaponThrowVFX.SetActive(true);
        }

        if(!player.hasWeapon && player.isCatching && isProjectile) {
            isMoving = true;
            gameObject.transform.LookAt(player.weaponCatchTarget.transform);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, player.weaponCatchTarget.transform.position, player.weaponRecallSpeed * Time.deltaTime);
        }
        else if(!player.hasWeapon && isProjectile && !player.isCatching && isMoving) {
            gameObject.transform.position += Vector3.Normalize(gameObject.transform.forward) * player.weaponThrowSpeed * Time.deltaTime;
        }
        
    }

    void OnTriggerEnter(Collider col) {
        if (isTriggered == false) {
            if(col.gameObject.tag == "Enemy") {
                col.gameObject.GetComponent<EnemyStats>().TakeDamage(player.attackPower);
                //check if hitting two bunnies
                if (isProjectile){
                    if (col.gameObject.GetComponent<EnemyStats>().isRabbit){
                        rabbitCombo += 1;
                    }
                    if (rabbitCombo >= 2){
                        var ach = new Steamworks.Data.Achievement("Two_Rabbits_One_Spear");
                        ach.Trigger();
                    }
                }
                AudioManager.instance.PlayOneShot(FMODEvents.instance.minorCut, this.transform.position);
                Transform hitVFXTargetLocation = null;
                hitVFXTargetLocation = col.gameObject.transform.Find("Hit VFX Target Location");
                if(hitVFXTargetLocation == null) {
                    hitVFXTargetLocation = col.gameObject.transform;
                }
                GameObject hitVFX = Instantiate(weaponHitVFX, hitVFXTargetLocation);
                activeHitVFX.Enqueue(hitVFX);
                StartCoroutine(DestroyHitVFX());
            }
        }

        if(isProjectile && !player.isCatching) {
            if(col.gameObject.layer == 7 || col.gameObject.layer == 12) {
                isMoving = false;
                rabbitCombo = 0;
                Collider[] damagePulse = Physics.OverlapSphere(gameObject.transform.position, player.damagePulseRadius, 6);
                damagePulseVFX.SetActive(true);
                StartCoroutine(DisablePulseVFX());
                player.attackPower = player.baseAttackPower * player.specialDamagePulseMult;
                foreach(Collider c in damagePulse) {
                    if(c.gameObject.tag == "Enemy") { 
                        c.gameObject.GetComponent<EnemyStats>().TakeDamage(player.attackPower);                    
                    }
                }
                player.attackPower = player.baseAttackPower * player.specialAttackMult;
            }
        }

        else if(isProjectile && player.isCatching) {
            if(col.gameObject.layer == 13) {
                player.GrabWeapon();
                rabbitCombo = 0;
            }
        }
    }

    void onTriggerExit(Collider col) {
        isTriggered = false;
    }

    public void SearchForPlayer() {
        if(players == null || players.Length == 0) {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
        foreach(GameObject p in players) {
            player = p.GetComponent<PlayerController>();
        }
    }

    IEnumerator DestroyHitVFX() {
        yield return new WaitForSeconds(1f);
        GameObject hitVFXToDestroy = activeHitVFX.Dequeue();
        Destroy(hitVFXToDestroy);
    }

    IEnumerator DisablePulseVFX()
    {
        yield return new WaitForSeconds(0.5f);
        damagePulseVFX.SetActive(false);
    }
}
