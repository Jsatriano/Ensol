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
        [HideInInspector] public GameObject[] players;

        protected void Start()
        {
            if(player == null) {
                SearchForPlayer();
            }
            root = SetupTree();
        }

        //Evaluates all nodes in the tree every update as long as the player is alive
        private void Update()
        {
            if(player == null) {
                SearchForPlayer();
            }
            if (root != null && player != null)
            {
                root.Evaluate();
            }
        }
        protected abstract Node SetupTree();

        
        /*
        private void OnDrawGizmos()
        {
            if (Application.isPlaying && root.GetData("move") != null)
            {
                
                Gizmos.color = Color.green;
                Vector3 move = (Vector3)root.GetData("move");
                Vector3 look = (Vector3)root.GetData("look");
                float dot = (float)root.GetData("dot");
                Gizmos.DrawRay(transform.position, move * 5);
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(transform.position, look * 5);
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
