using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using FMOD.Studio;

public class _07SecurityTowerNode : MonoBehaviour
{
    [Header("Exitting Variables")]
    public PathCollider exitOnTriggerEnterEvent;
    public BirdTriggerCheck birdCollider;
    private bool pathToPowerGrid;

    [Header("Story Variables")]
    public GameObject bird;
    public Collider birdTrigger;
    public Transform birdEndPoint;
    public EventInstance birdFlaps;
    public GameObject gun;
    public Collider gunTrigger;
    public GameObject player;
    public bool beepsCollided = false;

    // timer trigger
    private bool timerTrigger = false;

    private Story story;
    public TextAsset globals;

    [Header("Other Variables")]
    public float birdSpeed;
    private Coroutine birdRoutine = null;

    private void Awake()
    {
        //determine where to spawn
        if (CompletedNodes.prevNode == 4)
        {
            SpawnPoint.First = true;
            SpawnPoint.Second = false;
        } 
        else if (CompletedNodes.prevNode == 6)
        {
            SpawnPoint.First = false;
            SpawnPoint.Second = true;
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

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        //determine where to spawn part 2
        CompletedNodes.firstLoad[7] = false;
    }

    public void Update()
    {
        // move bird if triggered
        if(beepsCollided == true && PlayerData.killedBird == false && birdTrigger.gameObject.activeInHierarchy)
        {
            print ("bird go");
            //For the beep beep
            if (timerTrigger == false)
            {
                print ("go go go");
                StartCoroutine(BeepBeep(bird));
                StartCoroutine(Squawk());
                timerTrigger = true;
                print ("timer is " + timerTrigger);
                
            }
            if (birdRoutine == null)
            {
                birdRoutine = StartCoroutine(MoveBird());
            }
        } else if (beepsCollided == true && PlayerData.hasTransponder){
            if (timerTrigger == false)
            {
                StartCoroutine(BeepBeep(player));
                timerTrigger = true;
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
        birdCollider.birdOnTriggerEnter.AddListener(BirdTriggerMethod);
    }

    void OnDisable(){
        exitOnTriggerEnterEvent.exitOnTriggerEnter.RemoveListener(ExitTriggerMethod);
        birdCollider.birdOnTriggerEnter.RemoveListener(BirdTriggerMethod);
    }

    void ExitTriggerMethod(Collider col){
        
        if (col.tag == "Player"){
            pathToPowerGrid = true;
        }
        
    }

    void BirdTriggerMethod(Collider col){
        if (col.tag == "Player"){
            beepsCollided = true;
        }
    }


    private IEnumerator MoveBird()
    {
        while (bird.transform.position != birdEndPoint.position)
        {
            Debug.Log("moving");
            bird.transform.position = Vector3.MoveTowards(bird.transform.position, birdEndPoint.position, birdSpeed * Time.deltaTime);
            yield return null;
        }

        birdTrigger.gameObject.SetActive(false);

        //disables bird for next visits to this node
        //PlayerData.disableBird = true;
    }

    public IEnumerator BeepBeep(GameObject beepPosition)
    {
        yield return new WaitForSeconds(1.2f);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.birdBeepBeep, beepPosition.transform.position);
        //boopboop here
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.boopboop, gun.transform.position);
        PlayerData.birdTriggered = false;
    }

    public IEnumerator Squawk()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.birdSquawk, bird.transform.position);
        birdFlaps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.birdFly); 
        birdFlaps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(bird.gameObject));
        birdFlaps.start();
    }


}
