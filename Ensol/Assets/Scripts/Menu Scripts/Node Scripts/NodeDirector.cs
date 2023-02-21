using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDirector : MonoBehaviour
{
    public _01CabinNode _01cabinNode;

    [HideInInspector] public int node;
    // Start is called before the first frame update
    void Start()
    {
        node = NodeSelector.selectedNode; 
    }

    public void DirectToNodeScript()
    {
        print(node);
        if(node == 1)
        {
            _01cabinNode.CompletedLevel();
        }
    }
}
