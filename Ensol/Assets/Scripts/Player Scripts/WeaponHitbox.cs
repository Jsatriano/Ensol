using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public CharController player;
    public PlayerCombatController pcc;
    public DamageFlash damageFlash;
    private bool isTriggered = false;
    [HideInInspector] public GameObject[] players;
    [HideInInspector] public bool isProjectile = false;
    private bool isMoving = true;
    public GameObject damagePulseVFX;
    public GameObject weaponThrowVFX;
    public GameObject weaponHitVFX;
    private Queue<GameObject> activeHitVFX = new Queue<GameObject>();

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

        if(!pcc.hasWeapon && pcc.isCatching && isProjectile) {
            isMoving = true;
            gameObject.transform.LookAt(pcc.weaponCatchTarget.transform);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, pcc.weaponCatchTarget.transform.position, pcc.weaponRecallSpeed * Time.deltaTime);
        }
        else if(!pcc.hasWeapon && isProjectile && !pcc.isCatching && isMoving) {
            gameObject.transform.position += Vector3.Normalize(gameObject.transform.forward) * pcc.weaponThrowSpeed * Time.deltaTime;
        }
        
    }

    void OnTriggerEnter(Collider col) {
        if (isTriggered == false) {
            if(col.gameObject.tag == "Enemy") {
                col.gameObject.GetComponent<EnemyStats>().TakeDamage(pcc.attackPower);
                AudioManager.instance.PlayOneShot(FMODEvents.instance.minorCut, this.transform.position);
                print("Should be spawning hit vfx");
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

        if(isProjectile && !pcc.isCatching) {
            if(col.gameObject.layer == 7) {
                isMoving = false;
                Collider[] damagePulse = Physics.OverlapSphere(gameObject.transform.position, pcc.damagePulseRadius, 6);
                damagePulseVFX.SetActive(true);
                StartCoroutine(DisablePulseVFX());
                pcc.attackPower = pcc.baseAttackPower * pcc.specialDamagePulseMult;
                foreach(Collider c in damagePulse) {
                    if(c.gameObject.tag == "Enemy") { 
                        print("Damage Pulse Hit Enemy");
                        c.gameObject.GetComponent<EnemyStats>().TakeDamage(pcc.attackPower);
                    }
                }
                pcc.attackPower = pcc.baseAttackPower * pcc.specialAttackMult;
            }
        }

        else if(isProjectile && pcc.isCatching) {
            if(col.gameObject.layer == 13) {
                pcc.GrabWeapon();
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
            player = p.GetComponent<CharController>();
            pcc = p.GetComponent<PlayerCombatController>();
        }

        if(player == null) {
            print("Weapon failed to locate Player");
        }
        else {
            print("Weapon located Player");
        }
    }

    IEnumerator DestroyHitVFX() {
        yield return new WaitForSeconds(1f);
        GameObject hitVFXToDestroy = activeHitVFX.Dequeue();
        Destroy(hitVFXToDestroy);
        print("destroyed a hit vfx");
    }

    IEnumerator DisablePulseVFX() //Elizabeth
    {
        yield return new WaitForSeconds(0.5f);
        damagePulseVFX.SetActive(false);
    }
}
