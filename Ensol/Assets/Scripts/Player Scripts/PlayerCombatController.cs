using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    //This entire class is for the most part very extremely placeholder. I intend to do some more work later to find a better system to use for attack combos.
    //Elizabeth
    [Header("References")]
    public GameObject lightAttackHitbox;
    public GameObject heavyAttackHitbox;
    private CharController charController;
    private Rigidbody _rb;
    public FadeOnDeath fadeOnDeath;
    public HealthBar healthBar;
    public ElectricVials electricVials;
    public DamageFlash damageFlash;

    [Header("Health and Attack Stats")]
    public float maxHP = 10;
    public float currHP;
    public float attackPower; //used to calculate the real damage value of different attacks
    public float maxComboTimer = 2.0f;
    public float vialRechargeSpeed;
    private float vialTimer;
    [SerializeField] private float baseAttackPower = 5;
    [SerializeField] private float attackDelay = 0.8f;
    [SerializeField] private float force;

    [Header("Light Attack Stats")]
    [SerializeField] private float lightAttackSpeed;
    [SerializeField] private float lightAttackDuration;
    

    [Header("Heavy Attack Stats")]
    [SerializeField] private float heavyAttackSpeed = 1.5f;
    [SerializeField] private float heavyAttackDuration = 0.5f;
    [SerializeField] private float heavyDelay = 0.5f;
    [SerializeField] private float heavyMult;
    //[SerializeField] private float heavyForce;

    [Header("Special Attack Stats")]
    [SerializeField] private float specialAttackSpeed = 3.0f;
    [SerializeField] private float specialAttackDuration = 1.0f;
    [SerializeField] private float specialDelay = 0.3f;
    [SerializeField] private float specialMult;
    [SerializeField] private GameObject shockwaveParticles;
    
    [Header("Other Variables")]
    public float invulnLength;
    private float invulnTimer = 0f;
    public int comboCounter = 0;
    private float comboTimer = 0.0f;
    private float lightAttackCDTimer;
    private float heavyAttackCDTimer;
    private float specialAttackCDTimer;
    private float attackDurationTimer;
    public bool comboChain = false;
    public float lightAttackBuffer;
    private float lightAttackBufferTimer;
    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
        healthBar.SetMaxHealth(maxHP);
        vialTimer = vialRechargeSpeed;
        //spearHitbox = lightAttackHitbox.GetComponent<Collider>();
        lightAttackHitbox.SetActive(false);
        heavyAttackHitbox.SetActive(false);
        charController = gameObject.GetComponent<CharController>();
        attackPower = baseAttackPower;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // checks if light attack is off cooldown
        if(lightAttackCDTimer > 0) {
            lightAttackCDTimer -= Time.deltaTime;
        }

        // checks if heavy attack is off cooldown
        if(heavyAttackCDTimer > 0) {
            heavyAttackCDTimer -= Time.deltaTime;
        }

        // checks if special attack is off cooldown
        if(specialAttackCDTimer > 0) {
            specialAttackCDTimer -= Time.deltaTime;
        }

        // checks how long is left in current attack
        if(attackDurationTimer > 0) {
            attackDurationTimer -= Time.deltaTime;
        }

        // adds ElectricVials over time

        if(vialTimer > 0)
        {
            vialTimer -= Time.deltaTime;
        }
        else
        {
            electricVials.AddVial();
            vialTimer += vialRechargeSpeed;
        }

        // checks if light attack input pressed, if so buffer attack, else count down timer
        if(Input.GetButtonDown("LightAttack"))
        {
            lightAttackBufferTimer = lightAttackBuffer;
        }
        else
        {
            if(lightAttackBufferTimer >= 0)
            {
                lightAttackBufferTimer -= Time.deltaTime;
            }
        }


        if (comboChain == true) { // Harsha
            if (Time.time - comboTimer >= maxComboTimer) { // If the combo chain is activated (as in a combo was started), this checks if the next button was pressed within the time window alloted
                print("broken combo");
                comboChain = false; // If the next button wasn't pressed in time, then the combo chain is set to false
                lightAttackCDTimer += attackDelay; // Penality time to attack again is added
                comboCounter = 0; // combos set to 0 again
                charController.state = CharController.State.IDLE;
            }
        }

        // Start Light Attack
        if(lightAttackBufferTimer > 0 && lightAttackCDTimer <= 0 && charController.state != CharController.State.PAUSED) // Harsha and Justin
        {
            charController.state = CharController.State.ATTACKING;
            lightAttackCDTimer = 0; // lightAttackCDTimer is set to 0 because it is added to later on in code instead of being set equal to a certain value
            comboCounter++; // This counter is incremented whenever attack button is pressed and is used to check at what stage of the weak attack combo you are at
            charController.animator.SetInteger("lightAttackCombo", comboCounter);
            if (comboCounter == 1) {
                comboChain = true; // sets comboChain to true to initiate combo
                print("first hit");
                comboTimer = Time.time; // logs the time the button was pressed to check for the next time light attack button is pressed
                LightAttack(baseAttackPower);
                StartCoroutine(DisableWeapon());

            }
            else if (comboCounter == 2 && comboChain == true) { // checks whether second button press in combo was accomplished within max limit for combo button press timer
                //charController.animator.SetBool("isLightAttacking2", true);
                print("second hit!");
                comboTimer = Time.time; // ComboTimer is used to check if next button press is within the maxComboTimer limit
                LightAttack(baseAttackPower * 1.3f);
                StartCoroutine(DisableWeapon());
            }
            else if (comboCounter == 3 && comboChain == true && electricVials.currVial >= 1) { // checks whether third button press in combo was accomplished within max limit for combo button press timer
                //charController.animator.SetBool("isLightAttacking3", true);
                print("third hit!");
                
                comboChain = false; // resets combo variables for next time
                lightAttackCDTimer += attackDelay; // delay for next combo
                lightAttackCDTimer += lightAttackSpeed;

                // remove 1 electric vial
                electricVials.RemoveVials(1);

                LightAttack(baseAttackPower * 1.6f);
                StartCoroutine(DisableWeapon());
                
                StartCoroutine(EndAnim());

            }
            //isAttacking = false;
        }
        // Start heavy Attack
        if (Input.GetButtonDown("HeavyAttack") && heavyAttackCDTimer <= 0 && electricVials.currVial >= 1 &&
            charController.state != CharController.State.ATTACKING && charController.state != CharController.State.PAUSED) // Harsha and Justin
        {
            attackDurationTimer = heavyAttackDuration;
            charController.state = CharController.State.ATTACKING;
            charController.animator.SetBool("isHeavyAttacking", true);

            // remove 1 electric vial
            electricVials.RemoveVials(1);

            // start heavy attack function after 'heavyDelay' delay. This imitates a wind up feature
            Invoke(nameof(HeavyAttack), heavyDelay);
            StartCoroutine(DisableWeapon());
            StartCoroutine(EndAnim());
        }

        if(Input.GetButtonDown("SpecialAttack") && specialAttackCDTimer <= 0 && electricVials.currVial >= 2 &&
           charController.state != CharController.State.ATTACKING && charController.state != CharController.State.PAUSED)
        {
            print("START SPECIAL ATTACK");
            attackDurationTimer = specialAttackDuration;
            charController.state = CharController.State.ATTACKING;

            // remove 2 electric vials
            electricVials.RemoveVials(2);

            // start special attack funtion after 'specialDelay' delay. This imitates a wind up feature
            Invoke(nameof(SpecialAttack), specialDelay);
        }

        // End Light Attack | End Heavy Attack | End Special Attack
        if(charController.state == CharController.State.ATTACKING && attackDurationTimer <= 0) 
        {   
            // resets drag
            _rb.drag = 20;
            //charController.state = CharController.State.IDLE;
            //lightAttackHitbox.SetActive(false);
        }

        //Checks to see if the player is dead
        if(currHP <= 0) 
        {
            print("Player is dead");
            charController.animator.SetBool("isDead", true);
            //gameObject.SetActive(false); //do not destroy the objest, otherwise it causes an error in the enemy scripts which try to find the player - Elizabeth
        }

    }

    private void LightAttack(float ap) 
    {
        //isAttacking = true;
        lightAttackCDTimer += lightAttackSpeed;

        // speed of animations is different
        if(comboCounter == 3)
        {
            attackDurationTimer = lightAttackDuration * 1.15f;
            comboCounter = 0;
            StartCoroutine(AttackForce(1.25f, 0.4f));
        }
        else
        {
            attackDurationTimer = lightAttackDuration;
            StartCoroutine(AttackForce(1f, 0.1f));
        }
        attackPower = ap; //the Spear script references this variable when determining how much damage to do. It will use attackPower at the moment the collision starts.
        print(attackPower);
        lightAttackHitbox.SetActive(true);
        
    }

    private void HeavyAttack() // Harsha and Justin
    {
        print("in heavy attack");
        heavyAttackCDTimer = heavyAttackSpeed;
        attackPower = baseAttackPower * heavyMult; //the Spear script references this variable when determining how much damage to do. It will use attackPower at the moment the collision starts.
        heavyAttackHitbox.SetActive(true);

        // push player forward a bit | remove drag from player
        StartCoroutine(AttackForce(2.5f, 0.1f));
    }

    private void SpecialAttack() // Harsha and Justin
    {
        print("in special attack");
        specialAttackCDTimer = specialAttackSpeed;
        attackPower = baseAttackPower * specialMult;

        StartCoroutine(ShockwaveEffect());

        Collider[] colliders = Physics.OverlapSphere(transform.position, 4f);
        foreach (Collider c in colliders)
        {
            if(c.gameObject.tag == "Enemy")
            {
                c.gameObject.GetComponent<EnemyStats>().currHP -= attackPower;
                print("Did " + attackPower + " damage to " + c.gameObject.GetComponent<EnemyStats>().nameID);
            }
        }
        return;
    }

    // ends the animation of light attack 3
    IEnumerator EndAnim()
    {
        yield return new WaitForSeconds(attackDurationTimer);
        if(charController.direction != charController.zeroVector)
        {
            charController.state = CharController.State.MOVING;
        }
        else
        {
            charController.state = CharController.State.IDLE;
        }
        
    }

    // how much forward force is added to every attack
    IEnumerator AttackForce(float multiplier, float forceDelay)
    {
        yield return new WaitForSeconds(forceDelay);
        Vector3 forceToApply = transform.forward * force;
        _rb.drag = 0;
        _rb.AddForce(forceToApply * multiplier, ForceMode.Impulse);
    }

    // disable hitbox after attack is over
    IEnumerator DisableWeapon() 
    {
        yield return new WaitForSeconds(attackDurationTimer - 0.01f);
        lightAttackHitbox.SetActive(false);
        heavyAttackHitbox.SetActive(false);
        charController.attacking = false;
    }

    private IEnumerator ShockwaveEffect() // Harsha and Justin
    {
        shockwaveParticles.SetActive(true);
        yield return new WaitForSeconds(specialAttackDuration - specialDelay);
        shockwaveParticles.SetActive(false);
    }

    public void TakeDamage(float dmg) // Justin
    {
        if(Time.time - invulnTimer >= invulnLength && charController.canTakeDmg == true)
        {
            // does dmg
            currHP -= dmg;
            invulnTimer = Time.time;
            healthBar.SetHealth(currHP);
            StartCoroutine(damageFlash.FlashRoutine());
            print("took damage");

        }
    }
}
