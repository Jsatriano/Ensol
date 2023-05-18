using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _07SecurityTowerNode : MonoBehaviour
{
    [Header("Exitting Variables")]
    public PathCollider exitOnTriggerEnterEvent;
    private bool pathToPowerGrid;

    [Header("Story Variables")]
    public GameObject bird;
    public Collider birdTrigger;
    public Transform birdEndPoint;
    public GameObject gun;
    public Collider gunTrigger;

    [Header("Other Variables")]
    public float birdSpeed;


    private void Start()
    {
        CompletedNodes.prevNode = 7;
    }

    public void Update()
    {
        if(PlayerData.birdTriggered == true && PlayerData.disableBird == false && birdTrigger.gameObject.activeInHierarchy)
        {
            if(bird.transform.position != birdEndPoint.position)
            {
                bird.transform.position = Vector3.MoveTowards(bird.transform.position, birdEndPoint.position, birdSpeed * Time.deltaTime);

                //Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, points[current].position, speed * Time.deltaTime);
            }
            else
            {
                birdTrigger.gameObject.SetActive(false);

                //disables bird for next visits to this node
                PlayerData.disableBird = true;
            }
        }

        
        // if bottom gate was opened unlock node
        if (pathToPowerGrid)
        {
            CompletedNodes.powerGridNode = true;
        }
    }

    //have to detect collider on another object
    void OnEnable(){
        exitOnTriggerEnterEvent.exitOnTriggerEnter.AddListener(ExitTriggerMethod);
    }

    void OnDisable(){
        exitOnTriggerEnterEvent.exitOnTriggerEnter.RemoveListener(ExitTriggerMethod);
    }

    void ExitTriggerMethod(Collider col){
        
        if (col.tag == "Player"){
            pathToPowerGrid = true;
        }
        
    }


}
