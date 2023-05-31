using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class _03GateNode : MonoBehaviour
{
    [Header("Scripts")]
    public ElectricGateController electricGateController = null;

    private Story story;
    public TextAsset globals;

    private void Awake() 
    {
        //determine where to spawn
        if (CompletedNodes.prevNode == 0)
        {
            SpawnPoint.First = true;
        }
        else if (CompletedNodes.prevNode == 4)
        {
            SpawnPoint.First = false;
        }
        else 
        {
            SpawnPoint.First = SceneSwitch.exitFrom;
        }
        CompletedNodes.prevNode = 3;
    }

    private void Start()
    {
        //determine where to spawn part 2
        CompletedNodes.firstLoad[3] = false;

    }

    public void Update()
    {
        if (PlayerData.bunniesKilled >= 1)
        {
            story = new Story(globals.text);
            story.state.LoadJson(DialogueVariables.saveFile);
            story.EvaluateFunction("killedRabbit");
            DialogueVariables.saveFile = story.state.ToJson();
        }
        if(electricGateController.opening)
        {
            CompletedNodes.riverControlNode = true;
            CompletedNodes.completedNodes[3] = true;
        }
    }
}
