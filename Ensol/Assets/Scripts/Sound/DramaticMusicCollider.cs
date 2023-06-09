using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class DramaticMusicCollider : MonoBehaviour
{
    public bool musicPlaying = false; 
    private bool inRange = false;
    public KeyCode interactKey;
    public EventInstance zoneMusic;
    public EndingManager endMNG;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactKey) && inRange && !musicPlaying && endMNG.killMusic == false){
            musicPlaying = true;
            zoneMusic = AudioManager.instance.CreateEventInstance(FMODMusicEvents.instance.zoneMusic); 
            zoneMusic.start();
        }
        if (endMNG.killMusic){
            zoneMusic.stop(STOP_MODE.ALLOWFADEOUT);
            zoneMusic.release();
        }
        
    }

    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag == "Player"){
            inRange = true;
            //print("in range");
        }

    }

    void OnTriggerExit(Collider collider){
        if (collider.gameObject.tag == "Player"){
            inRange = false;
            //print("out of range");
        }
    }

    void OnDestroy()
    {
        zoneMusic.stop(STOP_MODE.ALLOWFADEOUT);
        zoneMusic.release();
    }
}
