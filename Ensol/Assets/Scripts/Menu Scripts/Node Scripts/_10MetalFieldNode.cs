using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _10MetalFieldNode : MonoBehaviour
{
    [Header("Exitting Variables")]
    public PathCollider exitOnTriggerEnterEvent;
    private bool pathToComputer = false;

    public GameObject teslaCoil01;
    public GameObject teslaCoil02;
    public GameObject teslaCoil03;


    private void Start()
    {
        CompletedNodes.prevNode = 10;
        CompletedNodes.firstLoad[10] = false;
    }

    public void Update()
    {
        // if bottom gate was opened unlock node
        if (pathToComputer)
        {
            CompletedNodes.computerNode = true;
            CompletedNodes.completedNodes[10] = true;
        }

        // first generator hit check
        if(PlayerData.firstGenHit)
        {
            teslaCoil01.SetActive(false);
        }

        // second generator hit check
        if(PlayerData.secondGenHit)
        {
            teslaCoil02.SetActive(false);
        }

        // third generator hit check
        if(PlayerData.thirdGenHit)
        {
            teslaCoil03.SetActive(false);
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
