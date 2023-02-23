using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprints : MonoBehaviour
{
    public Collider c;
    public static GameObject First {get; private set;}
    void Start()
    {
        if (First == null) {
            First = GameObject.Find("First");
        }
        First.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(c.enabled == false) {
            //Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform); 
            First.SetActive(true);
            print("working");
            //StartCoroutine(ShowMessage("Boot footprints are visible in the mud on the bank of the river. They lead directly into the water, then reappear coming out on the other side. The water is moving too fast for you to attempt to cross it.", 1f));
        }
    }
}
