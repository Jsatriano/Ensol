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

    [SerializeField] public string nameID; //a string name to ID the enemy
    [SerializeField] public int numID; //an int to id an enemy, if we want that too? maybe to differentiate units with the same name?

    [Header("Basic Stats")]
    public float maxHP;             //max health
    public float currHP;            //current health
    public float maxSpeed;          //move speed
    public float attackingCooldown; //How frequently the enemy can attack (makes it so the enemy can't do two attacks in a row)
    public float acceleration;      //How fast it gets to max speed
    public float visionRange;       //The radius at which the enemy detects the player
    public float rotationSpeed;     //How fast the enemy can rotate when tracking the player
    public float obstacleDetectRadius; //The radius at which the enemy detects obstacles. Used for avoiding obstacles when moving 

    [Header("Components")]
    public CharController player;      //Stores reference to player, in order to deal damage/otherwise affect them.
    public Transform playerTF;         //Player transform
    public Rigidbody playerRB;
    public Transform enemyTF;          //Enemy Transform
    public BoxCollider hitbox;         //The hitbox of the enemy
    public Rigidbody enemyRB;          //The enemy Rigidbody
    public LayerMask obstacleMask;     //The layer(s) that obstacles in the arena are on (includes other enemies)
    public LayerMask environmentMask;  //The layers the environment is on
    [HideInInspector] public GameObject[] players;
    public Renderer renderer;

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

        if(currHP == 0) {
            renderer.material.SetFloat("_Shader_Activation_Amount", 1.5f);
        }
        else if(currHP < maxHP / 2) {
            renderer.material.SetFloat("_Shader_Activation_Amount", 1f);
        }
        else if(currHP < maxHP) {

            renderer.material.SetFloat("_Shader_Activation_Amount", (currHP/maxHP) + 0.1f);
        }
    }

    public virtual void TakeDamage(float damage) { }

    public virtual void Die() { }

    public void SearchForPlayer() {
        if(players.Length == 0) {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
        foreach(GameObject p in players) {
            player = p.GetComponent<CharController>();
            playerTF = p.GetComponent<Transform>();
            playerRB = p.GetComponent<Rigidbody>();
        }

        if(player == null) {
            print("EnemyStats failed to locate Player");
        }
        else {
            print("EnemyStats located Player");
        }
    }
}
