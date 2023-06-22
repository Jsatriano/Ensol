using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player") {
            gameObject.GetComponent<Collider>().enabled = false;
            PlayerData.hasShield = false;
            col.gameObject.GetComponent<PlayerController>().TakeDamage(PlayerData.currHP + 1f, gameObject.transform.position);
        }
    }
}
