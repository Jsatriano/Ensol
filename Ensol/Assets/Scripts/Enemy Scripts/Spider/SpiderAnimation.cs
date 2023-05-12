using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimation : MonoBehaviour
{
    public enum State
    {
        IDLE,
        MOVING_FORWARD,
        MOVING_LEFT,
        MOVING_RIGHT,
        WEB_SHOOT,
        TAZER_SHOOT,
        WEB_DEPLOY,
        DYING
    }

    [Header("References")]
    [SerializeField] private Animator animController;
    [SerializeField] private Rigidbody spiderRB;
    [SerializeField] SpiderBT spiderBT;
    [SerializeField] SpiderTazerManager tazerManager;
    private Transform playerTF;

    [Header("Misc")]
    private State state = State.IDLE;
    private float maxWalkSpeed, minWalkSpeed;

    private void FixedUpdate()
    {
        //Stops all animation once deer is dead
        if (state == State.DYING)
        {
            animController.SetBool("dying", true);
            return;
        }
        if (!spiderBT.isAlive)
        {
            state = State.DYING;
            animController.SetBool("dying", true);
            return;
        }

        if (playerTF == null && spiderBT != null && spiderBT.root.GetData("player") != null)
        {
            playerTF = (Transform)spiderBT.root.GetData("player");
            maxWalkSpeed = spiderBT.spiderStats.maxSpeed;
            minWalkSpeed = spiderBT.spiderStats.minSpeed;
        }

        string anim = (string)spiderBT.root.GetData("animation");
        switch (state)
        {
            case State.IDLE:
                if (anim == "movingForward")
                {                
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.spiderAlerted, this.transform.position);
                }
                break;

            case State.MOVING_FORWARD:
                //Sets the speed of the walking animation based on current velocity
                animController.SetFloat("animSpeed", Mathf.Clamp((spiderRB.velocity.magnitude * 10) / minWalkSpeed, 0.5f, 1));

                if (anim == "tazer")
                {
                    animController.SetTrigger("tazer");
                    state = State.TAZER_SHOOT;
                }
                else if (anim == "webDeploy")
                {
                    animController.SetTrigger("webDeploy");
                    state = State.WEB_DEPLOY;
                }
                else if (anim == "webShot")
                {
                    animController.SetTrigger("webShot");
                    state = State.WEB_SHOOT;
                }
                break;

            case State.TAZER_SHOOT:
                if (anim == "movingForward")
                {
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                }
                break;

            case State.WEB_DEPLOY:
                if (anim == "movingForward")
                {
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                }
                break;

            case State.WEB_SHOOT:
                if (anim == "movingForward")
                {
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                }
                break;

            case State.DYING:
                break;
        }
    }

    private void TazerShoot()
    {
        spiderBT.root.SetData("startTazer", true);
    }

    private void EndTazer()
    {
        spiderBT.root.SetData("tazerEnded", true);
    }

    private void DropWeb()
    {
        spiderBT.root.SetData("dropWeb", true);
    }

    private void EndWebDeploy()
    {
        spiderBT.root.SetData("webEnded", true);
    }

    private void ShootWeb()
    {
        spiderBT.root.SetData("shootWeb", true);
    }

    private void EndWebShot()
    {
        spiderBT.root.SetData("webShotEnded", true);
    }
}
