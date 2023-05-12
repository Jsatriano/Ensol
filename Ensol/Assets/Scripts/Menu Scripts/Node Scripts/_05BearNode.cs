using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class _05BearNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    private Story story;
    public TextAsset globals;

    private void Start()
    {
        if (CompletedNodes.lNode == 4)
        {
            SpawnPoint.First = true;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        CompletedNodes.lNode = 5;

        CompletedNodes.prevNode = 5;
        CompletedNodes.firstLoad[5] = false;
    }

    public void Update()
    {
        if(electricGateController.opening)
        {
            if (PlayerData.bearsKilled == 1)
            {
                story = new Story(globals.text);
                story.state.LoadJson(DialogueVariables.saveFile);
                story.EvaluateFunction("killedBear");
                DialogueVariables.saveFile = story.state.ToJson();
            }
            CompletedNodes.brokenMachineNode = true;
        }
    }
}
