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
            if (Application.isPlaying && root.GetData("d") != null)
            {
                
                Gizmos.color = Color.green;
                float[] charge = (float[]) root.GetData("charge");
                Vector3[] d = (Vector3[])root.GetData("d");
                for (int i = 0; i < 3; i++)
                {
                    print(charge[i]);
                    //Gizmos.DrawRay(transform.position, d[i] * charge[i] * 5);
                }
                print("------");
               
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
