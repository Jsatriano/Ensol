using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricVials : MonoBehaviour // justin
{

    public GameObject[] vials;

    public int currVial;
    // Start is called before the first frame update
    void Start()
    {
        currVial = 2; // full vials
        for(int i = 0; i < vials.Length; i += 1)
        {
            vials[i].SetActive(true);
        }
    }

    public void AddVial()
    {
        // if vials arent full yet
        if(currVial < 2)
        {
            currVial += 1;
            vials[currVial].SetActive(true);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.hudBatteryCharge, this.transform.position);
        }
    }

    public void RemoveVials(int numVials)
    {
        // if vials arent empty yet
        if(currVial > -1)
        {
            
            // remove amount of vials (can vary per attack)
            for(int i = 0; i < numVials; i += 1)
            {
                vials[currVial].SetActive(false);
                currVial -= 1;
            }
            print(currVial);
        }
    }
}
