using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTriggerCheck : MonoBehaviour
{
    public GunPivotScript gunPivotScript;
    public GameObject exitEdgeCaseOff;

    void OnTriggerEnter(Collider col)
    {
        if(!PlayerData.hasTransponder) {
            gunPivotScript.foundPlayer = true;
            exitEdgeCaseOff.SetActive(false);
        }
    }
}
