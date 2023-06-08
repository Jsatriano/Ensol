using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShot : MonoBehaviour
{
    [HideInInspector] public Transform spiderTF;
    [HideInInspector] public float speedDebuff;
    [HideInInspector] public float debuffLength;
    [HideInInspector] public float webDamage;
    public GameObject dustVFX;
    private GameObject activeDustVFX;

    private void OnTriggerEnter(Collider other)
    {
        //Checks if the web hit an enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Ignores collision if it is the spider that threw it
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
                    enemyScript.TakeDamage(webDamage);
                    activeDustVFX = Instantiate(dustVFX, gameObject.transform.position, gameObject.transform.rotation);
                    StartCoroutine(DeleteWeb());
                    gameObject.SetActive(false);
                }
            }
        }
        //Checks if the web hit the player
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerScript = other.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                activeDustVFX = Instantiate(dustVFX, gameObject.transform.position, gameObject.transform.rotation);
                playerScript.TakeDamage(webDamage, spiderTF.position);
                playerScript.ApplySpeedChange(speedDebuff, debuffLength);
                StartCoroutine(DeleteWeb());
                gameObject.SetActive(false);
            }
        }
        else if (other.isTrigger == false)
        {
            activeDustVFX = Instantiate(dustVFX, gameObject.transform.position, gameObject.transform.rotation);
            StartCoroutine(DeleteWeb());
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DeleteWeb() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        Destroy(activeDustVFX);
    }

}
