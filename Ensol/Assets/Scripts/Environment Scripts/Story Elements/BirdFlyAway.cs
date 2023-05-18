using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlyAway : MonoBehaviour
{
    public GameObject flyRadius, flyTarget, bird;
    private Animator birdAnim;
    public bool birdIsFlying = false;
    public int birdSpeed = 23;

    // Start is called before the first frame update
    void Start()
    {
        birdAnim = bird.GetComponent<Animator>();
        birdAnim.SetBool("isFlying", false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(birdIsFlying) {
            birdAnim.SetBool("isFlying", true);
            if(gameObject.transform.position != flyTarget.transform.position) {
                gameObject.transform.LookAt(flyTarget.transform);
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, flyTarget.transform.position, birdSpeed * Time.deltaTime);
            }
        }
        else {
            birdAnim.SetBool("isFlying", false);
        }
        
    }

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.layer == 8) {
            birdAnim.SetBool("isDying", true);
            flyRadius.SetActive(false);
            gameObject.GetComponent<Collider>().enabled = false;
            print("bird shot!");
        }
    }
}
