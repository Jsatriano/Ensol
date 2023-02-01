using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    public DeerStats deerStats;
    public DeerBT deerBT;

    void OnTriggerEnter(Collider col) // Justin
    {
        if(col.gameObject.tag == "Player") 
        {
            if (deerBT.root.GetData("charging") != null)
            {
                col.gameObject.GetComponent<PlayerCombatController>().TakeDamage(deerStats.attackPower);
            } 
            else if (deerBT.root.GetData("attacking") != null)
            {
                col.gameObject.GetComponent<PlayerCombatController>().TakeDamage(deerStats.attackPower);
            }
            else
            {
                return;
            }
        }
    }
}
