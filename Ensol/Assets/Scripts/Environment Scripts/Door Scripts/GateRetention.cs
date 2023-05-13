using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateRetention : MonoBehaviour
{
    public GameObject closedGate;
    public GameObject openGate;
    [Header("[0] = cabin\n[1] = deer\n[2] = river\n[3] = gate\n[4] = river control\n[5] = bear\n[6] = broken machine\n[7] = security tower\n[8] = bird\n[9] = power grid\n[10] = metal field\n[11] = computer")]
    public int nextNodeNumber;
    // Start is called before the first frame update
    void Start()
    {
        //get array of active nodes from CompletedNodes
        //check if completedNodesarray[nextNodeNumber] is true

        if (PlayerData.startedGame){
            if (CompletedNodes.nodes[nextNodeNumber]){
                openGate.SetActive(true);
                closedGate.SetActive(false);
            }
        }
    }

}
