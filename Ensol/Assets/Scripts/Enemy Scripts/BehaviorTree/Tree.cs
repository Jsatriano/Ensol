using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class for behavior trees - RYAN

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        public Node root = null;
        public GameObject player;
        public bool isAlive; //Determines whether the behavior tree should be running/Animations should be changing
        [HideInInspector] public GameObject[] players;

        protected void Start()
        {
            isAlive = true;
            if(player == null) {
                SearchForPlayer();
            }
            root = SetupTree();
        }

        //Evaluates all nodes in the tree every update as long as the player is alive
        private void FixedUpdate()
        {
            if(player == null) {
                SearchForPlayer();
            }
            if (root != null && player != null && isAlive)
            {
                root.Evaluate();
            }
        }
        protected abstract Node SetupTree();

        
        /*
        private void OnDrawGizmos()
        {
            if (Application.isPlaying && root.GetData("movingDir") != null)
            {
                
                Gizmos.color = Color.green;
                Vector3 move = (Vector3)root.GetData("movingDir");
                Vector3 deerRight = (Vector3)root.GetData("deerRight");
                float dot = (float)root.GetData("dot");
                Gizmos.DrawRay(transform.position, move * 5);
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(transform.position, deerRight.normalized * 5);
                
                if (root.GetData("diagonal") != null)
                {
                    print("Diagonal");
                    root.ClearData("diagonal");
                }
                else if (root.GetData("straight") != null)
                {
                    print("straight");
                    root.ClearData("straight");
                }
                
            }
        }
        */
        
        

        //automatically find the player gameobject instead of putting it in the editor - Elizabeth
        public void SearchForPlayer() {
            if(players.Length == 0) {
            players = GameObject.FindGameObjectsWithTag("Player");
            }
            foreach(GameObject p in players) {
                player = p;
            }
            if(player == null) {
                print("BT failed to locate Player");
            }
            else {
                print("BT located Player");
            }
        }
    }
}
