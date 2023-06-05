using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meower : MonoBehaviour
{
    private bool inRange;
    public KeyCode interactKey;
    private GameObject meower;
    // Start is called before the first frame update
    void Start()
    {
        meower = GameObject.FindGameObjectWithTag("Meower");
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("Interact") || Input.GetMouseButtonDown(0) || Input.GetKeyDown(interactKey)) && inRange){
            print("try to meow");
            StartCoroutine(DelayedMeow());
        }
        
    }

    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag == "Player"){
            inRange = true;
            print("in range");
        }

    }

    void OnTriggerExit(Collider collider){
        if (collider.gameObject.tag == "Player"){
            inRange = false;
            print("out of range");
        }
    }

    IEnumerator DelayedMeow(){
        print("waiting to meow");
        yield return new WaitForSeconds(0.32f);
        if (DialogueManager.GetInstance().donePlaying == false){
            print("meow");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.catMeow, meower.transform.position);
        }
    }
}
