using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;


public class RiverSoundManager : MonoBehaviour
{
    private EventInstance river;
    private int song;
    public int nodeType;

    private void Awake()
    {
        river = AudioManager.instance.CreateEventInstance(FMODEvents.instance.envRiver);
        river.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            nodeType = NodeSelector.selectedNode;
            if (nodeType == 3 || nodeType == 5)
            {
                river.start();
                song = 0;
            }
        }
    }

    void OnDestroy()
    {
        if (song == 0)
        {
            river.stop(STOP_MODE.ALLOWFADEOUT);
            river.release();
        }
    }


}
