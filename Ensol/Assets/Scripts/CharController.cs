using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{

    public enum State
    {
        IDLE,
        MOVING,
        DASHING
    }
    public State state;

    private Rigidbody _rb;

    Vector3 forward, right, direction, heading;
    Vector3 zeroVector = new Vector3(0, 0, 0); // empty vector (helps with checking if player is moving)

    [Header("References")]
    public BoxCollider PlayerBox;
    
    [Header("Movement Vaiables")]
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotationSpeed;

    [Header("Dashing Variables")]
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCD;
    private float _dashCdTimer;
    private bool _isDashing = false;

    

    // function is called in scene start
    private void Start()
    {
        state = State.IDLE;
        _rb = GetComponent<Rigidbody>();

        //freeze X and Z axis rotation
        //_rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        //_rb.constraints = RigidbodyConstraints.FreezeRotationX;
    }
    

    // Update is called once per frame
    void Update()
    {
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

        // give player i-frames
        PlayerBox.enabled = false;

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

        // apply force forwards of where player is facing
        _rb.AddForce(forceToApply, ForceMode.Impulse);

        // invoke RestDash function after dash is done
        Invoke(nameof(ResetDash), _dashDuration);
    }

    private void ResetDash() // Justin
    {
        // player isnt seen as dashing anymore
        _isDashing = false;

        // enable the player's hitbox again
        PlayerBox.enabled = true;
    }
}
