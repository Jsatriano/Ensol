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

    private void Awake()
    {
        //determine where to spawn
        if (CompletedNodes.prevNode == 2)
        {
            SpawnPoint.First = true;
        } 
        else if (CompletedNodes.prevNode == 6)
        {
            SpawnPoint.First = false;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        CompletedNodes.prevNode = 5;
    }

    private void Start()
    {
        //determine where to spawn part 2
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
            CompletedNodes.completedNodes[5] = true;
        }
    }
}
