using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;


public class MusicController : MonoBehaviour
{
    private EventInstance zone1;
    public PlayerCombatController pcc;

    // Start is called before the first frame update
    void Start()
    {
        zone1 = AudioManager.instance.CreateEventInstance(FMODMusicEvents.instance.zone1);
        //zone1.start();
        //AudioManager.instance.PlayOneShot(FMODMusicEvents.instance.zone1, this.transform.position);

    }

    void Update()
    {
        PLAYBACK_STATE playbackState;
        zone1.getPlaybackState(out playbackState);

        if(playbackState.Equals(PLAYBACK_STATE.STOPPED) && pcc.currHP > 0){
            zone1.start();
        }
        if (pcc.currHP <= 0){
            zone1.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }


}
