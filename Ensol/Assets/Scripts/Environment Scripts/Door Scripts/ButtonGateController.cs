using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ButtonGateController : MonoBehaviour
{
    public MeshRenderer buttonMesh;
    public Collider buttonCol;
    public GameObject requiredObj = null;
    public ElectricGateController gateController;
    public Material greenMat;
    public InteractText text;
    [SerializeField] private EnemyManager enemyManager = null;

    void Update()
    {
        // if there IS a pickup required to open gate
        if(requiredObj != null)
        {
            if(requiredObj.activeInHierarchy && AllEnemiesDefeated())
            {
                // turns button green
                Material[] matArray = buttonMesh.materials;
                matArray[1] = greenMat;
                buttonMesh.materials = matArray;
                text.canBeInteracted = true;
                // give correct tag
                gameObject.tag = "Interactable";

                if(!buttonCol.enabled)
                {
                    // opens door
                    gateController.OpenGate();
                }
            }
            else
            {
                // if button was pressed but other conditionals aren't met, turn collider back on
                gameObject.tag = "Uninteractable";
                buttonCol.enabled = true;
                text.canBeInteracted = false;
            }
        }
        // if there ISNT a pickup required to open gate
        else
        {
            if(AllEnemiesDefeated())
            {
                // turns button green
                Material[] matArray = buttonMesh.materials;
                matArray[1] = greenMat;
                buttonMesh.materials = matArray;
                text.canBeInteracted = true;
                gameObject.tag = "Interactable";

                if (!buttonCol.enabled)
                {
                    // opens door
                    gateController.OpenGate();
                }
            }
            else
            {
                // if button was pressed but other conditionals aren't met, turn collider back on
                gameObject.tag = "Uninteractable";
                buttonCol.enabled = true;
                text.canBeInteracted = false;
            }
        }
    }

    private bool AllEnemiesDefeated()
    {
        if (!enemyManager)
        {
            return true;
        }

        foreach (BT enemy in enemyManager.aliveEnemies)
        {
            if (enemy.isAlive)
            {
                return false;
            }
        }
        return true;
    }
}
