using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using UnityEngine.Video;


public class CreditsController : MonoBehaviour
{
    public EventInstance zoneMusic;
    public GameObject camera;
    [SerializeField] private VideoPlayer videoPlayer;
    public bool musicStart = false;


    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.Prepare();
        zoneMusic = AudioManager.instance.CreateEventInstance(FMODMusicEvents.instance.zoneMusic); 
        videoPlayer.Play();
    }

    void OnDestroy()
    {
        zoneMusic.stop(STOP_MODE.ALLOWFADEOUT);
        zoneMusic.release();
    }

    void Update()
    {
        if (videoPlayer.isPlaying && !musicStart){
            musicStart = true;
            zoneMusic.start(); 
        }
    }


}
