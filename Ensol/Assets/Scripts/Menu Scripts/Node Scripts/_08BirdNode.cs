using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _08BirdNode : MonoBehaviour
{
   private void Start()
    {
        CompletedNodes.prevNode = 8;
        CompletedNodes.firstLoad[8] = false;
    } 
}
