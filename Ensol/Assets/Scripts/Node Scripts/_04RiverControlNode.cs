using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;


public class _04RiverControlNode : MonoBehaviour
{
    public static bool riverOn = true;
    private EventInstance river;

    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    [Header("Control Tower Variables")]
    public Collider controlsCollider;
    public GameObject screen;
    public GameObject compDialogue;

    [Header("Water Variables")]
    public GameObject water;
    public GameObject[] waterBounds;
    public GameObject waterfalls;
    public float endY;
    public float speed;

    private void Awake() 
    {
        if (CompletedNodes.prevNode == 3)
        {
            SpawnPoint.First = true;
            SpawnPoint.Second = false;
        }
        else if (CompletedNodes.prevNode == 7)
        {
            SpawnPoint.First = false;
            SpawnPoint.Second = true;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        CompletedNodes.prevNode = 4;

        if (PlayerData.controlsHit == true){
            compDialogue.SetActive(false);
            // disables water colliders
            foreach (GameObject waterBound in waterBounds)
            {
                waterBound.SetActive(false);
            }
        }

    }

    private void Start()
    {
        CompletedNodes.firstLoad[4] = false;
    }

    public void Update()
    {
        // if player interacted with river control tower
        if(PlayerData.controlsHit && water.transform.position.y > endY)
        {  
            TurnOffWater();
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
            CompletedNodes.completedNodes[4] = true;
        }
    }

    public void TurnOffWater(){
        // removes highlight material from mesh
        screen.GetComponent<Renderer>().materials[1].SetFloat("_SetAlpha", 0f);

        //turns off waterfalls
        waterfalls.SetActive(false);
        riverOn = false;

        // moves water down to look like its draining
        water.transform.position = Vector3.Lerp(water.transform.position, 
                                    new Vector3(water.transform.position.x, water.transform.position.y - 1, water.transform.position.z), 
                                    speed * Time.deltaTime);

        // signals to other nodes that controls have been hit
        PlayerData.controlsHit = true;
    }
}
