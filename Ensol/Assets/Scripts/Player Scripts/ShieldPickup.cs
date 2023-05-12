using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class ShieldPickup : MonoBehaviour
{
    //public static EventInstance playerShieldOn;

    void Awake() {
        gameObject.tag = "ShieldPickup";
    }
    // Start is called before the first frame update
    void Start()
    {
        //playerShieldOn = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerShieldOn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player" && !PlayerData.hasShield) {
            //playerShieldOn.start();
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerShieldOn, this.transform.position);
            //print("played sound!");
            PlayerData.hasShield = true;
            Destroy(gameObject);
        }
    }


}
