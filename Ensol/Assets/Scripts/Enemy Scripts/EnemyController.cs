using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /* This will be the parent class for all enemy npcs. 
    Everything that is common to ALL ENEMY NPCS goes in this class.
    Specific enemies should then each have their own script which extends this class.
    Override stats/abilities/etc in that child class.
    -Elizabeth */

    //STATS
    [SerializeField] public int maxHP; //max health
    public int currHP; //current health
    [SerializeField] protected int attackPower; //used in damage calculations
    [SerializeField] protected int speed; //move speed
    [SerializeField] public string nameID; //a string name to ID the enemy
    [SerializeField] public int numID; //an int to id an enemy, if we want that too? maybe to differentiate units with the same name?

    //STATE MACHINE
    public enum State {
        IDLE,
        ATTACKING,
        MOVING
    }

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
        if(currHP > 0) {
            //Executes the enemy's behavior AI.
            //The behavior AI should be contained in the function called here rather than hardcoded into update.
            //This way, you can override the behavior AI function in child classes for each enemy's specific behavior.
            BehaviorAI();
        }
        else {
            //[play the model's death animation or whatever here, once that's available]
            Destroy(gameObject);
        }
        
    }

    //Behavior AI function to contain behavior trees. Override this in child classes to customize each enemy's behavior.
    protected virtual void BehaviorAI() {
        //override this function in child classes to write behavior for each enemy type.
    }
}
