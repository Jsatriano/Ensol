using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAnimation : MonoBehaviour
{

    public enum State
    {
        IDLE,
        MOVING_FORWARD,
        MOVING_LEFT,
        MOVING_RIGHT,
        CHARGE_WINDUP,
        CHARGING,
        SWIPING,
        DYING
    }

    [Header("Sound Effects")] // Harsha
    [SerializeField] private AudioSource chargingSoundEffect;
    [SerializeField] private AudioSource walkingSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;
    [SerializeField] private AudioSource attackSoundEffect;


    [SerializeField] private Animator animController;
    [SerializeField] private Transform headTF;   
    [SerializeField] private float lookingSpeed;
    private Transform playerTF;
    public DeerBT deerBT;

    private Vector3 previousDirection;
    private Vector3 _dirToPlayer;
    private Vector3 movingDir;
    private Vector3 deerRight;
    State state;
    State tempState;

    void Start()
    {
        state = State.IDLE;
    }

    void FixedUpdate()
    {
        //Stops all animation once deer is dead
        if (state == State.DYING)
        {
            return;
        }
        if (!deerBT.isAlive)
        {
            state = State.DYING;
            if (walkingSoundEffect.isPlaying == true) 
            {
                walkingSoundEffect.Stop();
            }
            else if (chargingSoundEffect.isPlaying == true)
            {
                chargingSoundEffect.Stop();
            }
            animController.SetTrigger("dying");
            deathSoundEffect.Play(); // plays when deer dies

            return;
        }

        if (playerTF == null && deerBT.root.GetData("player") != null)
        {
            playerTF = (Transform)deerBT.root.GetData("player");
        }

        tempState = WalkingAnimDir();
        switch (state)
        {
            case State.IDLE:
                //Deer is only in idle before it notices the player, so it transitions from Idle once
                //the behavior tree has seen the player
                if (deerBT.root.GetData("player") != null)
                {
                    //Transitions into movingForward if the deer isn't moving sideways
                    if (deerBT.root.GetData("lookingForward") != null)
                    {
                        deerBT.root.ClearData("lookingForward");
                        if (walkingSoundEffect.isPlaying == true) 
                        {
                            walkingSoundEffect.Stop();
                        }
                        animController.SetTrigger("movingForward");
                        walkingSoundEffect.Play(); // Plays when deer is walking
                        state = State.MOVING_FORWARD;
                        //print("movingForward");
                    }
                    //Picks whether it transitions into movingLeft or movingRight
                    else
                    {
                        state = tempState;
                        if (tempState == State.MOVING_LEFT)
                        {
                            
                            animController.SetTrigger("movingLeft");
                            if (walkingSoundEffect.isPlaying == false) // If walking sound effect is not playing, then plays this
                            {
                                walkingSoundEffect.Play();
                            }
                            //print("MovingLeft");
                        }
                        else
                        {
                            animController.SetTrigger("movingRight");
                            if (walkingSoundEffect.isPlaying == false) // If walking sound effect is not playing, then plays this
                            {
                                walkingSoundEffect.Play();
                            }
                            //print("MovingRight");
                        }
                    }
                }
                break;

            case State.MOVING_FORWARD:
                //Checks if the deer is entering the windup for its charge
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    if (walkingSoundEffect.isPlaying == true) 
                    {
                        walkingSoundEffect.Stop();
                    }
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                    //print("Charge Windup");
                }
                //Checks if the deer is starting a swiping attack
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    if (walkingSoundEffect.isPlaying == true) 
                    {
                        walkingSoundEffect.Stop();
                    }
                    animController.SetTrigger("swipe");
                    attackSoundEffect.Play(); // Plays when deer does a swiping attack
                    state = State.SWIPING;
                    //print("Swiping");
                }
                //Checks if the deer is now moving sideways
                else if (deerBT.root.GetData("lookingForward") == null)
                {
                    state = tempState;
                    if (state == State.MOVING_LEFT)
                    {
                        animController.SetTrigger("movingLeft");
                        if (walkingSoundEffect.isPlaying == false) // If walking sound effect is not playing, then plays this
                        {
                            walkingSoundEffect.Play();
                        }
                        //print("MovingLeft");
                    }
                    else
                    {
                        animController.SetTrigger("movingRight");
                        if (walkingSoundEffect.isPlaying == false) // If walking sound effect is not playing, then plays this
                        {
                            walkingSoundEffect.Play();
                        }
                        //print("MovingRight");
                    }
                }
                break;

            case State.MOVING_LEFT:
                //Checks if the deer is entering the windup for its charge
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    if (walkingSoundEffect.isPlaying == true)  // checks if walking sfx is playing and if it is, then it stops
                    {
                        walkingSoundEffect.Stop();
                    }
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                    //print("Charge Windup");
                }
                //Checks if the deer is starting a swiping attack
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    if (walkingSoundEffect.isPlaying == true) // checks if walking sfx is playing and if it is, then it stops
                    {
                        walkingSoundEffect.Stop();
                    }
                    animController.SetTrigger("swipe");
                    attackSoundEffect.Play(); // Plays swiping sound effect
                    state = State.SWIPING;
                    //print("Swiping");
                }
                //Checks if the deer is no longer moving sideways
                else if (deerBT.root.GetData("lookingForward") != null)
                {
                    deerBT.root.ClearData("lookingForward");
                    animController.SetTrigger("movingForward");
                    if (walkingSoundEffect.isPlaying == false) // if walking sfx not playing, then it plays it
                    {
                        walkingSoundEffect.Play();
                    }
                    state = State.MOVING_FORWARD;
                    //print("movingForward");
                }
                //Checks if the deer is now moving sideways in the other direction
                else if (tempState != state)
                {
                    animController.SetTrigger("movingRight");
                    if (walkingSoundEffect.isPlaying == false) // if walking sfx not playing, then it plays it
                    {
                        walkingSoundEffect.Play();
                    }
                    state = tempState;
                    //print("MovingRight");
                }
                break;

            case State.MOVING_RIGHT:
                //Checks if the deer is entering the windup for its charge
                if (deerBT.root.GetData("chargeWindupAnim") != null)
                {
                    if (walkingSoundEffect.isPlaying == true) // checks if walking sfx is playing and if it is, then it stops
                    {
                        walkingSoundEffect.Stop();
                    }
                    animController.SetTrigger("chargeWindup");
                    state = State.CHARGE_WINDUP;
                    //print("ChargeWindup");
                }
                //Checks if the deer is starting a swiping attack
                else if (deerBT.root.GetData("swipingAnim") != null)
                {
                    if (walkingSoundEffect.isPlaying == true) // checks if walking sfx is playing and if it is, then it stops
                    {
                        walkingSoundEffect.Stop();
                    }
                    animController.SetTrigger("swipe");
                    attackSoundEffect.Play();
                    state = State.SWIPING;
                    //print("Swipe");
                }
                //Checks if the deer is no longer moving sideways
                else if (deerBT.root.GetData("lookingForward") != null)
                {
                    deerBT.root.ClearData("lookingForward");
                    animController.SetTrigger("movingForward");
                    if (walkingSoundEffect.isPlaying == false) // if walking sfx not playing, then it plays it
                    {
                        walkingSoundEffect.Play();
                    }
                    state = State.MOVING_FORWARD;
                    //print("movingForward");
                }
                //Checks if the deer is now moving sideways in the other direction
                else if (tempState != state)
                {
                    animController.SetTrigger("movingLeft");
                    if (walkingSoundEffect.isPlaying == false) // if walking sfx not playing, then it plays it
                    {
                        walkingSoundEffect.Play();
                    }
                    state = tempState;
                    //print("MovingLeft");
                }
                break;

            case State.CHARGE_WINDUP:
                //Transitions to charging anim at the end of the charge windup anim from animation event
                return;

            case State.CHARGING:
                //Checks if the charge has ended
                if (deerBT.root.GetData("chargingAnim") == null)
                {
                    //Transitions into movingForward if the deer isn't moving sideways
                    if (deerBT.root.GetData("lookingForward") != null)
                    {
                        deerBT.root.ClearData("lookingForward");
                        animController.SetTrigger("movingForward");
                        if (chargingSoundEffect.isPlaying == true) 
                        {
                            chargingSoundEffect.Stop();
                        }
                        walkingSoundEffect.Play();
                        state = State.MOVING_FORWARD;
                        //print("movingForward");
                    }
                    //Picks whether it transitions into movingLeft or movingRight
                    else
                    {
                        state = tempState;
                        if (tempState == State.MOVING_LEFT)
                        {
                            animController.SetTrigger("movingLeft");
                            if (chargingSoundEffect.isPlaying == true) // checks if walking sfx is playing and if it is, then it stops
                            {
                                chargingSoundEffect.Stop();
                            }
                            walkingSoundEffect.Play();
                            //print("MovingLeft");
                        }
                        else
                        {
                            animController.SetTrigger("movingRight");
                            if (chargingSoundEffect.isPlaying == true) // checks if walking sfx is playing and if it is, then it stops
                            {
                                chargingSoundEffect.Stop();
                            }
                            walkingSoundEffect.Play();
                            //print("MovingRight");
                        }
                    }
                }
                break;

            case State.SWIPING:
                //Checks if the swiping attack has ended
                if (deerBT.root.GetData("swipingAnim") == null)
                {
                    //Transitions into movingForward if the deer isn't moving sideways
                    if (deerBT.root.GetData("lookingForward") != null)
                    {
                        deerBT.root.ClearData("lookingForward");
                        animController.SetTrigger("movingForward");
                        if (walkingSoundEffect.isPlaying == false) // if walking sfx not playing, then it plays it
                        {
                            walkingSoundEffect.Play();
                        }
                        state = State.MOVING_FORWARD;
                        //print("movingForward");
                    }
                    //Picks whether it transitions into movingLeft or movingRight
                    else
                    {
                        state = tempState;
                        if (tempState == State.MOVING_LEFT)
                        {
                            animController.SetTrigger("movingLeft");
                            if (walkingSoundEffect.isPlaying == false) // if walking sfx not playing, then it plays it
                            {
                                walkingSoundEffect.Play();
                            }
                            //print("MovingLeft");
                        }
                        else
                        {
                            animController.SetTrigger("movingRight");
                            if (walkingSoundEffect.isPlaying == false) // if walking sfx not playing, then it plays it
                            {
                                walkingSoundEffect.Play();
                            }
                            //print("MovingRight");
                        }
                    }
                }
                break;

            case State.DYING:
                //Don't update animations anymore when dead
                
                return;
        }
    }

    private void LateUpdate()
    {
        //Rotates deers head towards player when walking around
        if (state == State.MOVING_FORWARD || state == State.MOVING_LEFT || state == State.MOVING_RIGHT)
        {
            //Finds the direction to the player and then checks if that is greater than 90 degrees from the current head position
            _dirToPlayer = new Vector3(playerTF.position.x - headTF.position.x, 0, playerTF.position.z - headTF.position.z).normalized;
            if (Vector3.Dot(_dirToPlayer, transform.forward) < 0)
            {
                //If greater than 90 degrees, check if the dir to player is closer to the right or left of the deer (that will be the new rotate goal)
                if (Vector3.Dot(transform.right, _dirToPlayer) > Vector3.Dot(-transform.right, _dirToPlayer))
                {
                    _dirToPlayer = transform.right;
                }
                else
                {
                    _dirToPlayer = -transform.right;
                }
            }
            //Rotate deers head towards the player
            headTF.forward = Vector3.Lerp(previousDirection, _dirToPlayer, lookingSpeed * Time.deltaTime).normalized;        
        }
        previousDirection = headTF.forward;
    }

    private void StartCharge()
    {
        deerBT.root.SetData("chargingAnim", true);
        deerBT.root.ClearData("chargeWindupAnim");
        animController.SetTrigger("charging");
        chargingSoundEffect.Play(); // Plays charging sound effect
        state = State.CHARGING;
        return;
    }

    private void EndSwipeWindup()
    {
        deerBT.root.SetData("endSwipeWindup", true);
    }

    private void EndSwipe()
    {
        deerBT.root.SetData("endSwipe", true);
    }

    //Calculates whether the deer is moving to the left or right 
    private State WalkingAnimDir()
    {
        if (deerBT.root.GetData("deerRight") != null && deerBT.root.GetData("movingDir") != null)
        {
            movingDir = (Vector3) deerBT.root.GetData("movingDir");
            deerRight = (Vector3)deerBT.root.GetData("deerRight");
            if (Vector3.Dot(movingDir, deerRight) > 0)
            {
                return State.MOVING_LEFT;
            }
            else
            {
                return State.MOVING_RIGHT;
            }
        }
        else
        {
            return State.MOVING_LEFT;
        }
    }
}
