using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;


public class GeneratorSoundManager : MonoBehaviour
{
    private EventInstance generator;
    private bool trigger = false;
    public int nodeType;

    private void Awake()
    {
        generator = AudioManager.instance.CreateEventInstance(FMODEvents.instance.generatorOn);
        generator.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
    }

    // Start is called before the first frame update
    void Start()
    {
        print("HELLO??");
        if (SceneManager.GetActiveScene().name == "GameplayScene")
        {
            nodeType = NodeSelector.selectedNode;
            if (nodeType == 9)
            {
                generator.start();
                print("I've started my jobbb!!!");
            }
        }
    }

    private void Update()
    {
        if (DialogueTrigger.triggeredPanel == true && trigger == false)
        {
            print("is this happening?");
            OnDestroy();
            trigger = true;
        }
    }

    void OnDestroy()
    {
        generator.stop(STOP_MODE.ALLOWFADEOUT);
        generator.release();
    }


}
