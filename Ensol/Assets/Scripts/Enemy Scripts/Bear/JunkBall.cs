using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkBall : MonoBehaviour
{
    public SphereCollider junkCollider;  //Collider for the junk ball
    public SphereCollider explosionCollider;  //Collider for the explosion
    [HideInInspector] public Transform bearTF;  //The bear that threw the junk ball
    [HideInInspector] public float junkDamage;  //Damage for the junk ball
    [HideInInspector] public float explosionDamage;  //Damage for the explosion
    [HideInInspector] public float explosionLength;  //How long the explosion lasts
    private float explosionTimer;  //Used to keep track of explosion time
    private bool isExploding;      //Used to keep track of if the ball is exploding
    private float interpolater;    //Used to smoothly scale up explosion size
    public Vector3 explosionStartingSize;  //The size the explosion starts at
    [HideInInspector] public Vector3 explosionFinalSize;  //How big the explosion gets
    public Transform explosionTF;  //Transform of the explosion
    private Rigidbody ballRB;      //Rigidbody of the junk ball
    public GameObject ballModel;   //Visual model for the junk ball
    private Vector3 bearPos;
    private bool explodedAudio = false;
    private Coroutine explosionRoutine;

    private void Start()
    {
        isExploding = false;
        ballRB = gameObject.GetComponent<Rigidbody>();
        bearPos = bearTF.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Checks if the junkball hit an enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Ignores collision if it is with the bear that threw it
            if (other.transform == bearTF || other.tag == "Sound")
            {
                return;
            }
            //If it hits another enemy, damage that enemy
            else
            {
                EnemyStats enemyScript = other.GetComponent<EnemyStats>();
                if (enemyScript != null)
                {
                    if (isExploding)
                    {
                        enemyScript.TakeDamage(explosionDamage);
                    }
                    else
                    {
                        enemyScript.TakeDamage(junkDamage);
                        Explode();
                    }
                }
                          
            }
        }
        //Checks if the junkball hit the player
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerScript = other.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                //Damage player
                if (isExploding)
                {
                    playerScript.TakeDamage(explosionDamage, bearPos);
                }
                else
                {
                    playerScript.TakeDamage(junkDamage, bearPos);
                    Explode();
                }
            }
        }
            
        else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle") || other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            Explode();
        }     
    }

    //Explodes the ball
    private void Explode()
    {
        if (explosionRoutine == null)
        {
            if (explodedAudio == false)
            {
                explodedAudio = true;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.bearScrapExplosion, this.transform.position);
            }
            Destroy(junkCollider);
            ballModel.SetActive(false);
            explosionTF.gameObject.SetActive(true);
            explosionTF.localScale = explosionStartingSize;
            isExploding = true;
            explosionRoutine = StartCoroutine(ExplosionRoutine());

        }   
    }

    private IEnumerator ExplosionRoutine()
    {

        yield return null;
        explosionCollider.enabled = false;

        ballRB.velocity = Vector3.zero;
        while (explosionTimer < explosionLength)
        {
            //Uses sin with the Lerp so that it slows down as it reaches the end
            interpolater = explosionTimer / explosionLength;
            interpolater = Mathf.Sin(Mathf.PI * interpolater * 0.5f) * 2f;

            explosionTF.localScale = Vector3.Lerp(explosionStartingSize, explosionFinalSize, interpolater);
            explosionTimer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}

