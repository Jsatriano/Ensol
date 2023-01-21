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

    Vector3 forward, right, direction;
    Vector3 zeroVector = new Vector3(0, 0, 0);
    
    [Header("Movement Vaiables")]
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotationSpeed;

    // function is called in scene start
    private void Start()
    {
        state = State.IDLE;
    }
    

    // Update is called once per frame
    void Update()
    {
        // stores what inputs on the keyboard are being pressed in direction vector
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        print(state);

        switch (state)
        {
            case State.IDLE:
                // checks if player starts to move
                if(direction != zeroVector)
                {
                    state = State.MOVING;
                }
                break;
            
            case State.MOVING:
                Move();

                if(direction == zeroVector)
                {
                    state = State.IDLE;
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
            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

            // smoothly rotates player when changeing directions (rather than abruptly)
            Quaternion toRotation = Quaternion.LookRotation(heading, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);

            // makes our movement happen
            transform.position += heading * _moveSpeed * Time.deltaTime;
        }        
    }

}
