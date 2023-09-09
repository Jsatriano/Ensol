using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

// JUSTIN
public class ButtonDoorController : MonoBehaviour
{
    public MeshRenderer buttonMesh;
    public Collider buttonCol;
    public GameObject requiredObj = null;
    public DoorController doorController;
    public Material greenMat;
    public List<GameObject> enemiesList = new List<GameObject>();
    private GameObject[] enemiesTotal;
    private int enemiesKilled;
    public InteractText text;
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
                text.canBeInteracted = true;
                if(!buttonCol.enabled)
                {
                    // opens door
                    doorController.OpenDoor();
                    AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
                } 
            }
            else
            {
                // if button was pressed but conditionals aren't met, turn collider back on
                buttonCol.enabled = true;
                text.canBeInteracted = false;
            }
        }
        else
        {
            // if there are no enemies left alive
            if(enemiesList.Count == 0)
            {
                // turns button green
                buttonMesh.material = greenMat;
                text.canBeInteracted = true;
                if (cabin == false)
                {
                    if(!buttonCol.enabled)
                    {
                        // opens door
                        doorController.OpenDoor();
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
                        }
                    }
                }           
            }
            else
            {
                // if button was pressed but conditionals aren't met, turn collider back on
                buttonCol.enabled = true;
                text.canBeInteracted = false;
            }
        }
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
