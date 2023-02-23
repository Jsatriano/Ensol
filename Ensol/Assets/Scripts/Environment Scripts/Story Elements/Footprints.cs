using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprints : MonoBehaviour
{
    public Collider c;
    // Update is called once per frame
    void Update()
    {
        if(c.enabled == false) {
            print("Boot footprints are visible in the mud on the bank of the river. They lead directly into the water, then reappear coming out on the other side. The water is moving too fast for you to attempt to cross it.");
        }
    }
}
