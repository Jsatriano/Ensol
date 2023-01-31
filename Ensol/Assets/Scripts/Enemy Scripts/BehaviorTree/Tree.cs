using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class for behavior trees - RYAN

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        public GameObject player;

        protected void Start()
        {
            _root = SetupTree();
        }

        //Evaluates all nodes in the tree every update as long as the player is alive
        private void Update()
        {
            if (_root != null && player != null)
            {
                _root.Evaluate();
            }
        }
        protected abstract Node SetupTree();

        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying && _root.GetData("dir") != null)
            {
                
                Gizmos.color = Color.green;
                Vector3 dir = (Vector3)_root.GetData("dir");
                Gizmos.DrawRay(transform.position, dir * 5);
            }
        }
    }
}
