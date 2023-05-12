using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathCollider : MonoBehaviour
{
    public UnityEvent<Collider> exitOnTriggerEnter;

    public void OnTriggerEnter(Collider col){
        
        if (exitOnTriggerEnter != null){
                exitOnTriggerEnter.Invoke(col);
        }
        
    }
}
