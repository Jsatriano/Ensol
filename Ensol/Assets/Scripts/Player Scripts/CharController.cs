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
        ATTACKING,
        KNOCKBACK,
        PAUSED,
        DEAD,
        DIALOGUE
    }
    public State state;

    private Rigidbody _rb;

    public Vector3 forward, right, direction, heading;
    [HideInInspector] public Vector3 zeroVector = new Vector3(0, 0, 0); // empty vector (helps with checking if player is moving)

    [Header("References & Other Variables")]
    public GameObject mouseFollower;
    public GameObject pauseMenu;
    public Animator animator;
    public PlayerCombatController pcc;
    public bool attacking = false;
    public bool controller = false;
    private State prevState;
    [HideInInspector] public bool knockback;
    public float knockbackForce;
    
    
    [Header("Movement Vaiables")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _acceleration;
    public float normalDrag;
    [SerializeField] private float _rotationSpeed;
    private bool _isGrounded;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;

    [Header("Dashing Variables")]
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCD;
    private float _dashCdTimer;
    private bool _isDashing = false;


    // function is called in scene start
    private void Start()
    {
        Cursor.visible = false;
        state = State.IDLE;
        _rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameObject.tag = "Player";
        print(gameObject.tag);
        knockback = false;
        _rb.drag = normalDrag;
    }

    IEnumerator CheckforControllers() // Justin
    {
        while (true)
        {
            // determines if controller is connected, removes cursor if one is
            var controllers = Input.GetJoystickNames();
            if(controllers.Length > 0)
            {
                if(!controller && Input.GetJoystickNames()[0].Length > 0) // controller is connected
                {
                    //Cursor.visible = false;
                    controller = true;
                    print("Connected");
                    
                }
                else if (controller && Input.GetJoystickNames()[0].Length <= 0) // controller is disconnected
                {
                    //Cursor.visible = true;
                    controller = false;
                    print("Disconnected");
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void Awake() // Justin
    {
        gameObject.tag = "Player";
        StartCoroutine(CheckforControllers());
    }

    private void FixedUpdate()
    {
        //Runs Move() in fixedUpdate so that physics are consistent
        if (state == State.MOVING)
        {
            Move();
        }
    }


    // Update is called once per frame
    void Update()
    {
        // stores what inputs on the keyboard are being pressed in direction vector
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //Updates players drag based on whether they are in the air or on the ground
        _isGrounded = Physics.CheckSphere(_groundCheck.position, .1f, _groundMask);
        ControlDrag();

        // count down dash timer
        if (_dashCdTimer > 0)
        {
            _dashCdTimer -= Time.deltaTime;
        }

        // in case animations dont finish, prevents bugs
        if(state != State.ATTACKING)
        {
            pcc.lightSlashVFX[0].SetActive(false);
            pcc.lightSlashVFX[1].SetActive(false);
            pcc.lightSlashVFX[2].SetActive(false);
            pcc.heavySlashVFX.SetActive(false);
        }

        switch (state)
        {
            case State.IDLE:
                // animations
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);

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
                else if(Input.GetButtonDown("Cancel"))
                {
                    prevState = State.IDLE;
                    state = State.PAUSED;
                }
                break;
            
            case State.MOVING:
                // animations
                animator.SetBool("isRunning", true);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);
                
                attacking = false;



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
                else if(Input.GetButtonDown("Cancel"))
                {
                    prevState = State.MOVING;
                    state = State.PAUSED;

                }
                break;

            case State.DASHING:
                // make player dash if CD is done
                if(_dashCdTimer <= 0)
                {
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isDashing", true);
                    animator.SetBool("isHeavyAttacking", false);
                    animator.SetInteger("lightAttackCombo", 0);
                    Dash();
                }

                // after the dash is done, change states
                if(!_isDashing)
                {   
                    if(direction == zeroVector)
                    {
                        state = State.IDLE;
                    }
                    else if(direction != zeroVector)
                    {
                        state = State.MOVING;
                    }
                    else if(Input.GetButtonDown("Cancel"))
                    {
                        prevState = State.DASHING;
                        state = State.PAUSED;
                    }
                }
                break;

            case State.ATTACKING:
                //We will have to decide if the player can move or take other actions while attacking.
                //This state is just to tell this script that the player is attacking, so
                //hold other state changes. Attack combos will be handled in PlayerCombatController.
                //Since there is probably going to be a lot of combat code, I put it in a different script. -Elizabeth
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                

                if(Input.GetButtonDown("Cancel"))
                {
                    prevState = State.ATTACKING;
                    state = State.PAUSED;
                }

                // stop ability to rotate player when attacking | only active if on keyboard
                if(!attacking && !controller)
                {
                // Turns player towards mouse for attack
                Vector3 toMouse = (mouseFollower.transform.position - transform.position);
                transform.forward = new Vector3(toMouse.x, 0, toMouse.z);
                }
                attacking = true;
                break;

            case State.KNOCKBACK:
                if(pcc.isMidGrab) {
                    knockback = false;
                    state = State.ATTACKING;
                    print("Prioritized weapon catch over knockback");
                }
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);
                print(knockback);


                // once knockback is over, go to idle state
                if(knockback == false)
                {
                    state = State.IDLE;
                }
                // if paused during knockback, save state so game doesnt break
                else if(Input.GetButtonDown("Cancel"))
                {
                    prevState = State.KNOCKBACK;
                    state = State.PAUSED;
                }
                break;
                
            case State.PAUSED:
                Cursor.visible = true;
                // pause game, make all actions unavailable
                if(!pauseMenu.activeInHierarchy)
                {
                    Cursor.visible = false;
                    state = prevState;
                }
                break;

            case State.DEAD:
                //print("Player is Dead");
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);
                Cursor.visible = true;
                break;
                
            case State.DIALOGUE:
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);
                Cursor.visible = true;
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed);

            // makes our movement happen

            _rb.AddForce(heading * _acceleration, ForceMode.Acceleration);
            Vector3 velocityXZ = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);           
            if (velocityXZ.magnitude > _moveSpeed)
            {
                Vector3 velocityY = new Vector3(0, _rb.velocity.y, 0);
                velocityXZ = Vector3.ClampMagnitude(velocityXZ, _moveSpeed);
                _rb.velocity = velocityXZ + velocityY;
            }
        }        
    }


    private void Dash() // Justin
    {
        // return func if dash is still on CD | else dash is successful, start CD until dahs is available again
        if(_dashCdTimer > 0) return;
        else _dashCdTimer = _dashCD;

        // player is now seen as dashing
        _isDashing = true;
        _rb.velocity = Vector3.zero;

        //sfx
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerDodge, this.transform.position);

        // find out how much force to apply to player (also check if player is moving or not)
        Vector3 forceToApply;
        if(direction == zeroVector)
        {
            forceToApply = transform.forward * _dashForce;
        }
        else 
        {
            if(heading != transform.forward) {
                transform.forward = heading;
            }
            forceToApply = heading * _dashForce;
        }


        // increase drag and apply force forwards of where player is facing
        _rb.drag = 0;
        _rb.AddForce(forceToApply, ForceMode.Impulse);
        print(_rb.drag);


        // invoke RestDash function after dash is done
        Invoke(nameof(ResetDash), _dashDuration);
    }

    private void ResetDash() // Justin
    {
        // reset drag
        _rb.drag = normalDrag;

        // player isnt seen as dashing anymore
        _isDashing = false;
    }

    private void ControlDrag()
    {
        if (_isGrounded && state != State.DASHING && state != State.ATTACKING)
        {
            _rb.drag = normalDrag;
        }
        else
        {
            _rb.drag = 0;
        }
    }
}
