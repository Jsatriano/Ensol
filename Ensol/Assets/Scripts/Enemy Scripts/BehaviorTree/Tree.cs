using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class for behavior trees - RYAN

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        //Evaluates all nodes in the tree every update
        private void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
                //print(_root.Evaluate());
                if (_root.GetData("charging") == null)
                {
                    //print("NO");
                } else
                {
                    //print("YES");
                }
            }
        }
        protected abstract Node SetupTree();
    }
}
