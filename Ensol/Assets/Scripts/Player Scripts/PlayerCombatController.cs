using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    //A new combat controller class where attacks are handled using ANIMATION EVENTS
    //Elizabeth
    [Header("References")]
    public GameObject weapon;
    public GameObject weaponHead;
    public GameObject weaponBase;
    public GameObject weaponProjectilePrefab;
    public GameObject weaponCatchTarget;
    public GameObject FX1;
    public GameObject FX2;
    public GameObject lightHitbox;
    public GameObject heavyHitbox;
    private CharController charController;
    private Rigidbody _rb;
    public FadeOnDeath fadeOnDeath;
    public HealthBar healthBar;
    public ElectricVials electricVials;
    public DamageFlash damageFlash;

    [Header("Sound Effects")] // Harsha
    [SerializeField] private AudioSource heavySoundEffect;
    [SerializeField] private AudioSource specialSoundEffect;
    [SerializeField] private AudioSource lightSoundEffect;
    [SerializeField] private AudioSource lightStabSoundEffect;
    [SerializeField] private AudioSource minorcutSoundEffect;
    [SerializeField] private AudioSource deathcutSoundEffect;


    [Header("Player Stats & Variables")]
    public float maxHP = 10;
    [HideInInspector] public float currHP;
    public float vialRechargeSpeed;
    private float vialTimer;
    public float baseAttackPower = 5;
    [SerializeField] private float force = 1;
    public float invulnLength = 1;
    public float maxComboTimer = 1.0f;
    public float knockbackTimer = 0.3f;

    [Header("Light Attack Variables")]
    public float lightAttackAnimationResetTimer = 0.5f;
    public float lightAttackMult = 1f;
    public float light1Mult = 1f;
    public float light2Mult = 1.3f;
    public float light3Mult = 1.7f;

    [Header("Heavy Attack Variables")]
    public float heavyAttackMult;

    [Header("Special Attack Variables")]
    public float specialAttackMult = 0.9f;
    public float specialDamagePulseMult = 0.5f;
    public float weaponThrowSpeed = 4f;
    public float weaponRecallSpeed = 5f;
    public float weaponCatchDistance = 1f;
    public float damagePulseRadius = 1f;
    [HideInInspector] public bool hasWeapon = true;
    [HideInInspector] public bool isCatching = false;
    
    [Header("Other Variables")]
    [HideInInspector] public float attackPower;
    private float invulnTimer = 0f;
    [HideInInspector] public bool comboChain = false;
    [HideInInspector] public int comboCounter = 0;
    private float comboTimer = -1f;
    private bool comboTimerActive = false;
    private bool acceptingInput = true;
    private bool isNextAttackBuffered = false;
    private GameObject activeWeaponProjectile;
    private Vector3 throwAim;

    [Header("VFX References")]
    public GameObject[] lightSlashVFX;
    public GameObject heavySlashVFX;

    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
        healthBar.SetMaxHealth(maxHP);
        vialTimer = vialRechargeSpeed;
        charController = gameObject.GetComponent<CharController>();
        attackPower = baseAttackPower;
        _rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activeWeaponProjectile != null && !activeWeaponProjectile.activeSelf) {
            activeWeaponProjectile.SetActive(true);
            Destroy(activeWeaponProjectile);
        }
        //Adds electric vials over time
        if(vialTimer > 0)
        {
            vialTimer -= Time.deltaTime;
        }
        else
        {
            electricVials.AddVial();
            vialTimer += vialRechargeSpeed;
        }

        charController.animator.SetBool("hasWeapon", hasWeapon);
        
        if(isCatching) {
            Collider[] weaponSearch = Physics.OverlapSphere(weaponCatchTarget.transform.position, weaponCatchDistance);
            foreach(Collider col in weaponSearch) {
                if(col.gameObject.tag == "WeaponProjectile") {
                    //print("located catchable weapon");
                    charController.animator.SetBool("isCatching", true);
                    isCatching = false;
                }
            }

        }

        if(charController.state != CharController.State.ATTACKING && hasWeapon) {
            charController.animator.SetInteger("lightAttackCombo", 0);
            lightSoundEffect.Play(); // Plays when doing a light attack sound effect
            acceptingInput = true;
            isNextAttackBuffered = false;
            if(lightHitbox.activeSelf) {
                lightHitbox.SetActive(false);
            }
            if(heavyHitbox.activeSelf) {
                heavyHitbox.SetActive(false);
            }
            

        }

        if(charController.state == CharController.State.KNOCKBACK) {
            ResetLightAttackCombo();
            charController.animator.SetBool("isHeavyAttacking", false);
        }

        if(comboChain && comboTimerActive && Input.GetButtonDown("Dash")) {
            ResetLightAttackCombo();
            LookAtMouse();
            charController.state = CharController.State.DASHING;
        }
        

        //Start Light Attack //Harsha Justin and Elizabeth
        if(Input.GetButtonDown("LightAttack") && charController.state != CharController.State.PAUSED && !charController.animator.GetBool("isHeavyAttacking")
        && acceptingInput && hasWeapon && !isNextAttackBuffered && comboCounter < 3) {
            charController.state = CharController.State.ATTACKING;
            comboCounter++;
            charController.animator.SetInteger("lightAttackCombo", comboCounter);
            if(comboCounter != 3) // Plays normal light attack sound effect if combo counter is less than 3, otherwise plays the stab sound effect
            {
                lightSoundEffect.Play();
            }
            else 
            {
                lightStabSoundEffect.Play();
            }
            comboTimer = -1f;
            comboTimerActive = false;
            acceptingInput = false;
            isNextAttackBuffered = true;
           // print("input taken COMBO COUNTER " + comboCounter.ToString());

            
        }

        if (comboChain && comboTimerActive && comboTimer != -1f) { // Harsha and Elizabeth
            if (Time.time - comboTimer >= maxComboTimer) { // If the combo chain is activated (as in a combo was started), this checks if the next button was pressed within the time window alloted
                //print("broken combo, COMBO COUNTER " + comboCounter.ToString());
                ResetLightAttackCombo();
            }
        } 

        // Start heavy Attack
        if (Input.GetButtonDown("HeavyAttack") && electricVials.currVial >= 0 &&
            charController.state != CharController.State.PAUSED && charController.state != CharController.State.ATTACKING && hasWeapon) // Harsha and Justin and Elizabeth
        {
            ResetLightAttackCombo();

            charController.state = CharController.State.ATTACKING;

            charController.animator.SetBool("isHeavyAttacking", true);
            heavySoundEffect.Play(); // Plays when heavy attack is clicked
            
            // remove 1 electric vial
            electricVials.RemoveVials(1);
        }
        if(currHP <= 0) 
        {
            //print("Player is dead");
            charController.state = CharController.State.DEAD;
            charController.animator.SetBool("isDead", true);
            
        }

        if(Input.GetButtonDown("SpecialAttack") && !hasWeapon && !isCatching &&
        !charController.animator.GetBool("isCatching") && !charController.animator.GetBool("isThrowing")) {
           // print("activated catching");
            isCatching = true;
        }

        if(Input.GetButtonDown("SpecialAttack") && electricVials.currVial >= 1 && hasWeapon && !isCatching && 
        !charController.animator.GetBool("isThrowing") && !charController.animator.GetBool("isCatching") ) {
            charController.state = CharController.State.ATTACKING;
            hasWeapon = false;
            charController.animator.SetBool("hasWeapon", hasWeapon);
            LookAtMouse();
            charController.animator.SetBool("isThrowing", true);
            specialSoundEffect.Play(); // Plays when special attack is clicked
            electricVials.RemoveVials(2);
            acceptingInput = false;

        }

    }

    private void SpawnSpecialAttackProjectile() {
        attackPower = baseAttackPower * specialAttackMult;

        LookAtMouse();
        activeWeaponProjectile = Instantiate(weaponProjectilePrefab, weapon.transform.position, charController.transform.rotation);
        activeWeaponProjectile.GetComponent<WeaponHitbox>().isProjectile = true;

        Vector3 throwTarget = charController.mouseFollower.transform.position;
        throwTarget.y = activeWeaponProjectile.transform.position.y;
        activeWeaponProjectile.transform.LookAt(throwTarget);
        weapon.SetActive(false);
        weaponHead.SetActive(false);
        weaponBase.SetActive(false);
        FX1.SetActive(false);
        FX2.SetActive(false);

    }

    private void EndThrow() {
        charController.animator.SetBool("isThrowing", false);
        charController.state = CharController.State.IDLE;
    }

    private void BeginCatch() {
        charController.state = CharController.State.ATTACKING;
    }

    public void GrabWeapon() {
        attackPower = baseAttackPower;

        hasWeapon = true;
        charController.animator.SetBool("hasWeapon", true);
        weapon.SetActive(true);
        weaponHead.SetActive(true);
        weaponBase.SetActive(true);
        FX1.SetActive(true);
        FX2.SetActive(true);
        if(activeWeaponProjectile != null) {
            activeWeaponProjectile.SetActive(false);
        }
    }

    private void EndCatch() {
       // print("ending catch");
        isCatching = false;
        acceptingInput = true;
        charController.animator.SetBool("isCatching", false);
        charController.state = CharController.State.IDLE;
    }

    private void EnableLightAttackHitbox() {
        comboTimerActive = false;
        comboTimer = -1f;
        if(comboCounter == 1) {
                comboChain = true;
                attackPower = baseAttackPower * lightAttackMult * light1Mult;
            }
            else if(comboCounter == 2 && comboChain) {
                attackPower = baseAttackPower * lightAttackMult * light2Mult;
            }
            else if(comboCounter == 3 && comboChain) {
                comboChain = false;
                attackPower = baseAttackPower * lightAttackMult * light3Mult;
            }
        lightHitbox.SetActive(true);
       // print("enabled attack hitbox COMBO COUNTER " + comboCounter.ToString());
    }

    private void MarkComboTimer() {
        if(!isNextAttackBuffered) {
           comboTimer = Time.time;
            comboTimerActive = true;
            //print("marked combo timer COMBO COUNTER " + comboCounter.ToString()); 
        }
        else{
           // print("combo timer not needed COMBO COUNTER " + comboCounter.ToString());
        }
        
    }

    private void AllowInput() {
        acceptingInput = true;
        isNextAttackBuffered = false;
       // print("allowing input COMBO COUNTER " + comboCounter.ToString());
    }

    private void DisableLightAttackHitbox() {
        lightHitbox.SetActive(false);
    }

    private void ResetLightAttackCombo() {
        comboCounter = 0;
        charController.animator.SetInteger("lightAttackCombo", 0);
        attackPower = baseAttackPower;
        charController.state = CharController.State.IDLE;
        comboChain = false;
        comboTimerActive = false;
        acceptingInput = true;
    }

    private void EnableHeavyAttackHitbox() {
        heavyHitbox.SetActive(true);

        attackPower = baseAttackPower * heavyAttackMult;
    }

    private void DisableHeavyAttackHitbox() {
        heavyHitbox.SetActive(false);
        
        attackPower = baseAttackPower;
    }

    private void EndHeavyAttack() {
        charController.animator.SetBool("isHeavyAttacking", false);
        charController.state = CharController.State.IDLE;
        acceptingInput = true;
    }

    private void LookAtMouse() {
        if(!charController.controller) {
            Vector3 toMouse = (charController.mouseFollower.transform.position - charController.transform.position);
            charController.transform.forward = new Vector3(toMouse.x, 0, toMouse.z);
        }
    }

    private void SetDashDirection(){
        charController.transform.rotation.SetLookRotation(charController.heading);
    }

    // functions for showing slashes on attacks
    private void StartLightSlash()
    {
        lightSlashVFX[comboCounter - 1].SetActive(true);
    }

    private void EndLightSlash()
    {
        if(comboCounter > 0 && comboCounter < 3) {
            lightSlashVFX[comboCounter - 1].SetActive(false);
        }
        else{
            foreach(GameObject vfx in lightSlashVFX) {
                vfx.SetActive(false);
            }
        }
    }

    private void StartHeavySlash()
    {
        heavySlashVFX.SetActive(true);
    }

    private void EndHeavySlash()
    {
        heavySlashVFX.SetActive(false);
    }

    // how much forward force is added to every attack
    private void AttackForce(float multiplier)
    {
        _rb.velocity = Vector3.zero;
        Vector3 forceToApply = transform.forward * force;
        print(force);
        _rb.drag = 0;
        _rb.AddForce(forceToApply * multiplier, ForceMode.Impulse);
    }
    
    public void TakeDamage(float dmg, Collider collider) // Justin
    {
        if(Time.time - invulnTimer >= invulnLength)
        {
            // does dmg
            currHP -= dmg;
            if(currHP <= 0) // if the damage taken kills the deer, plays a death cut sound effect, otherwise it plays a regular sound effect
            {
                deathcutSoundEffect.Play();
            }
            else 
            {
                minorcutSoundEffect.Play();
            }

            // starts invuln
            invulnTimer = Time.time;

            // updates UI healthbar
            healthBar.SetHealth(currHP);

            // damage flash
            StartCoroutine(damageFlash.FlashRoutine());

            // knockback player
            StartCoroutine(Knockback(collider));

            // change state to limit actions
            charController.state = CharController.State.KNOCKBACK;
           // print("took damage");

        }
    }

    private IEnumerator Knockback(Collider collider)
    {
        // start knockback
        charController.knockback = true;

        // calculate direction to push player back
        Vector3 direction = (collider.transform.position - transform.position) * -1;
        Vector3 forceToApply = direction * charController.knockbackForce;

        // push player back
        _rb.drag = 0;
        _rb.AddForce(forceToApply, ForceMode.Impulse);
        yield return new WaitForSeconds(knockbackTimer);

        // reset drag and end knockback
        _rb.drag = charController.normalDrag;
        charController.knockback = false;
    }
}
