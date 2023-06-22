using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlyRadius : MonoBehaviour
{
    public BirdFlyAway bird;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player" && !PlayerData.killedBird) {
            gameObject.GetComponent<Collider>().enabled = false;
            bird.birdIsFlying = true;
        }
    }
}
