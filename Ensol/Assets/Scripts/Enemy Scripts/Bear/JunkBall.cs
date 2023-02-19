using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkBall : MonoBehaviour
{
    public LayerMask enemyLayer;
    public LayerMask playerLayer;
    public SphereCollider junkCollider;
    [HideInInspector] public Transform bearTF;  //The bear that threw the junk ball
    [HideInInspector] public float junkDamage;  

    private void OnTriggerEnter(Collider other)
    {
        //Checks if the junkball hit an enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Ignores collision if it is with the bear that threw it
            if (other.transform == bearTF)
            {
                return;
            }
            //If it hits another enemy, damage that enemy
            else
            {
                other.GetComponent<EnemyStats>().TakeDamage(junkDamage);
            }
        }
        //Checks if the junkball hit the player
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Damage player
            other.GetComponent<PlayerCombatController>().TakeDamage(junkDamage, junkCollider);
        }
        //Destroy gameobject when it hits anything (not inlcuding the bear that threw it)
        Destroy(transform.gameObject);
    }
}
