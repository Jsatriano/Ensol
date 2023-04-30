using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _07SecurityTowerNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController gateTop = null;
    public ElectricGateController gateBottom = null;

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

        // if top gate was opened unlock node
        if(gateTop.opening)
        {
            CompletedNodes.brokenMachineNode = true;
        }
        // if bottom gate was opened unlock node
        if(gateBottom.opening)
        {
            CompletedNodes.powerGridNode = true;
        }
    }


}
