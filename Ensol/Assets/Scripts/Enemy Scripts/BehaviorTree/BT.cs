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
        [SerializeField] private bool useBreadcrumbs;
        private List<Vector3> playerBreadcrumbs = new List<Vector3>();
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
                if (useBreadcrumbs)
                {
                    ManageBreadcrumbs();
                }
                root.Evaluate();
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
                    playerBreadcrumbs.Add(player.transform.position);
                }
                //Creates a new breadcrumb when the player has moved far enough away from the previous one
                if (Vector3.Distance(player.transform.position, playerBreadcrumbs[playerBreadcrumbs.Count - 1]) > breadcrumbFreq)
                {
                    //Inserts a new breadcrumb, replacing an old one if at the max
                    if (playerBreadcrumbs.Count >= numBreadcrumbs)
                    {
                        playerBreadcrumbs.RemoveAt(0);
                    }
                    playerBreadcrumbs.Add(player.transform.position);
                    root.SetData("breadcrumbs", playerBreadcrumbs);
                }
            }
        }




        /*
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.red;;
                Gizmos.DrawWireSphere(target, .35f);


                Gizmos.color = Color.blue;
                for (int i = 0; i < playerBreadcrumbs.Count; i++)
                {
                    Gizmos.DrawWireSphere(playerBreadcrumbs[i], 0.25f);
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
