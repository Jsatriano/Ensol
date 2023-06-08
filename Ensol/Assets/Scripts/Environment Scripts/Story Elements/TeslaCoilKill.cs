using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoilKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            PlayerData.hasShield = false;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerShieldBreak, damageVFXLocation.transform.position);
            PlayerData.currHP = 0;
        }
    }
}
