using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    //A new combat controller class where attacks are handled using ANIMATION EVENTS
    //Elizabeth
    [Header("References")]
    public GameObject weapon;
    public GameObject weaponProjectilePrefab;
    public GameObject weaponCatchTarget;
    public GameObject lightHitbox;
    public GameObject heavyHitbox;
    private CharController charController;
    private Rigidbody _rb;
    public FadeOnDeath fadeOnDeath;
    public HealthBar healthBar;
    public ElectricVials electricVials;
    public DamageFlash damageFlash;

    [Header("Player Stats & Variables")]
    public float maxHP = 10;
    [HideInInspector] public float currHP;
    public float vialRechargeSpeed;
    private float vialTimer;
    [SerializeField] private float baseAttackPower = 5;
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
    public float specialAttackMult;
    public float weaponThrowSpeed = 4f;
    public float weaponRecallSpeed = 5f;
    public float weaponCatchDistance = 1f;
    [HideInInspector] public bool hasWeapon = true;
    [HideInInspector] public bool isCatching = false;
    
    [Header("Other Variables")]
    [HideInInspector] public float attackPower;
    private float invulnTimer = 0f;
    [HideInInspector] public bool comboChain = false;
    [HideInInspector] public int comboCounter = 0;
    private float comboTimer = 0f;
    private bool comboTimerActive = false;
    private bool acceptingInput = true;
    private bool isNextAttackBuffered = false;
    private GameObject activeWeaponProjectile;
    private Vector3 throwAim;
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
                    print("located catchable weapon");
                    charController.animator.SetBool("isCatching", true);
                    isCatching = false;
                }
            }

        }

        if(charController.state != CharController.State.ATTACKING) {
            charController.animator.SetInteger("lightAttackCombo", 0);
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
        

        if (comboChain && comboTimerActive && comboTimer != 0f) { // Harsha and Elizabeth
            if (Time.time - comboTimer >= maxComboTimer) { // If the combo chain is activated (as in a combo was started), this checks if the next button was pressed within the time window alloted
               // print("broken combo, COMBO COUNTER " + comboCounter.ToString());
                ResetLightAttackCombo();
            }
        } 

        //Start Light Attack //Harsha Justin and Elizabeth
        if(Input.GetButtonDown("LightAttack") && charController.state != CharController.State.PAUSED && !charController.animator.GetBool("isHeavyAttacking")
        && acceptingInput && hasWeapon && !isNextAttackBuffered) {
            charController.state = CharController.State.ATTACKING;
            comboCounter++;
            if(comboCounter > 3) {
               // print("combo counter > 3!!!! COMBO COUNTER " + comboCounter.ToString());
                comboCounter = 0;
            }
            charController.animator.SetInteger("lightAttackCombo", comboCounter);
            comboTimer = 0f;
            comboTimerActive = false;
            acceptingInput = false;
            isNextAttackBuffered = true;
           // print("input taken COMBO COUNTER " + comboCounter.ToString());

            
        }

        // Start heavy Attack
        if (Input.GetButtonDown("HeavyAttack") && electricVials.currVial >= 1 &&
            charController.state != CharController.State.PAUSED && charController.state != CharController.State.ATTACKING && hasWeapon) // Harsha and Justin and Elizabeth
        {
            ResetLightAttackCombo();

            charController.state = CharController.State.ATTACKING;

            charController.animator.SetBool("isHeavyAttacking", true);
            
            // remove 1 electric vial
            electricVials.RemoveVials(1);
        }
        if(currHP <= 0) 
        {
            print("Player is dead");
            charController.animator.SetBool("isDead", true);
        }

        if(Input.GetButtonDown("SpecialAttack") && electricVials.currVial >= 1 && !hasWeapon && !isCatching &&
        !charController.animator.GetBool("isCatching") && !charController.animator.GetBool("isThrowing")) {
            print("activated catching");
            isCatching = true;
        }

        if(Input.GetButtonDown("SpecialAttack") && electricVials.currVial >= 1 && hasWeapon && !isCatching && 
        !charController.animator.GetBool("isThrowing") && !charController.animator.GetBool("isCatching") ) {
            charController.state = CharController.State.ATTACKING;
            hasWeapon = false;
            charController.animator.SetBool("hasWeapon", hasWeapon);
            LookAtMouse();
            charController.animator.SetBool("isThrowing", true);
            electricVials.RemoveVials(1);

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
        if(activeWeaponProjectile != null) {
            activeWeaponProjectile.SetActive(false);
        }
    }

    private void EndCatch() {
        print("ending catch");
        isCatching = false;
        charController.animator.SetBool("isCatching", false);
        charController.state = CharController.State.IDLE;
    }

    private void EnableLightAttackHitbox() {
        comboTimerActive = false;
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
        comboTimer = Time.time;
        comboTimerActive = true;
      //  print("marked combo timer COMBO COUNTER " + comboCounter.ToString());
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
        lightSlashVFX[comboCounter - 1].SetActive(false);
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
        Vector3 forceToApply = transform.forward * force;
        _rb.drag = 0;
        _rb.AddForce(forceToApply * multiplier, ForceMode.Impulse);
    }
    
    public void TakeDamage(float dmg, Collider collider) // Justin
    {
        if(Time.time - invulnTimer >= invulnLength && charController.canTakeDmg == true)
        {
            // does dmg
            currHP -= dmg;

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
            print("took damage");

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
        _rb.drag = 20;
        charController.knockback = false;
    }
}
