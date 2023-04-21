using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    void Awake() {
        gameObject.tag = "ShieldPickup";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerCombatController>().shieldIsActive = true;
            Destroy(gameObject);
        }
    }


}
