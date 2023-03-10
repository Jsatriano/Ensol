using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerHitPlayer : MonoBehaviour
{
    public DeerBT deerBT;
    public float attackDamage;
    [SerializeField] private Collider coll;
    [SerializeField] private bool tellBT;
    [SerializeField] private bool swipeAttack;
    [SerializeField] private bool chargeAttack;

    private void Start()
    {
        if (swipeAttack)
        {
            attackDamage = deerBT.deerStats.basicDamage;
        }
        if (chargeAttack)
        {
            attackDamage = deerBT.deerStats.chargeDamage;
        }
    }

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
        else if (col.gameObject.layer == LayerMask.NameToLayer("Enemy") || col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if (tellBT)
            {
                deerBT.root.SetData("attackHit", true);
            }
        }
    }
}
