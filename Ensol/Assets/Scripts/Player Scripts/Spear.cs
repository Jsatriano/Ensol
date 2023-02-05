using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public CharController charController;
    public PlayerCombatController combatController;
    private bool isTriggered = false;
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
        if (isTriggered == false) {
            if(col.gameObject.tag == "Enemy") {
                col.gameObject.GetComponent<EnemyStats>().currHP -= combatController.attackPower;
                print("Did " + combatController.attackPower + " damage to " + col.gameObject.GetComponent<EnemyStats>().nameID);
            }
        }
    }

    void onTriggerExit(Collider col) {
        isTriggered = false;
    }
}
