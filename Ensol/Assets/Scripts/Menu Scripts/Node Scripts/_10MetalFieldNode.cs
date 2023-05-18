using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _10MetalFieldNode : MonoBehaviour
{
    [Header("Exitting Variables")]
    public PathCollider exitOnTriggerEnterEvent;
    private bool pathToComputer = false;


    private void Start()
    {
        CompletedNodes.prevNode = 10;
    }

    public void Update()
    {
        // if bottom gate was opened unlock node
        if (pathToComputer)
        {
            CompletedNodes.computerNode = true;
        }
    }

    //have to detect collider on another object
    void OnEnable(){
        exitOnTriggerEnterEvent.exitOnTriggerEnter.AddListener(ExitTriggerMethod);
    }

    void OnDisable(){
        exitOnTriggerEnterEvent.exitOnTriggerEnter.RemoveListener(ExitTriggerMethod);
    }

    void ExitTriggerMethod(Collider col){
        
        if (col.tag == "Player"){
            pathToComputer = true;
        }
        
    }
}
