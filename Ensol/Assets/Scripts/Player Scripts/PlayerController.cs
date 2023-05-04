using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        MOVING,
        DASHING,
        LIGHTATTACKING,
        HEAVYATTACKING,
        THROWING,
        CATCHING,
        KNOCKBACK,
        PAUSED,
        DEAD,
        DIALOGUE
    }
    public State state;
    public int comboCounter = 0;

    public Vector3 forward, right, heading;
    [HideInInspector] public Vector3 zeroVector = new Vector3(0, 0, 0); // empty vector (helps with checking if player is moving)

    public bool controller = false;

    [Header("References")]
    public GameObject weapon;
    public GameObject weaponHead;
    public GameObject weaponBase;
    public GameObject backpack;
    public GameObject weaponProjectilePrefab;
    public GameObject weaponCatchTarget;
    public GameObject lightHitbox;
    public GameObject heavyHitbox;
    private Rigidbody rb;
    public GameObject mouseFollower;
    public GameObject pauseMenu;
    public Animator animator;

    [Header("VFX & UI References")]
    public GameObject[] lightSlashVFX;
    public GameObject heavySlashVFX;
    public GameObject damageVFX;
    public Transform damageVFXLocation;
    public GameObject shieldBreakVFX;
    public DamageFlash damageFlash;
    private Material backpackVialMaterial;
    private Queue<GameObject> activeDamageVFX = new Queue<GameObject>();
    public GameObject shieldVisual;
    public HealthBar healthBar;
    public ElectricVials electricVials;
    public GameObject FX1;
    public GameObject FX2;

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    public float normalDrag;
    [SerializeField] private float rotationSpeed;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    [Header("Dashing Variables")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCD;
    private float dashCdTimer;
    private bool isDashing = false;
    
    [Header("Player Combat Stats & Variables")]
    public float maxHP = 10;
    public float currentHP = PlayerData.currHP;
    public float vialRechargeSpeed;
    private float vialTimer;
    public float baseAttackPower = 5;
    [SerializeField] private float force = 1;
    public float invulnLength = 1;
    public float maxComboTimer = 1.0f;
    public float knockbackTimer = 0.3f;
    public float knockbackForce;

    [Header("Light Attack Variables")]
    public float lightAttackAnimationResetTimer = 0.5f;
    public float lightAttackMult = 1f;
    public float light1Mult = 1f;
    public float light2Mult = 1.3f;
    public float light3Mult = 1.7f;
    private Stack<bool> activeComboTimer = new Stack<bool>();
    private bool allowCancellingLightAttack = false;

    [Header("Heavy Attack Variables")]
    public float heavyAttackMult;
    public int heavyAttackCost = 2;

    [Header("Special Attack Variables")]
    public float specialAttackMult = 0.9f;
    public float specialDamagePulseMult = 0.5f;
    public float weaponThrowSpeed = 4f;
    public float weaponRecallSpeed = 5f;
    public float weaponCatchDistance = 1f;
    public float damagePulseRadius = 1f;
    public int specialAttackCost = 2;
    [HideInInspector] public bool hasWeapon = true;
    [HideInInspector] public bool isCatching = false;
    
    [Header("Other Variables")]
    [HideInInspector] public float attackPower;
    private float invulnTimer = 0f;
    [HideInInspector] public bool comboChain = false;
    private float comboTimer = -1f;
    [HideInInspector] public bool comboTimerActive = false;
    private GameObject activeWeaponProjectile;
    private Vector3 throwAim;
    private bool dying = false;
    private State prevState;
    [HideInInspector] public bool knockback;
    private bool allowInput = true;

    
    //Input Read Variables
    private Vector3 direction;
    private bool lightAttackInput, heavyAttackInput, dashInput, throwAttackInput, shieldInput, pauseInput;

    // function is called in scene start
    private void Start()
    {
        state = State.IDLE;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameObject.tag = "Player";
        knockback = false;
        rb.drag = normalDrag;
        dying = false;
        healthBar.SetMaxHealth(maxHP);
        vialTimer = vialRechargeSpeed;

        if(PlayerData.currHP == -1) {
            PlayerData.currHP = maxHP;
            healthBar.SetMaxHealth(PlayerData.currHP);
        }
        if(PlayerData.currentlyHasBroom || PlayerData.currentlyHasSolar) {
            animator.SetBool("hasWeapon", true);
        }

        backpackVialMaterial = backpack.GetComponent<Renderer>().materials[0];

        if (!PlayerData.currentlyHasBroom && !PlayerData.currentlyHasSolar)
        {
            animator.SetBool("hasWeapon", false);
            hasWeapon = false;
            weapon.SetActive(false);
            weaponHead.SetActive(false);
            weaponBase.SetActive(false);
            backpack.SetActive(false);
            FX1.SetActive(false);
            FX2.SetActive(false);
        }
        else if (PlayerData.currentlyHasBroom && !PlayerData.currentlyHasSolar)
        {
            animator.SetBool("hasWeapon", true);
            weapon.SetActive(true);
            weaponHead.SetActive(false);
            weaponBase.SetActive(false);
            backpack.SetActive(false);
            FX1.SetActive(false);
            FX2.SetActive(false);
        }
        else
        {
            animator.SetBool("hasWeapon", true);
            weapon.SetActive(true);
            weaponHead.SetActive(true);
            weaponBase.SetActive(true);
            backpack.SetActive(true);
            FX1.SetActive(true);
            FX2.SetActive(true);
        }
    }

    void Update() {
        if(PauseMenu.isPaused) {
            return;
        }

        animator.SetBool("hasWeapon", hasWeapon);

        //read inputs
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if(Input.GetButtonDown("Dash")) {
            dashInput = true;
        }
        if(Input.GetButtonDown("SpecialAttack") && allowInput) {
            throwAttackInput = true;
        }
        if(Input.GetButtonDown("LightAttack") && allowInput) {
            lightAttackInput = true;
        }
        if(Input.GetButtonDown("HeavyAttack") && allowInput) {
            heavyAttackInput = true;
        }
        if(Input.GetButtonDown("Shield") && allowInput) {
            shieldInput = true;
        }
        if(Input.GetButtonDown("Cancel") && allowInput) {
            pauseInput = true;
        }

        //decrement dash timer if active
        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }

        //turn on and off shield visibility
        shieldVisual.SetActive(PlayerData.hasShield);

        //Adds electric vials over time (when vials are currently expended)
        if(vialTimer > 0 && electricVials.currVial < 2)
        {
            electricVials.UpdateVial(1 - (vialTimer / vialRechargeSpeed), electricVials.currVial + 1);
            vialTimer -= Time.deltaTime;
        }
        else
        {
            electricVials.AddVial(1);
            vialTimer = vialRechargeSpeed;
        }
        ManageVialShader();
    }

    void FixedUpdate() {
        if(PauseMenu.isPaused) {
            return;
        }

        //State-Agnostic Events
        CheckIfDying();
        isGrounded = Physics.CheckSphere(groundCheck.position, .1f, groundMask);
        ControlDrag();

        //checking if the weapon is within grabbing range mid-recall
        if(isCatching) {
            Collider[] weaponSearch = Physics.OverlapSphere(weaponCatchTarget.transform.position, weaponCatchDistance);
            foreach(Collider col in weaponSearch) {
                if(col.gameObject.tag == "WeaponProjectile") {
                    if(state != State.DASHING) {
                        animator.SetBool("isCatching", true);
                    }
                    else {
                        GrabWeapon();
                    }
                    isCatching = false;
                }
            }

        }
        
        //Ye Olde State Machine
        switch (state) {
            case State.IDLE:
                // animations
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);
                foreach(GameObject vfx in lightSlashVFX) {
                    vfx.SetActive(false);
                }
                allowCancellingLightAttack = false;
                allowInput = true;

                //player recalls weapon if it has been thrown
                if(throwAttackInput && !hasWeapon && !isCatching) {
                    throwAttackInput = false;
                    isCatching = true;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecial, this.transform.position);
                }
                //starts light attack
                if(lightAttackInput && hasWeapon) {
                    ResetInput();
                    PlayerData.lightAttacks += 1;
                    comboCounter += 1;
                    if(comboCounter > 3) {
                        comboCounter = 1;
                    }
                    animator.SetInteger("lightAttackCombo", comboCounter);
                    state = State.LIGHTATTACKING;
                }
                //starts heavy attack
                else if(heavyAttackInput && hasWeapon) {
                    heavyAttackInput = false;
                    if(electricVials.enoughVials(heavyAttackCost) && PlayerData.hasSolarUpgrade) {
                        ResetInput();
                        ResetLightAttackCombo();
                        PlayerData.heavyAttacks += 1;
                        animator.SetBool("isHeavyAttacking", true);
                        electricVials.RemoveVials(heavyAttackCost);
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponHeavyPrep, this.transform.position);
                        state = State.HEAVYATTACKING;
                    }

                }
                //starts throw attack
                else if(throwAttackInput && hasWeapon) {
                    throwAttackInput = false;
                    if(electricVials.enoughVials(specialAttackCost) && PlayerData.hasThrowUpgrade) {
                        ResetInput();
                        LookAtMouse();
                        ResetLightAttackCombo();
                        PlayerData.throwAttacks += 1;
                        hasWeapon = false;
                        electricVials.RemoveVials(specialAttackCost);
                        animator.SetBool("isThrowing", true);
                        animator.SetBool("hasWeapon", hasWeapon);
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecialPrep, this.transform.position);
                        state = State.THROWING;
                    }
                }
                // checks if player starts to move
                else if(direction != zeroVector)
                {
                    state = State.MOVING;
                }
                // if player hits dash button, dash
                else if(PlayerData.hasBroom && dashInput)
                {
                    ResetInput();
                    state = State.DASHING;
                }
                //if player hits pause button, pause
                else if(pauseInput)
                {
                    pauseInput = false;
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
                allowCancellingLightAttack = false;
                allowInput = true;

                //player recalls weapon if it has been thrown
                if(throwAttackInput && !hasWeapon && !isCatching) {
                    throwAttackInput = false;
                    isCatching = true;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecial, this.transform.position);
                }
                //starts light attack
                if(lightAttackInput && hasWeapon) {
                    ResetInput();
                    PlayerData.lightAttacks += 1;
                    comboCounter += 1;
                    if(comboCounter > 3) {
                        comboCounter = 1;
                    }
                    animator.SetInteger("lightAttackCombo", comboCounter);
                    state = State.LIGHTATTACKING;
                }
                //starts heavy attack
                else if(heavyAttackInput && hasWeapon) {
                    heavyAttackInput = false;
                    if(electricVials.enoughVials(heavyAttackCost) && PlayerData.hasSolarUpgrade) {
                        ResetInput();
                        ResetLightAttackCombo();
                        PlayerData.heavyAttacks += 1;
                        animator.SetBool("isHeavyAttacking", true);
                        electricVials.RemoveVials(heavyAttackCost);
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponHeavyPrep, this.transform.position);
                        state = State.HEAVYATTACKING;
                    }

                }
                //starts throw attack
                else if(throwAttackInput && hasWeapon) {
                    throwAttackInput = false;
                    if(electricVials.enoughVials(specialAttackCost) && PlayerData.hasThrowUpgrade) {
                        ResetInput();
                        LookAtMouse();
                        ResetLightAttackCombo();
                        PlayerData.throwAttacks += 1;
                        hasWeapon = false;
                        electricVials.RemoveVials(specialAttackCost);
                        animator.SetBool("isThrowing", true);
                        animator.SetBool("hasWeapon", hasWeapon);
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecialPrep, this.transform.position);
                        state = State.THROWING;
                    }
                }
                // if player hits dash button, dash
                else if(PlayerData.hasBroom && dashInput)
                {
                    ResetInput();
                    state = State.DASHING;
                }
                // if player stops moving, go idle
                else if(direction == zeroVector)
                {
                    state = State.IDLE;                   
                }

                else if(pauseInput)
                {
                    pauseInput = false;
                    prevState = State.MOVING;
                    state = State.PAUSED;

                }

                Move();
                break;

            case State.DASHING:
                allowInput = false;
                // make player dash if CD is done
                if(dashCdTimer <= 0)
                {
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isDashing", true);
                    animator.SetBool("isHeavyAttacking", false);
                    animator.SetInteger("lightAttackCombo", 0);
                    Dash();
                }

                //player recalls weapon if it has been thrown
                if(throwAttackInput && !hasWeapon && !isCatching) {
                    throwAttackInput = false;
                    isCatching = true;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecial, this.transform.position);
                }

                // after the dash is done, change states
                if(!isDashing)
                {   
                    if(direction == zeroVector)
                    {
                        state = State.IDLE;
                    }
                    else if(direction != zeroVector)
                    {
                        state = State.MOVING;
                    }
                    else if(pauseInput)
                    {
                        pauseInput = false;
                        prevState = State.DASHING;
                        state = State.PAUSED;
                    }
                }
                break;

            case State.LIGHTATTACKING:
                if(!allowCancellingLightAttack) {
                    allowInput = false;
                }
                else{
                    allowInput = true;
                }
                if(pauseInput) {
                    pauseInput = false;
                    prevState = State.LIGHTATTACKING;
                    state = State.PAUSED;
                }
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);

                comboTimerActive = false;

                if(allowCancellingLightAttack && lightAttackInput) {
                    lightAttackInput = false;
                    allowCancellingLightAttack = false;
                    comboCounter += 1;
                    if(comboCounter > 3) {
                        comboCounter = 1;
                    }
                    foreach(GameObject vfx in lightSlashVFX) {
                        vfx.SetActive(false);
                    }
                    animator.SetInteger("lightAttackCombo", comboCounter);
                }
                //Various other events that occur during the animation, such as hitboxes and movement are handled in anim events
                //so is transitioning out of this state
                break;

            case State.HEAVYATTACKING:
                allowInput = false;
                ResetInput();
                if(pauseInput)
                {
                    pauseInput = false;
                    prevState = State.HEAVYATTACKING;
                    state = State.PAUSED;
                }
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", true);
                animator.SetInteger("lightAttackCombo", 0);
                break;

            case State.THROWING:
                allowInput = false;
                if(pauseInput)
                {
                    pauseInput = false;
                    prevState = State.THROWING;
                    state = State.PAUSED;
                }
                animator.SetBool("isRunning", true);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);
                animator.SetBool("isThrowing", true);
                break;

            case State.CATCHING:
                allowInput = false;
                if(pauseInput)
                {
                    pauseInput = false;
                    prevState = State.CATCHING;
                    state = State.PAUSED;
                }
                animator.SetBool("isRunning", true);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);
                animator.SetBool("isCatching", true);
                break;

            case State.KNOCKBACK:
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);

                // once knockback is over, go to idle state
                if(knockback == false)
                {
                    state = State.IDLE;
                }
                // if paused during knockback, save state so game doesnt break
                else if(pauseInput)
                {
                    pauseInput = false;
                    prevState = State.KNOCKBACK;
                    state = State.PAUSED;
                }
                break;

            case State.PAUSED:
                allowInput = false;
                // pause game, make all actions unavailable
                if(!pauseMenu.activeInHierarchy)
                {
                    state = prevState;
                }
                break;

            case State.DEAD:
                allowInput = false;
                if(pauseInput)
                {
                    pauseInput = false;
                    prevState = State.DEAD;
                    state = State.PAUSED;
                }
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);
                break;

            case State.DIALOGUE:
                allowInput = false;
                if(pauseInput)
                {
                    pauseInput = false;
                    prevState = State.DIALOGUE;
                    state = State.PAUSED;
                }
                animator.SetBool("isRunning", false);
                animator.SetBool("isDashing", false);
                animator.SetBool("isHeavyAttacking", false);
                animator.SetInteger("lightAttackCombo", 0);

                break;
        }

    }

    //ANIMATION EVENTS

    //Generic Anim Events

    private void LookAtMouse() {
        if(!controller) {
            Vector3 toMouse = (mouseFollower.transform.position - transform.position);
            transform.forward = new Vector3(toMouse.x, 0, toMouse.z);
        }
    }

    private void AttackForce(float multiplier)
    {
        rb.velocity = Vector3.zero;
        Vector3 forceToApply = transform.forward * force;
        rb.drag = 1;
        rb.AddForce(forceToApply * multiplier, ForceMode.Impulse);
    }

    private void ReturnToIdle() {
        state = State.IDLE;
    }

    //Light Attack anim events

    private void EnableLightAttackHitbox() {
        if (comboCounter < 3){
            if (PlayerData.currentlyHasSolar)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponLight, this.transform.position);
            } else
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponLightNormal, this.transform.position);
            }
        } else {
            if (PlayerData.currentlyHasSolar)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponLightStab, this.transform.position);
            }
            else
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponLightNormal, this.transform.position);
            }
        }
            
        if(comboCounter == 1) {
                attackPower = baseAttackPower * lightAttackMult * light1Mult;
        }
        else if(comboCounter == 2) {
            attackPower = baseAttackPower * lightAttackMult * light2Mult;
        }
        else if(comboCounter == 3) {
            attackPower = baseAttackPower * lightAttackMult * light3Mult;
        }
        allowCancellingLightAttack = false;
        lightHitbox.SetActive(true);
    }

    private void DisableLightAttackHitbox() {
        lightHitbox.SetActive(false);
    }

    private void StartLightSlash(int callCounter)
    {
        lightSlashVFX[callCounter - 1].SetActive(true);
    }

    private void EndLightSlash(int callCounter)
    {
        lightSlashVFX[callCounter - 1].SetActive(false);
    }

    private void AllowInput() {
        allowCancellingLightAttack = true;
    }

    private void MarkComboTimer(int callCounter) {
        StartCoroutine(EndComboTimer(callCounter));
    }

    private void ResetLightAttackCombo() {
        comboCounter = 0;
        animator.SetInteger("lightAttackCombo", comboCounter);
        foreach(GameObject vfx in lightSlashVFX) {
            vfx.SetActive(false);
        }
    }

    //Heavy Attack anim events
    private void EnableHeavyAttackHitbox() {
        heavyHitbox.SetActive(true);

        attackPower = baseAttackPower * heavyAttackMult;
    }

    private void DisableHeavyAttackHitbox() {
        heavyHitbox.SetActive(false);
        
        attackPower = baseAttackPower;
    }

    private void EndHeavyAttack() {
        animator.SetBool("isHeavyAttacking", false);
        state = State.IDLE;
    }
    private void StartHeavySlash()
    {
        heavySlashVFX.SetActive(true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponHeavy, this.transform.position);
    }

    private void EndHeavySlash()
    {
        heavySlashVFX.SetActive(false);
    }

    //Throw/Catch anim events

    private void SpawnSpecialAttackProjectile() {
        attackPower = baseAttackPower * specialAttackMult;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecial, damageVFXLocation.transform.position);

        LookAtMouse();
        activeWeaponProjectile = Instantiate(weaponProjectilePrefab, weaponCatchTarget.transform.position, gameObject.transform.rotation);
        activeWeaponProjectile.GetComponent<WeaponHitbox>().isProjectile = true;

        Vector3 throwTarget = mouseFollower.transform.position;
        throwTarget.y = activeWeaponProjectile.transform.position.y;
        activeWeaponProjectile.transform.LookAt(throwTarget);
        weapon.SetActive(false);
        weaponHead.SetActive(false);
        weaponBase.SetActive(false);
        FX1.SetActive(false);
        FX2.SetActive(false);

    }

    private void EndThrow() {
        animator.SetBool("isThrowing", false);
        state = State.IDLE;
    }

    private void BeginCatch() {
        state = State.CATCHING;
    }

    public void GrabWeapon() {
        attackPower = baseAttackPower;

        hasWeapon = true;
        animator.SetBool("hasWeapon", true);
        weapon.SetActive(true);
        weaponHead.SetActive(true);
        weaponBase.SetActive(true);
        FX1.SetActive(true);
        FX2.SetActive(true);
        if(activeWeaponProjectile != null) {
            activeWeaponProjectile.SetActive(false);
        }
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecialReturn, damageVFXLocation.transform.position);
    }

    private void EndCatch() {
        isCatching = false;
        animator.SetBool("isCatching", false);
        state = State.IDLE;
    }


    //MOVEMENT FUNCTIONS

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
            Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal");

            // says what our up movement is going to be ( + / - ) depending on what vert key is being pressed
            Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");

            // combines both movements to create a direction that our character will point to
            heading = Vector3.Normalize(rightMovement + upMovement);

            // smoothly rotates player when changeing directions (rather than abruptly)
            if (heading != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(heading, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
            }
           
            // makes our movement happen

            rb.AddForce(heading * acceleration, ForceMode.Acceleration);
            Vector3 velocityXZ = new Vector3(rb.velocity.x, 0, rb.velocity.z);           
            if (velocityXZ.magnitude > moveSpeed)
            {
                Vector3 velocityY = new Vector3(0, rb.velocity.y, 0);
                velocityXZ = Vector3.ClampMagnitude(velocityXZ, moveSpeed);
                rb.velocity = velocityXZ + velocityY;
            }
            PlayerData.distanceMoved += rb.velocity.magnitude * Time.deltaTime;
        }    
    }

    private void Dash() // Justin
    {
        // return func if dash is still on CD | else dash is successful, start CD until dahs is available again
        if(dashCdTimer > 0) return;
        else dashCdTimer = dashCD;

        PlayerData.dashes += 1;

        // player is now seen as dashing
        isDashing = true;
        rb.velocity = Vector3.zero;

        //sfx
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerDodge, this.transform.position);

        // find out how much force to apply to player (also check if player is moving or not)
        Vector3 forceToApply;
        if(direction == zeroVector)
        {
            forceToApply = transform.forward * dashForce;
        }
        else 
        {
            if(heading != transform.forward) {
                transform.forward = heading;
            }
            forceToApply = heading * dashForce;
        }


        // increase drag and apply force forwards of where player is facing
        rb.drag = 0;
        rb.AddForce(forceToApply, ForceMode.Impulse);


        // invoke RestDash function after dash is done
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash() // Justin
    {
        // reset drag
        rb.drag = normalDrag;

        // player isnt seen as dashing anymore
        isDashing = false;
    }

    private void ControlDrag()
    {
        if (isGrounded && state != State.DASHING && state != State.LIGHTATTACKING && state != State.HEAVYATTACKING && state != State.THROWING && state != State.CATCHING)
        {
            rb.drag = normalDrag;
        }
        else
        {
            rb.drag = 1;
        }
    }

    //COMBAT FUNCTIONS

    private void ResetInput() {
        throwAttackInput = false;
        dashInput = false;
        heavyAttackInput = false;
        lightAttackInput = false;
        shieldInput = false;
    }

    public void CheckIfDying() {
        if(PlayerData.currHP <= 0) 
        {
            if (SceneManager.GetActiveScene().name == "PlaytestingScene")
            {
                PlayerData.currHP = -1;
                SceneManager.LoadScene("PlaytestingScene");
                return;
            }
            state = State.DEAD;
            PlayerData.currentlyHasBroom = false;
            PlayerData.currentlyHasSolar = false;
            PlayerData.currentNode = 1;
            animator.SetBool("isDead", true);
            //sfx
            if (dying == false){
                PlayerData.deaths += 1;
                dying = true;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.playerDeath, this.transform.position);
                AudioManager.instance.PlayOneShot(FMODEvents.instance.deathCut, this.transform.position);
            }
        } 
    }

    public void TakeDamage(float dmg, Collider collider) // Justin
    {
        if(Time.time - invulnTimer >= invulnLength)
        {
            if(!PlayerData.hasShield) {
                // does dmg
                PlayerData.currHP -= dmg;

                // starts invuln
                invulnTimer = Time.time;

                // updates UI healthbar
                healthBar.SetHealth(PlayerData.currHP);

                // damage flash
                StartCoroutine(damageFlash.FlashRoutine());

                // knockback player
                StartCoroutine(Knockback(collider));

                // change state to limit actions
                state = State.KNOCKBACK;

                AudioManager.instance.PlayOneShot(FMODEvents.instance.minorCut, damageVFXLocation.transform.position);

                //play the damage vfx
                GameObject newDamageVFX = Instantiate(damageVFX, damageVFXLocation);
                activeDamageVFX.Enqueue(newDamageVFX);
                StartCoroutine(DeleteDamageVFX());
            }
            else{
                GameObject newDamageVFX = Instantiate(shieldBreakVFX, damageVFXLocation);
                ShieldPickup.playerShieldOn.stop(STOP_MODE.ALLOWFADEOUT);
                AudioManager.instance.PlayOneShot(FMODEvents.instance.playerShieldBreak, damageVFXLocation.transform.position);
                activeDamageVFX.Enqueue(newDamageVFX);
                StartCoroutine(DeleteDamageVFX());
                PlayerData.hasShield = false;
                invulnTimer = Time.time;
            }

        }
    }

    private IEnumerator DeleteDamageVFX() {
        yield return new WaitForSeconds(1f);
        GameObject damageVFXToDelete = activeDamageVFX.Dequeue();
        Destroy(damageVFXToDelete);
    }

    private IEnumerator Knockback(Collider collider)
    {
        // start knockback
        knockback = true;

        // calculate direction to push player back
        Vector3 direction = (collider.transform.position - transform.position) * -1;
        Vector3 forceToApply = direction * knockbackForce;

        // push player back
        rb.drag = 0;
        rb.AddForce(forceToApply, ForceMode.Impulse);
        yield return new WaitForSeconds(knockbackTimer);

        // reset drag and end knockback
        rb.drag = normalDrag;
        knockback = false;
    }

    private IEnumerator EndComboTimer(int callCounter) {
        yield return new WaitForSeconds(maxComboTimer);
        if(comboCounter == callCounter) {
            comboCounter = 0;
        }
    }

    //EQUIPMENT CHECK/CHANGE

    public void PickedUpBroom()
    {
        animator.SetBool("hasWeapon", true);
        hasWeapon = true;
        PlayerData.hasBroom = true;
        PlayerData.currentlyHasBroom = true;
        weapon.SetActive(true);
        weaponHead.SetActive(false);
        weaponBase.SetActive(false);
        FX1.SetActive(false);
        FX2.SetActive(false);
    }

    public void PickedUpThrowUpgrade()
    {
        PlayerData.hasThrowUpgrade = true;
    }

    public void PickedUpSolarUpgrade()
    {
        PlayerData.hasSolarUpgrade = true;
        PlayerData.hasThrowUpgrade = true;
        PlayerData.currentlyHasSolar = true;
        PlayerData.currentlyHasBroom = true;
        hasWeapon = true;
        animator.SetBool("hasWeapon", true);
        weapon.SetActive(true);
        weaponHead.SetActive(true);
        weaponBase.SetActive(true);
        backpack.SetActive(true);
        FX1.SetActive(true);
        FX2.SetActive(true);
    }

    public void TestPickedUpSolarUpgrade()
    {
        PlayerData.hasSolarUpgrade = true;
        PlayerData.currentlyHasSolar = true;
        PlayerData.currentlyHasBroom = true;
        hasWeapon = true;
        animator.SetBool("hasWeapon", true);
        weapon.SetActive(true);
        weaponHead.SetActive(true);
        weaponBase.SetActive(true);
        backpack.SetActive(true);
        FX1.SetActive(true);
        FX2.SetActive(true);
    }

    public void RemoveThrowUpgrade()
    {
        PlayerData.hasThrowUpgrade = false;
    }

    public void RemoveSolarUpgrade()
    {
        PlayerData.hasSolarUpgrade = false;
        PlayerData.currentlyHasSolar = false;
        hasWeapon = true;
        animator.SetBool("hasWeapon", true);
        weapon.SetActive(true);
        weaponHead.SetActive(true);
        weaponBase.SetActive(true);
        backpack.SetActive(true);
        FX1.SetActive(true);
        FX2.SetActive(true);
    }

    //SHADER MANAGEMENT

    private void ManageVialShader() {
        if(electricVials.currVial + 1 >= 3) {
            backpackVialMaterial.SetFloat("Gradient_Clipping_Amount", 1f);
        }
        else if(electricVials.currVial + 1 == 2) {
            backpackVialMaterial.SetFloat("_Gradient_Clipping_Amount", 0.2f);
        }
        else if(electricVials.currVial + 1 == 1) {
            backpackVialMaterial.SetFloat("_Gradient_Clipping_Amount", 0.015f);
        }
        else {
            print("vials dark");
            backpackVialMaterial.SetFloat("_Gradient_Clipping_Amount", -1f);
        }
    }

}
