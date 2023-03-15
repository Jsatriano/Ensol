using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;


public class MusicController : MonoBehaviour
{
    private EventInstance zone1;
    private int song;
    public int nodeType;

    // Start is called before the first frame update
    void Start()
    {
        zone1 = AudioManager.instance.CreateEventInstance(FMODMusicEvents.instance.zone1);
        

        if(nodeType == 1){
            zone1.start();
            song = 1;
        }

    }

    void OnDestroy()
    {
        PLAYBACK_STATE ZplaybackState;
        zone1.getPlaybackState(out ZplaybackState);

        if(song == 1){
            zone1.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }


}
