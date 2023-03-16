using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;


public class MusicController : MonoBehaviour
{
    private EventInstance cabin;
    private EventInstance zone1;
    private EventInstance zone2;
    private EventInstance zone3;
    private int song;
    public int nodeType;

    // Start is called before the first frame update
    void Start()
    {
        nodeType = NodeSelector.selectedNode;
        cabin = AudioManager.instance.CreateEventInstance(FMODMusicEvents.instance.cabin);
        zone1 = AudioManager.instance.CreateEventInstance(FMODMusicEvents.instance.zone1);
        zone2 = AudioManager.instance.CreateEventInstance(FMODMusicEvents.instance.zone2);
        zone3 = AudioManager.instance.CreateEventInstance(FMODMusicEvents.instance.zone3);
        
        if(nodeType == 1){
            cabin.start();
            song = 0;
        } else if(nodeType > 1 && nodeType < 6){
            zone1.start();
            song = 1;
        } else if(nodeType > 5 && nodeType < 11){
            zone2.start();
            song = 2;
        } else if(nodeType > 10){
            zone3.start();
            song = 1;
        }

    }

    void OnDestroy()
    {
        //PLAYBACK_STATE ZplaybackState;
        //zone1.getPlaybackState(out ZplaybackState);

        if(song == 0){
            cabin.stop(STOP_MODE.ALLOWFADEOUT);
        } else if(song == 1){
            zone1.stop(STOP_MODE.ALLOWFADEOUT);
        } else if(song == 2){
            zone2.stop(STOP_MODE.ALLOWFADEOUT);
        } else if(song == 3){
            zone3.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }


}
