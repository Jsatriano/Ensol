using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public MeshRenderer buttonMesh;
    public Collider buttonCol;
    public GameObject requiredObj;
    public DoorController doorController;
    public Material greenMat;
    public List<GameObject> enemiesList = new List<GameObject>();
    private GameObject[] enemiesTotal;
    private int enemiesKilled;

    void Update()
    {
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
