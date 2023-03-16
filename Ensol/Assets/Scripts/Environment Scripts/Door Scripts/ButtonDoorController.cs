using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class ButtonDoorController : MonoBehaviour
{
    public MeshRenderer buttonMesh;
    public Collider buttonCol;
    public GameObject requiredObj = null;
    public DoorController doorController;
    //GameObject dialogueController;// = GameObject.FindWithTag("DialogueManager");
    public Material greenMat;
    public List<GameObject> enemiesList = new List<GameObject>();
    private GameObject[] enemiesTotal;
    private int enemiesKilled;
    public GateTutorialText text;
    public bool cabin = false;

    public TextAsset InkDoorStory;
    public Story door;

    void Start()
    {
        if (InkDoorStory != null)
        {
            door = new Story(InkDoorStory.text);
        }
    }

    void Update()
    {
        if(requiredObj != null)
        {
            if(!requiredObj.activeInHierarchy && enemiesList.Count == 0)
            {
                // turns button green
                buttonMesh.material = greenMat;
                if(!buttonCol.enabled)
                {
                    // opens door
                    doorController.OpenDoor();
                    AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
                    text.opened = true;
                } 
                
                
            }
            else
            {
                // if button was pressed but conditionals aren't met, turn collider back on
                buttonCol.enabled = true;
            }
        }
        else
        {
            if(enemiesList.Count == 0)
            {
                // turns button green
                buttonMesh.material = greenMat;
                if (cabin == false)
                {
                    if(!buttonCol.enabled)
                    {
                        // opens door
                        doorController.OpenDoor();
                        text.opened = true;
                    }
                }
                else 
                {
                    if (DialogueManager.GetInstance().openSesame == true)
                    {
                        if(!buttonCol.enabled)
                        {
                            // opens door
                            doorController.OpenDoor();
                            text.opened = true;
                        }
                    }
                }
                
            }
            else
            {
                // if button was pressed but conditionals aren't met, turn collider back on
                buttonCol.enabled = true;
            }
        }
        /*
        // opens door if button is pressed AND required obj is picked up AND there are no enemies left
        if(!buttonCol.enabled && !requiredObj.activeInHierarchy && enemiesList.Count == 0)
        {
            // turns button green
            buttonMesh.material = greenMat;

            // opens door
            doorController.OpenDoor();
        }
        else
        {
            // if button was pressed but other conditionals aren't met, turn collider back on
            buttonCol.enabled = true;
        }
        */
    }

    // called by DeerStats when an enemy dies
    public void enemyKilled(GameObject enemy)
    {
        if(enemiesList.Contains(enemy))
        {
            enemiesList.Remove(enemy);
        }
    }
}
