using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueAchievement : MonoBehaviour
{
    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player") {
            var ach = new Steamworks.Data.Achievement("The_Boss_Battle_That_Never_Was");
            ach.Trigger();
        }
    }
}
