using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sys = System.IO;
using Ink.Runtime;

public class DialogueSaver : MonoBehaviour
{
    public TextAsset globals;
    public Story story;
    [SerializeField] private string fileName;

    public void SaveStory()
    {
        //Debug.Log("hello" + DialogueVariables.saveFile);
        story = new Story(globals.text);
        story.state.LoadJson(DialogueVariables.saveFile);


        string fullPath = Sys.Path.Combine(Application.persistentDataPath, fileName);
        try
        {
            Sys.Directory.CreateDirectory(Sys.Path.GetDirectoryName(fullPath));

            string dataToStore = story.state.ToJson();
            Debug.Log("hello" + dataToStore);

            using (Sys.FileStream stream = new Sys.FileStream(fullPath, Sys.FileMode.Create))
            {
                using (Sys.StreamWriter writer = new Sys.StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Erroroccured when trying to save story to file: " + fullPath + "\n" + e);
        }
    }

    private void OnApplicationQuit()
    {
        SaveStory();
    }
}
