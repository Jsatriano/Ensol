using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamIntegration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try {
            Steamworks.SteamClient.Init(2437800);
            PrintYourName();
        }
        catch (System.Exception e){
            //something went wrong - it's one of these:
            //  Steam is closed?
            //  Can't find Steam_api dll?
            //  Don't have permission to play app?
            Debug.Log(e);

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }

    private void PrintYourName()
    {
        Debug.Log(Steamworks.SteamClient.Name);
    }

    void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

    public void ClearAchievement(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();
        Debug.Log($"Achievement {id} cleared");
    }

    public void IsUnlocked(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        Debug.Log($"Achievement {id} status:" + ach.State);
    }
}
