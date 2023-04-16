using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _04RiverControlNode : MonoBehaviour
{
    public static bool controlsHit = false;

    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    [Header("Control Tower Variables")]
    public Collider controlsCollider;
    public GameObject screen;

    [Header("Water Variables")]
    public GameObject water;
    public GameObject[] waterBounds;
    public GameObject waterfalls;
    public float endY;
    public float speed;

    private void Start()
    {
        CompletedNodes.prevNode = 4;
    }

    public void Update()
    {
        // if player interacted with river control tower
        if((controlsHit && water.transform.position.y > endY) || (controlsCollider.enabled == false && water.transform.position.y > endY))
        {  
            // removes highlight material from mesh
            screen.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);

            //turns off waterfalls
            waterfalls.SetActive(false);

            // moves water down to look like its draining
            water.transform.position = Vector3.Lerp(water.transform.position, 
                                       new Vector3(water.transform.position.x, water.transform.position.y - 1, water.transform.position.z), 
                                       speed * Time.deltaTime);

            // signals to other nodes that controls have been hit
            controlsHit = true;
        }
        // once water is gone, disables water colliders
        if(water.transform.position.y <= endY)
        {
            // disables water colliders
            foreach (GameObject waterBound in waterBounds)
            {
                waterBound.SetActive(false);
            }
        }

        if(electricGateController.opening)
        {
            CompletedNodes.securityTowerNode = true;
        }
    }
}
