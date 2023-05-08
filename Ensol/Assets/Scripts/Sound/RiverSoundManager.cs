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

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            nodeType = NodeSelector.selectedNode;
            river = AudioManager.instance.CreateEventInstance(FMODEvents.instance.envRiver);

            if (nodeType == 2 || nodeType == 4)
            {
                river.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
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
