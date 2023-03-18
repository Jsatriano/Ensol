using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGateController : MonoBehaviour
{
    public MeshRenderer buttonMesh;
    public Collider buttonCol;
    public GameObject requiredObj = null;
    public ElectricGateController gateController;
    public Material greenMat;
    public List<GameObject> enemiesList = new List<GameObject>();
    private GameObject[] enemiesTotal;
    private int enemiesKilled;
    public InteractTutorialText text;

    void Update()
    {
        // if there IS a pickup required to open gate
        if(requiredObj != null)
        {
            if(requiredObj.activeInHierarchy && enemiesList.Count == 0)
            {
                // turns button green
                buttonMesh.material = greenMat;
                text.canBeInteracted = true;

                if(!buttonCol.enabled)
                {
                    // opens door
                    gateController.OpenGate();
                }
            }
            else
            {
                // if button was pressed but other conditionals aren't met, turn collider back on
                buttonCol.enabled = true;
                text.canBeInteracted = false;
            }
        }
        // if there ISNT a pickup required to open gate
        else
        {
            if(enemiesList.Count == 0)
            {
                // turns button green
                buttonMesh.material = greenMat;
                text.canBeInteracted = true;

                if (!buttonCol.enabled)
                {
                    // opens door
                    gateController.OpenGate();
                }
            }
            else
            {
                // if button was pressed but other conditionals aren't met, turn collider back on
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
