using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    /*implementation plan:
    each checkpoint contains a dialogue menu which will allow travel to any other unlocked checkpoint.
    dialogue menu excludes the current checkpoint.
    each checkpoint is a prefab object, storing the node it is placed on, its model, bool for unlock status, and positional data.
    completednodes contains a static list of checkpoints. 
    when a checkpoint is interacted with:
    1) display dialogue options of available travel points, exclude self. also have option to cancel and close dialogue menu.
    2) on option selected, travel to that node. reload samplescene with the new node. 
    spawn player at the checkpoint location- can store a transform for spawn location in the checkpoint prefab.
    3) play the map scene animations erasing current node markers and circling the new node markers. 
    may need to adjust functionality for this to happen with non-adjacent nodes? ask justin.
    4) set relevant node data for current node. set the previous node to the prior node IN THE NODE MAP ORDER, not the node before the checkpoint. 
    are there any other node tracking vars that could get fucked here besides current node and previous node? ask justin.
    */

    public Transform spawnPoint;
    public int index;
    [HideInInspector] public bool active;
    private Collider col;
    private PauseMenu checkpointMenu;
    private GameObject checkpointDialogue;
    [SerializeField] private Renderer hatch;

    //KEY: INDEX - TARGET NODE
    // 0 - Cabin
    // 1 - Bear
    // 2 - Power Grid
    // 3 - Computer Exterior

    void Awake() {
        gameObject.tag = "Checkpoint";
    }
    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<Collider>();
        if(checkpointMenu == null) {
            SearchForCheckpointMenu();
        }

        active = CompletedNodes.checkpoints[index];
        //if this is the first checkpoint and no others are active, hide it
        if(index == 0 && !active) {
            gameObject.SetActive(false);
        }

        if(active) {
            hatch.materials[1].SetFloat("_SetAlpha", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(checkpointMenu == null) {
            SearchForCheckpointMenu();
        }
        if (checkpointDialogue != null && checkpointDialogue.activeInHierarchy){
            col.enabled = false;
        } else {
            col.enabled = true;
        }
    }

    public void ActivateCheckpoint(){
        active = true;
        //col.enabled = true;
        CompletedNodes.checkpoints[index] = active;
        if(!CompletedNodes.checkpoints[0]) {
            CompletedNodes.checkpoints[0] = true;
        }
        hatch.materials[1].SetFloat("_SetAlpha", 0);
    }

    public void UseActiveCheckpoint() {
        checkpointMenu.OpenCheckpointMenu();
        checkpointDialogue = GameObject.Find("CheckpointOptions");
        //col.enabled = false;

    }

    public void SearchForCheckpointMenu() {
        checkpointMenu = GameObject.Find("UI").GetComponent<PauseMenu>();
    }

}
