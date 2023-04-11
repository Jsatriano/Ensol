using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class for behavior trees - RYAN

namespace BehaviorTree
{
    public abstract class BT : MonoBehaviour
    {
        public Node root = null;
        public GameObject player;
        private List<Vector3> playerBreadcrumbs = new List<Vector3>();
        private List<GameObject> test = new List<GameObject>();
        [SerializeField] private GameObject breadcrumb;
        [SerializeField] private float breadcrumbFreq;
        [SerializeField] private float numBreadcrumbs;
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
                ManageBreadcrumbs();
                root.Evaluate();
            }
            if (root.GetData("AYO") != null)
            {
                root.ClearData("AYO");
                print("AYO");
            }

        }
        protected abstract Node SetupTree();

        private void ManageBreadcrumbs()
        {
            if (root.GetData("player") != null)
            {
                //Makes sure there is always at least 1 breadcrumb
                if (playerBreadcrumbs.Count <= 0)
                {
                    GameObject newCrumb = Instantiate(breadcrumb, player.transform.position, player.transform.rotation);
                    test.Add(newCrumb);
                    playerBreadcrumbs.Add(player.transform.position);
                }
                //Creates a new breadcrumb when the player has moved far enough away from the previous one
                if (Vector3.Distance(player.transform.position, playerBreadcrumbs[playerBreadcrumbs.Count - 1]) > breadcrumbFreq)
                {
                    //Inserts a new breadcrumb, replacing an old one if at the max
                    if (playerBreadcrumbs.Count >= numBreadcrumbs)
                    {
                        playerBreadcrumbs.RemoveAt(0);
                        Destroy(test[0]);
                        test.RemoveAt(0);
                    }
                    GameObject newCrumb = Instantiate(breadcrumb, player.transform.position, player.transform.rotation);
                    test.Add(newCrumb);
                    playerBreadcrumbs.Add(player.transform.position);
                    root.SetData("breadcrumbs", playerBreadcrumbs);
                }
            }
        }


        /*
        private void OnDrawGizmos()
        {
            if (Application.isPlaying && root.GetData("obstacles") != null && root.GetData("playerWeights") != null)
            {
                
               
                //float[] move = (float[])root.GetData("final");
                float[] obstacle = (float[])root.GetData("obstacles");
                float[] pWeights = (float[])root.GetData("playerWeights");
                

                //Vector3 deerRight = (Vector3)root.GetData("deerRight");
                Gizmos.color = Color.green;
                //print("--------");
                for (int i = 0; i < pWeights.Length; i++)
                {

                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * pWeights[i] * 3);
                }
                Gizmos.color = Color.red;
                for (int i = 0; i < obstacle.Length; i++)
                {
                    //print(i + ": " + obstacle[i]);
                   Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * obstacle[i] * 3);
                    //print(obstacle[i]);
                }
                
                
                Vector3 moveDir = (Vector3)root.GetData("movingDir");
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, moveDir * 5);
               
                float[] pWeights = (float[])root.GetData("obstacles");
                Gizmos.color = Color.green;
                for (int i = 0; i < pWeights.Length; i++)
                {

                   Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * pWeights[i] * 3);
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
