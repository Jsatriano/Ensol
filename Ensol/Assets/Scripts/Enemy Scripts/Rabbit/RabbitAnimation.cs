using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class RabbitAnimation : MonoBehaviour
{
    public enum State
    {
        SITTING,
        LEAPING,
        DYING
    }

    [SerializeField] private Animator animController;
    public RabbitBT rabbitBT;
    private State state;
    private Transform playerTF;
    private EventInstance attackSound;

    private void Start()
    {
        state = State.SITTING;
    }

    private void FixedUpdate()
    {
        //Used to check when the rabbit enters agro
        if (playerTF == null && rabbitBT.root.GetData("player") != null)
        {
            playerTF = (Transform)rabbitBT.root.GetData("player");
        }

        switch (state)
        {
            case State.SITTING:
                //Exits idle once rabbit has seen player
                if (playerTF != null)
                {
                    animController.SetBool("Sitting", false);
                    animController.SetBool("Leaping", true);
                    state = State.LEAPING;
                    attackSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.bunnyAttack);
                    attackSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
                    attackSound.start();
                }
                break;

            case State.LEAPING:
                animController.SetFloat("AnimSpeed", 1);
                //Checks if rabbit has died
                if (!rabbitBT.isAlive)
                {
                    animController.SetBool("Leaping", false);
                    animController.SetBool("Dying", true);
                    state = State.DYING;
                    return;
                }
                break;

            case State.DYING:
                //Do nothing once rabbit is dead
                attackSound.stop(STOP_MODE.IMMEDIATE);
                return;
        }
    }

    //Called when the rabbits hind legs push off the ground. Signals to the rabbit's BT to apply force to the rabbit
    private void PushOffGround()
    {
        rabbitBT.root.SetData("feetOnGround", true);
        rabbitBT.root.ClearData("applyLandingDrag");
        //AudioManager.instance.PlayOneShot(FMODEvents.instance.rabbitMove, this.transform.position);
    }

    //Called when the rabbits hind legs leave the ground. Signals to stop accelerating the rabbit
    private void StopPushingOffGround()
    {
        rabbitBT.root.SetData("feetOnGround", false);
    }

    private void ApplyLandingDrag()
    {
        rabbitBT.root.SetData("applyLandingDrag", true);
    }

    private void IncrementLeaps()
    {
        rabbitBT.root.SetData("incrementLeaps", true);
    }

    void Update()
    {
        attackSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
    }

}
