using System.Collections.Generic;
using System;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    private Story globalVariablesStory;
    public static string saveFile;
    
    public DialogueVariables(TextAsset loadGlobalsJSON)
    {
        globalVariablesStory = new Story(loadGlobalsJSON.text);
        // if (saveFile != "")
        // {
        //     globalVariablesStory.state.LoadJson(saveFile);
        // }


        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }

    public void SaveVariables()
    {
        if (globalVariablesStory != null)
        {
            VariablesToStory(globalVariablesStory);
            // saveFile = globalVariablesStory.state.ToJson();
        }
    }

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged; 
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged; 
    }


    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        //Debug.Log("Variable changed: " + name + " = " + value);
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
