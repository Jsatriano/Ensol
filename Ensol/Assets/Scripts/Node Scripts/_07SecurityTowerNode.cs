using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

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

    // timer trigger
    private bool timerTrigger = false;

    private Story story;
    public TextAsset globals;

    [Header("Other Variables")]
    public float birdSpeed;

    private void Awake()
    {
        //determine where to spawn
        if (CompletedNodes.prevNode == 4)
        {
            SpawnPoint.First = true;
        } 
        else if (CompletedNodes.prevNode == 6)
        {
            SpawnPoint.First = false;
        }
        else if (CompletedNodes.prevNode == 9)
        {
            SpawnPoint.First = false;
            SpawnPoint.Second = false;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        CompletedNodes.prevNode = 7;
    }

    private void Start()
    {
        //determine where to spawn part 2
        CompletedNodes.firstLoad[7] = false;
    }

    public void Update()
    {
        // move bird if triggered
        if(PlayerData.birdTriggered == true && PlayerData.disableBird == false && birdTrigger.gameObject.activeInHierarchy)
        {
            //For the beep beep
            if (timerTrigger == false)
            {
                StartCoroutine(BeepBeep());
                timerTrigger = true;
            }
            // move it until it reaches end point
            if(bird.transform.position != birdEndPoint.position)
            {
                bird.transform.position = Vector3.MoveTowards(bird.transform.position, birdEndPoint.position, birdSpeed * Time.deltaTime);
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
            CompletedNodes.completedNodes[7] = true;
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

    public IEnumerator BeepBeep()
    {
        yield return new WaitForSeconds(1.0f);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.birdBeepBeep, bird.transform.position);
    }


}
