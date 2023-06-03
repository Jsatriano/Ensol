using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sys = System.IO;
using Ink.Runtime;

public class FileDataHandler
{
    private Story story;
    private TextAsset globals;

    

    private string dataDirPath = "";

    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName, TextAsset globals)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.globals = globals;
    }

    public PData Load()
    {

        string fullPath = Sys.Path.Combine(dataDirPath, dataFileName);

        PData loadedData = null;

        if (Sys.File.Exists(fullPath))
        {

            try
            {
                string dataToLoad = "";
                using (Sys.FileStream stream = new Sys.FileStream(fullPath, Sys.FileMode.Open))
                {
                    using (Sys.StreamReader reader = new Sys.StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<PData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.LogError("Erroroccured when trying to load data from file: " + fullPath + "\n" + e);
            }

        }
        return loadedData;

    }

    public void Save(PData data)
    {
        string fullPath = Sys.Path.Combine(dataDirPath, dataFileName);
        try
        {
            Sys.Directory.CreateDirectory(Sys.Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

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
            Debug.LogError("Erroroccured when trying to save data to file: " + fullPath + "\n" + e);
        }

    }

    public void SaveStory(TextAsset globals)
    {
         //Debug.Log("hello" + DialogueVariables.saveFile);
        story = new Story(globals.text);
        story.state.LoadJson(DialogueVariables.saveFile);


        string fullPath = Sys.Path.Combine(dataDirPath, "DialogueState.game");
        try
        {
            Sys.Directory.CreateDirectory(Sys.Path.GetDirectoryName(fullPath));

            string dataToStore = story.state.ToJson();
            //Debug.Log("hello" + dataToStore);

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

    public void LoadStory()
    {

        string fullPath = Sys.Path.Combine(dataDirPath, "DialogueState.game");

        //Story loadedStory;

        if (Sys.File.Exists(fullPath))
        {

            try
            {
                string storyToLoad = "";
                using (Sys.FileStream stream = new Sys.FileStream(fullPath, Sys.FileMode.Open))
                {
                    using (Sys.StreamReader reader = new Sys.StreamReader(stream))
                    {
                        storyToLoad = reader.ReadToEnd();
                    }
                }

                DialogueVariables.saveFile = storyToLoad;

            }
            catch (Exception e)
            {
                Debug.LogError("Erroroccured when trying to load data from file: " + fullPath + "\n" + e);
            }

        }
        //return loadedStory;

    }

    public void Delete()
    {
        string fullPath1 = Sys.Path.Combine(dataDirPath, dataFileName);
        string fullPath2 = Sys.Path.Combine(dataDirPath, "DialogueState.game");
        try
        {
            if (Sys.File.Exists(fullPath1))
            {
                Sys.File.Delete(fullPath1);
                Sys.File.Delete(fullPath2);
            }
            else 
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath1);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete save file at path: " + fullPath1);
        }
    }
}
