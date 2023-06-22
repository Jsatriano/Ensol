using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using UnityEngine.Video;


public class CreditsController : MonoBehaviour
{
    public EventInstance zoneMusic;
    public GameObject screen;
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

        if ((videoPlayer.frame > 0) && (videoPlayer.isPlaying == false))
        {
            //Video has finshed playing!
            //videoPlayer.gameObject.SetActive(false);
            //screen.SetActive(false);
            SceneManager.LoadScene(sceneName:"MenuScene");
        }

    }

    public void BackToMenu()
    {
        AudioManager.instance.PlayOneShot(FMODButtonEvents.instance.envbeepboop, this.transform.position);
        SceneManager.LoadScene(sceneName:"MenuScene");
    }


}
