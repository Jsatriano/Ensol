using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _09PowerGridNode : MonoBehaviour
{
    private void Start()
    {
        CompletedNodes.prevNode = 9;
        CompletedNodes.firstLoad[9] = false;
    }
}
