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
        currVial = 4; // full vials
        for(int i = 0; i < vials.Length; i += 1)
        {
            vials[i].SetActive(true);
        }
    }

    public void AddVial()
    {
        // if vials arent full yet
        if(currVial < 4)
        {
            currVial += 1;
            vials[currVial].SetActive(true);
        }
    }

    public void RemoveVials(int numVials)
    {
        // if vials arent empty yet
        if(currVial > 0)
        {
            // remove amount of vials (can vary per attack)
            for(int i = 0; i < numVials; i += 1)
            {
                vials[currVial].SetActive(false);
                currVial -= 1;
            }
        }
    }
}
