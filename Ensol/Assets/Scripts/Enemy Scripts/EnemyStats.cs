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
    public int speed = 5; //move speed
    [SerializeField] public string nameID; //a string name to ID the enemy
    [SerializeField] public int numID; //an int to id an enemy, if we want that too? maybe to differentiate units with the same name?

    //OTHER VARIABLES
    public CharController player; //Stores reference to player, in order to deal damage/otherwise affect them.


    // Start is called before the first frame update
    protected virtual void Start()
    {
        currHP = maxHP;
        gameObject.tag = "Enemy";
        nameID = "DefaultEnemy";
        numID = -1;
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Checks to see if the enemy is dead
        if(currHP <= 0) {
            print(nameID + " is dead!");
            Destroy(gameObject);
        }
    }
}
