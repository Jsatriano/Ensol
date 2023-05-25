using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTriggerCheck : MonoBehaviour
{
    public GunPivotScript gunPivotScript;

    void OnTriggerEnter(Collider col)
    {
        gunPivotScript.foundPlayer = true;
    }
}
