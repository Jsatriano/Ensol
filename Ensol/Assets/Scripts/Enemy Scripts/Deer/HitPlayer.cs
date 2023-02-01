using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    public DeerStats deerStats;

    void OnTriggerEnter(Collider col) // Justin
    {
        if(col.gameObject.tag == "Player") 
        {
            col.gameObject.GetComponent<PlayerCombatController>().TakeDamage(deerStats.attackPower);
        }
    }
}
