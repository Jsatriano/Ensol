using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;


public class MusicController : MonoBehaviour
{
    public EventInstance zoneMusic;

    // Start is called before the first frame update
    void Start()
    {
        zoneMusic = AudioManager.instance.CreateEventInstance(FMODMusicEvents.instance.zoneMusic); 
        zoneMusic.start(); 
    }

    void OnDestroy()
    {
        zoneMusic.stop(STOP_MODE.ALLOWFADEOUT);
        zoneMusic.release();
    }


}
