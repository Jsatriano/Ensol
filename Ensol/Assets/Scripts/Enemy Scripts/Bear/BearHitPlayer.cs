using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearHitPlayer : MonoBehaviour
{
    public BearBT bearBT;
    [HideInInspector] public float attackDamage;
    [SerializeField] private Collider coll;
    [SerializeField] private bool tellBT;
    [SerializeField] private bool swipeAttack;
    [SerializeField] private bool junkAttack;

    private void Start()
    {
        //Checking what bear attack it is
        if (swipeAttack)
        {
            attackDamage = bearBT.bearStats.swipeDamage;
        }
        if (junkAttack)
        {
            attackDamage = bearBT.bearStats.junkDamage;
        }
    }

    void OnTriggerEnter(Collider col) // Justin/Ryan
    {
        if (col.gameObject.tag == "Player")
        {
            //Does damage to the player based on provided attack damage
            col.gameObject.GetComponent<PlayerCombatController>().TakeDamage(attackDamage, coll);
            if (tellBT)
            {
                bearBT.root.SetData("attackHit", true);
            }
            return;
        }
    }
}
