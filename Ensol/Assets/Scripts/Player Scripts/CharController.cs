using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{

    public enum State
    {
        IDLE,
        MOVING,
        DASHING,
        ATTACKING
    }
    public State state;

    private Rigidbody _rb;

    public Vector3 forward, right, direction, heading;
    Vector3 zeroVector = new Vector3(0, 0, 0); // empty vector (helps with checking if player is moving)

    [Header("Other Vaiables")]
    public GameObject mouseFollower;
    public bool attacking = false;
    private bool controller = false;

    
    [Header("Movement Vaiables")]
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotationSpeed;

    [Header("Dashing Variables")]
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCD;
    private float _dashCdTimer;
    private bool _isDashing = false;
    public bool canTakeDmg;

    

    // function is called in scene start
    private void Start()
    {
        state = State.IDLE;
        _rb = GetComponent<Rigidbody>();
        gameObject.tag = "Player";
        canTakeDmg = true;
    }
    

    // Update is called once per frame
    void Update()
    {
        // determines if controller is connected, removes cursor if one is
        if(Input.GetJoystickNames().Length <= 0)
        {
            Cursor.visible = false;
            controller = true;
        }
        // stores what inputs on the keyboard are being pressed in direction vector
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        

        // count down dash timer
        if(_dashCdTimer > 0)
        {
            _dashCdTimer -= Time.deltaTime;
        }

        switch (state)
        {
            case State.IDLE:
                attacking = false;
                // checks if player starts to move
                if(direction != zeroVector)
                {
                    state = State.MOVING;
                }
                // if player hits space, dash
                else if(Input.GetButtonDown("Dash"))
                {
                    state = State.DASHING;
                }
                break;
            
            case State.MOVING:
                Move();

                // if player stops moving, go idle
                if(direction == zeroVector)
                {
                    state = State.IDLE;
                }

                // if player hits space, dash
                else if(Input.GetButtonDown("Dash"))
                {
                    state = State.DASHING;
                }
                break;

            case State.DASHING:
                // make player dash if CD is done
                if(_dashCdTimer <= 0)
                {
                    print(state);
                    Dash();
                }

                // after the dash is done, change states
                if(!_isDashing)
                {
                    print("dash done");   
                    if(direction == zeroVector)
                    {
                        state = State.IDLE;
                    }
                    else if(direction != zeroVector)
                    {
                        state = State.MOVING;
                    }
                }
                break;
            case State.ATTACKING:
                //We will have to decide if the player can move or take other actions while attacking.
                //This state is just to tell this script that the player is attacking, so
                //hold other state changes. Attack combos will be handled in PlayerCombatController.
                //Since there is probably going to be a lot of combat code, I put it in a different script.

                // stop ability to rotate player when attacking | only active if on keyboard
                if(!attacking && !controller)
                {
                // Turns player towards mouse for attack
                Vector3 toMouse = (mouseFollower.transform.position - transform.position);
                transform.forward = new Vector3(toMouse.x, 0, toMouse.z);
                }
                attacking = true;
                break;
        }
    }

    void Move() // Justin
    {
         // forward vector is going to equal our camera's forward vector 
        forward = Camera.main.transform.forward;
        forward.y = 0;

        // keeps length of vector to 1 so we can use it for motion
        forward = Vector3.Normalize(forward);

        // creates a rotation for our vector
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        // checks if player is moving
        if(direction.magnitude > 0.1f)
        {
            // says what our right movement is going to be ( + / - ) depending on what horiz key is being pressed
            Vector3 rightMovement = right * _moveSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal");

            // says what our up movement is going to be ( + / - ) depending on what vert key is being pressed
            Vector3 upMovement = forward * _moveSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");

            // combines both movements to create a direction that our character will point to
            heading = Vector3.Normalize(rightMovement + upMovement);

            // smoothly rotates player when changeing directions (rather than abruptly)
            Quaternion toRotation = Quaternion.LookRotation(heading, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);

            // makes our movement happen
            transform.position += heading * _moveSpeed * Time.deltaTime;
        }        
    }


    private void Dash() // Justin
    {
        // return func if dash is still on CD | else dash is successful, start CD until dahs is available again
        if(_dashCdTimer > 0) return;
        else _dashCdTimer = _dashCD;

        // player is now seen as dashing
        _isDashing = true;

        // turn on i-frames
        canTakeDmg = false;

        // find out how much force to apply to player (also check if player is moving or not)
        Vector3 forceToApply;
        if(direction == zeroVector)
        {
            forceToApply = transform.forward * _dashForce;
        }
        else 
        {
            forceToApply = heading * _dashForce;
        }

        // increase drag and apply force forwards of where player is facing
        _rb.drag = 0;
        _rb.AddForce(forceToApply, ForceMode.Impulse);


        // invoke RestDash function after dash is done
        Invoke(nameof(ResetDash), _dashDuration);
    }

    private void ResetDash() // Justin
    {
        // reset drag
        _rb.drag = 20;

        // player isnt seen as dashing anymore
        _isDashing = false;

        // turn off i-frames
        canTakeDmg = true;
    }
}