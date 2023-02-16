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
    public bool isProjectile = false;
    private bool isMovingForward = true;

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

        if(!pcc.hasWeapon && pcc.isCatching && isProjectile) {
            gameObject.transform.LookAt(pcc.weaponCatchTarget.transform);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, pcc.weaponCatchTarget.transform.position, pcc.weaponRecallSpeed * Time.deltaTime);
        }
        else if(!pcc.hasWeapon && isProjectile && !pcc.isCatching && isMovingForward) {
            gameObject.transform.position += Vector3.Normalize(gameObject.transform.forward) * pcc.weaponThrowSpeed * Time.deltaTime;
        }
        
    }

    void OnTriggerEnter(Collider col) {
        if (isTriggered == false) {
            if(col.gameObject.tag == "Enemy") {
                col.gameObject.GetComponent<EnemyStats>().TakeDamage(pcc.attackPower);
            }
        }

        if(isProjectile && !pcc.isCatching) {
            if(col.gameObject.layer == 7) {
                isMovingForward = false;
            }
        }

        else if(isProjectile && pcc.isCatching) {
            if(col.gameObject.tag == "Player") {
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
}
