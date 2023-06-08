using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimation : MonoBehaviour
{
    public enum State
    {
        IDLE,
        MOVING_FORWARD,
        MOVING_BACKWARD,
        WEB_SHOOT,
        TAZER_SHOOT,
        WEB_DEPLOY,
        DYING
    }

    [Header("References")]
    [SerializeField] private Animator animController;
    [SerializeField] private Rigidbody spiderRB;
    [SerializeField] SpiderBT spiderBT;
    [SerializeField] private float miniSpeedMult;
    [SerializeField] private float miniTazerMult;
    [SerializeField] private bool isMiniSpider;
    private Transform playerTF;

    [Header("Misc")]
    public State state = State.IDLE;
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
        ResetAllTriggers();
        string anim = (string)spiderBT.root.GetData("animation");
        if (anim == null)
        {
            return;
        }
        switch (state)
        {
            case State.IDLE:
                if (anim == "movingForward")
                {                
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.spiderAlerted, this.transform.position);
                }
                else if (anim == "movingBackward")
                {
                    animController.SetTrigger("movingBackward");
                    state = State.MOVING_BACKWARD;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.spiderAlerted, this.transform.position);
                }
                break;

            case State.MOVING_FORWARD:
                //Sets the speed of the walking animation based on current velocity
                if (isMiniSpider)
                {
                    animController.SetFloat("animSpeed", Mathf.Clamp(((spiderRB.velocity.magnitude * 10) / minWalkSpeed) * miniSpeedMult, 1f, 2.5f));
                }
                else
                {
                    animController.SetFloat("animSpeed", Mathf.Clamp((spiderRB.velocity.magnitude * 10) / minWalkSpeed, 0.5f, 1f));
                }

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
                else if (anim == "movingBackward")
                {
                    animController.SetTrigger("movingBackward");
                    state = State.MOVING_BACKWARD;
                }
                break;

            case State.MOVING_BACKWARD:
                //Sets the speed of the walking animation based on current velocity
                if (isMiniSpider)
                {
                    animController.SetFloat("animSpeed", Mathf.Clamp(((spiderRB.velocity.magnitude * 10) / minWalkSpeed) * miniSpeedMult, 1f, 2.5f));
                }
                else
                {
                    animController.SetFloat("animSpeed", Mathf.Clamp((spiderRB.velocity.magnitude * 10) / minWalkSpeed, 0.5f, 1f));
                }

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
                else if (anim == "movingForward")
                {
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                }
                break;

            case State.TAZER_SHOOT:
                if (isMiniSpider)
                {
                    animController.SetFloat("animSpeed", miniTazerMult);
                }
                else
                {
                    animController.SetFloat("animSpeed", 1);
                }
                if (anim == "movingForward")
                {
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                }
                else if (anim == "movingBackward")
                {
                    animController.SetTrigger("movingBackward");
                    state = State.MOVING_BACKWARD;
                }
                break;

            case State.WEB_DEPLOY:
                if (anim == "movingForward")
                {
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                }
                else if (anim == "movingBackward")
                {
                    animController.SetTrigger("movingBackward");
                    state = State.MOVING_BACKWARD;
                }
                break;

            case State.WEB_SHOOT:
                if (anim == "movingForward")
                {
                    animController.SetTrigger("movingForward");
                    state = State.MOVING_FORWARD;
                }
                else if (anim == "movingBackward")
                {
                    animController.SetTrigger("movingBackward");
                    state = State.MOVING_BACKWARD;
                }
                break;

            case State.DYING:
                break;
        }
    }

    private void ResetAllTriggers()
    {
        animController.ResetTrigger("movingForward");
        animController.ResetTrigger("movingBackward");
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
