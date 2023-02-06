using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    public DeerBT deerBT;
    [SerializeField] private float attackDamage;
    [SerializeField] private Collider collider;

    void OnTriggerEnter(Collider col) // Justin/Ryan
    {
        if(col.gameObject.tag == "Player") 
        {
            //Does damage to the player based on provided attack damage
            col.gameObject.GetComponent<PlayerCombatController>().TakeDamage(attackDamage, collider);
            return;
        }
    }
}
