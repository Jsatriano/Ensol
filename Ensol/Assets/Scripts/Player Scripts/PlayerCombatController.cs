using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    //A new combat controller class where attacks are handled using ANIMATION EVENTS
    //Elizabeth
    [Header("References")]
    public GameObject weapon;
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
    public float lightAttackInputAcceptDelay = 0.2f;
    private float lightAttackInputAcceptTimer = 0f;

    [Header("Heavy Attack Variables")]
    public float heavyAttackMult;
    public float heavyAttackWindupDuration = 0.35f;

    [Header("Special Attack Variables")]
    public float specialAttackMult;
    
    [Header("Other Variables")]
    [HideInInspector] public float attackPower;
    private float invulnTimer = 0f;
    [HideInInspector] public bool comboChain = false;
    [HideInInspector] public int comboCounter = 0;
    private float comboTimer = 0f;
    private bool comboTimerActive = false;

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

        if(charController.state != CharController.State.ATTACKING) {
            charController.animator.SetInteger("lightAttackCombo", 0);
        }

        if (comboChain && comboTimerActive) { // Harsha
            if (Time.time - comboTimer >= maxComboTimer) { // If the combo chain is activated (as in a combo was started), this checks if the next button was pressed within the time window alloted
                print("broken combo");
                ResetLightAttackCombo();
            }
        } 

        //Start Light Attack
        if(Input.GetButtonDown("LightAttack") && charController.state != CharController.State.PAUSED && !charController.animator.GetBool("isHeavyAttacking")
        && Time.time - lightAttackInputAcceptTimer >= lightAttackInputAcceptDelay) {
            lightAttackInputAcceptTimer = Time.time;
            charController.state = CharController.State.ATTACKING;
            comboCounter++;
            if(comboCounter > 3) {
                print("combo counter > 3!!!!");
                comboCounter = 0;
            }
            charController.animator.SetInteger("lightAttackCombo", comboCounter);
            comboTimerActive = false;

            
        }

        // Start heavy Attack
        if (Input.GetButtonDown("HeavyAttack") && electricVials.currVial >= 1 &&
            charController.state != CharController.State.PAUSED) // Harsha and Justin
        {
            ResetLightAttackCombo();

            charController.state = CharController.State.ATTACKING;

            charController.animator.SetBool("isHeavyAttacking", true);
            
            // remove 1 electric vial
            electricVials.RemoveVials(1);
        }
    }

    private void EnableLightAttackHitbox() {
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
    }

    private void MarkComboTimer() {
        comboTimer = Time.time;
        comboTimerActive = true;
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
    }

    private void EnableHeavyAttackHitbox() {
        heavyHitbox.SetActive(true);
    }

    private void DisableHeavyAttackHitbox() {
        heavyHitbox.SetActive(false);
    }

    private void EndHeavyAttack() {
        charController.animator.SetBool("isHeavyAttacking", false);
        charController.state = CharController.State.IDLE;
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
