using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    public DeerStats deerStats;
    public PlayerCombatController pcc;

    void OnTriggerEnter(Collider col) 
    {
        if(col.gameObject.tag == "Player") 
        {
            pcc.TakeDamage(deerStats.attackPower);
        }
    }
}
