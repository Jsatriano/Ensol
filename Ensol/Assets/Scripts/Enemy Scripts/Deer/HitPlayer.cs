using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    public DeerBT deerBT;
    [SerializeField] private string attackName;
    [SerializeField] private float attackDamage;

    void OnTriggerEnter(Collider col) // Justin/Ryan
    {
        if(col.gameObject.tag == "Player") 
        {
            if (deerBT.root.GetData(attackName) != null)
            {
                col.gameObject.GetComponent<PlayerCombatController>().TakeDamage(attackDamage);
            } 
            return;
        }
    }
}
