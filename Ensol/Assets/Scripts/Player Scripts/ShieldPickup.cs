using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class ShieldPickup : MonoBehaviour
{
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 posOffset = new Vector3 ();
    private Vector3 tempPos = new Vector3 ();
 

    void Awake() {
        gameObject.tag = "ShieldPickup";
    }
    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
         // Spin around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f));
 
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
        transform.position = tempPos;
        
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
