using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    /* This will be the parent class for all enemy npcs. 
    Everything that is common to ALL ENEMY NPCS goes in this class.
    Specific enemies should then each have their own script which extends this class.
    Override stats/abilities/etc in that child class.
    -Elizabeth */

    //STATS

    public float maxHP = 10; //max health
    public float currHP; //current health
    public float attackPower = 5; //used in damage calculations
    public float speed = 5; //move speed
    public float attackRange;

    [SerializeField] public string nameID; //a string name to ID the enemy
    [SerializeField] public int numID; //an int to id an enemy, if we want that too? maybe to differentiate units with the same name?
    public float visionRange; //The radius at which the enemy detects the player
    public float rotationSpeed; //How fast the enemy can rotate when tracking the player
    public float obstacleDetectRadius; //The radius at which the enemy detects obstacles. Used for avoiding obstacles when moving


    //OTHER VARIABLES
    [HideInInspector] public GameObject[] players;
    public CharController player; //Stores reference to player, in order to deal damage/otherwise affect them.
    public Transform playerTF; //Player transform
    public Transform enemyTF; //Enemy Transform
    public BoxCollider hitZone; //The attacking hitbox of the enemy
    public Rigidbody enemyRB; //The enemy Rigidbody
    public LayerMask obstacleMask; //The layer(s) that obstacles in the arena are on

    //Playtesting
    public MeshRenderer enemyMaterial;
    public Material defaultMaterial;
    public Material windupMaterial;
    public Material attackMaterial;

    private void Awake() {
        gameObject.tag = "Enemy";
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currHP = maxHP;
        gameObject.tag = "Enemy";
        nameID = "DefaultEnemy";
        numID = -1;

        SearchForPlayer();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //checks if player has been successfully located, if not, tries to locate it
        if(player == null) {
            SearchForPlayer();
        }
        //Checks to see if the enemy is dead
        if(currHP <= 0) {
            print(nameID + " is dead!");
            Destroy(gameObject);
        }
    }

    public void SearchForPlayer() {
        if(players.Length == 0) {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
        foreach(GameObject p in players) {
            player = p.GetComponent<CharController>();
            playerTF = p.GetComponent<Transform>();
        }

        if(player == null) {
            print("EnemyStats failed to locate Player");
        }
        else {
            print("EnemyStats located Player");
        }
    }
}
