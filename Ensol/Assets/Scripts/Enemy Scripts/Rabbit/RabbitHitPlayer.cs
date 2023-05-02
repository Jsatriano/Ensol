using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitHitPlayer : MonoBehaviour
{
    public RabbitBT rabbitBT;
    private float attackDamage;
    [SerializeField] private Collider coll;
    [SerializeField] private bool tellBT;

    private void Start()
    {
          attackDamage = rabbitBT.rabbitStats.attackDamage;
    }

    void OnTriggerEnter(Collider col) // Justin/Ryan
    {
        if (col.gameObject.tag == "Player")
        {
            //Does damage to the player based on provided attack damage
            PlayerController combatController = col.gameObject.GetComponent<PlayerController>();
            if (combatController != null)
            {
                combatController.TakeDamage(attackDamage, coll);
                if (tellBT)
                {
                    rabbitBT.root.SetData("attackHit", true);
                }
            }
            return;
        }
    }
}
