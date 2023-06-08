using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoreKilling : MonoBehaviour
{
    public Collider killbox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            killbox.enabled = false;
        }
        
    }
}
