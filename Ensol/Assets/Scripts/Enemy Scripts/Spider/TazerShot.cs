using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TazerShot : MonoBehaviour
{
    [HideInInspector] public Transform spiderTF;
    [HideInInspector] public float tazerDamage;
    [SerializeField] private SphereCollider tazerCollider;

    private void OnTriggerEnter(Collider other)
    {
        //Checks if the tazer bolt hit an enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Ignores collision if it is with the spider that threw it
            if (other.transform == spiderTF || other.tag == "Sound")
            {
                return;
            }
            //If it hits another enemy, damage that enemy
            else
            {
                EnemyStats enemyScript = other.GetComponent<EnemyStats>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(tazerDamage);
                    Destroy(gameObject);
                }
            }
        }
        //Checks if the tazer bolt hit the player
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerScript = other.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(tazerDamage, tazerCollider);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
