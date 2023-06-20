using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class BirdFlyAway : MonoBehaviour
{
    public GameObject flyRadius, flyTarget, bird;
    private Animator birdAnim;
    public EventInstance birdFlaps;
    public bool birdIsFlying = false;
    public int birdSpeed = 23;

    private bool trigger = false;

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
            if (trigger == false)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.birdSquawk, bird.transform.position);
                birdFlaps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.birdFly); 
                birdFlaps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(bird.gameObject));
                birdFlaps.start();
                trigger = true;
            }
            if(gameObject.transform.position != flyTarget.transform.position) {
                gameObject.transform.LookAt(flyTarget.transform);
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, flyTarget.transform.position, birdSpeed * Time.deltaTime);
            }
        }
        else {
            birdAnim.SetBool("isFlying", false);
        }
        
    }

    void OnDisable()
    {
        birdFlaps.stop(STOP_MODE.IMMEDIATE);
        birdFlaps.release();
    }

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.layer == 8) {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.birdDie, bird.transform.position);
            birdAnim.SetBool("isDying", true);
            flyRadius.SetActive(false);
            gameObject.GetComponent<Collider>().enabled = false;
            PlayerData.killedBird = true;
            print("Bird killed");
        }
    }
}
