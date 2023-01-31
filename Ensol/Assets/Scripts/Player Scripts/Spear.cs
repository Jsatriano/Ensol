using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public CharController charController;
    public PlayerCombatController combatController;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Spear";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponent<EnemyStats>().currHP -= combatController.attackPower;
            print("Did " + combatController.attackPower + " damage to " + col.gameObject.GetComponent<EnemyStats>().nameID);
        }
    }
}