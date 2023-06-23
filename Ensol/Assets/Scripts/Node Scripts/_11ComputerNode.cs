using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class _11ComputerNode : MonoBehaviour
{
    public TextAsset exteriorCutscene;
    public GameObject enemyWave;
    public GameObject redLight;
    public GameObject farChargeAudio;
    public EventInstance alarmSound;
    public float enemyWaveSpeed = 15;
    private bool seenCutsceneDialogue;
    private DialogueManager dialogueManager;
    private PlayerController player;
    private bool playingAlarm = false;

    private void Start()
    {
        SpawnPoint.First = true;
        CompletedNodes.prevNode = 11;
        PlayerData.currentNode = 11; //this is not redundant, it is here for player loading into this node if they exited last on computer node
        redLight.SetActive(false);
        enemyWave.SetActive(false);
        dialogueManager = DialogueManager.GetInstance();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update(){
        if(player == null) {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
        if(seenCutsceneDialogue && player.state != PlayerController.State.DIALOGUE) {
            enemyWave.SetActive(true);
        }

        if(enemyWave.activeInHierarchy) {
            enemyWave.transform.position += new Vector3(0, 0, (enemyWaveSpeed * Time.deltaTime));
        }
    }

    public void StartCutsceneDialogue() {
        seenCutsceneDialogue = true;
        dialogueManager.EnterDialogueMode(exteriorCutscene);
        redLight.SetActive(true);
        farChargeAudio.SetActive(true);
        if (!playingAlarm){
            playingAlarm = true;
            alarmSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.ensolAlarm);
            alarmSound.start();
        }
    }

    void OnDestroy()
    {
        alarmSound.stop(STOP_MODE.ALLOWFADEOUT);
        alarmSound.release();
    }
}
