using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meower : MonoBehaviour
{
    public Collider collider;
    public GameObject cat;
    public static bool meowAgain = true;
    private bool inMeowRange;
    //find e
    public KeyCode _key;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(_key) && inMeowRange && meowAgain){
            //play cat meow
            AudioManager.instance.PlayOneShot(FMODEvents.instance.catMeow, cat.transform.position);
        } else if (Input.GetKeyDown(_key) && inMeowRange && !meowAgain){
            ResetMeow();
        }
        
    }

    public void ResetMeow(){
        meowAgain = true;
    }

    public void OnTriggerEnter(Collider hitter){
        //trigger the dialogue
        if (hitter.tag == "Player"){
            inMeowRange = true;
        } 
    }

    public void OnTriggerExit(Collider hitter){
        //trigger the dialogue
        if (hitter.tag == "Player"){
            inMeowRange = false;
        } 
    }
}
