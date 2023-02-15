using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerHitPlayer : MonoBehaviour
{
    public DeerBT deerBT;
    [SerializeField] private float attackDamage;
    [SerializeField] private Collider coll;
    [SerializeField] private bool tellBT;

    void OnTriggerEnter(Collider col) // Justin/Ryan
    {
        if(col.gameObject.tag == "Player") 
        {
            //Does damage to the player based on provided attack damage
            col.gameObject.GetComponent<PlayerCombatController>().TakeDamage(attackDamage, coll);
            if (tellBT)
            {
                deerBT.root.SetData("attackHit", true);
            }
            return;
        }
    }
}
