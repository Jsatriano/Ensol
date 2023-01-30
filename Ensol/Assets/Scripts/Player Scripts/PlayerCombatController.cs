using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    //This entire class is for the most part very extremely placeholder. I intend to do some more work later to find a better system to use for attack combos.
    //Elizabeth
    [Header("References")]
    public GameObject placeholderSpear;
    private Collider spearHitbox;
    private CharController charController;
    private Rigidbody _rb;

    [Header("Health and Attack Stats")]
    public int maxHP = 10;
    public int currHP;
    public int attackPower; //used to calculate the real damage value of different attacks
    [SerializeField] private int baseAttackPower = 5;

    [Header("Light Attack Stats")]
    [SerializeField] private float lightAttackSpeed = 1f;
    [SerializeField] private float lightAttackDuration = 0.3f;

    [Header("Heavy Attack Stats")]
    [SerializeField] private float heavyAttackSpeed = 1.5f;
    [SerializeField] private float heavyAttackDuration = 0.5f;
    [SerializeField] private float heavyDelay = 0.5f;
    [SerializeField] private int heavyMult;
    [SerializeField] private float heavyForce;

    [Header("Special Attack Stats")]
    [SerializeField] private float specialAttackSpeed = 3.0f;
    [SerializeField] private float specialAttackDuration = 1.0f;
    [SerializeField] private float specialDelay = 0.3f;
    [SerializeField] private int specialMult;
    [SerializeField] private GameObject shockwaveParticles;
    
    [Header("Other Variables")]
    public float invulnLength;
    private float invulnTimer = 0f;
    private int comboCounter = 0;
    public float maxComboTimer = 2.0f;
    private float comboTimer = 0.0f;
    private float lightAttackCDTimer;
    private float heavyAttackCDTimer;
    private float specialAttackCDTimer;
    private float attackDurationTimer;

    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
        spearHitbox = placeholderSpear.GetComponent<Collider>();
        placeholderSpear.SetActive(false);
        charController = gameObject.GetComponent<CharController>();
        attackPower = baseAttackPower;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        print(attackDurationTimer);
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

        // Start Light Attack
        if(Input.GetButtonDown("LightAttack") && lightAttackCDTimer <= 0 && charController.state != CharController.State.ATTACKING) // Harsha and Justin
        {
            comboCounter++; // This counter is incremented whenever attack button is pressed and is used to check at what stage of the weak attack combo you are at
            if (comboCounter == 2 && (Time.time - comboTimer <= maxComboTimer)) { // checks whether second button press in combo was accomplished within max limit for combo button press timer
                print("second hit!");
                comboTimer = Time.time; // ComboTimer is used to check if next button press is within the maxComboTimer limit
                LightAttack(baseAttackPower);
            }
            else if (comboCounter == 3 && (Time.time - comboTimer <= maxComboTimer)) { // checks whether third button press in combo was accomplished within max limit for combo button press timer
                print("third hit!");
                LightAttack(baseAttackPower);
            }
            else { // This else statement is for a regular attack or if the light attack button was pressed beyond the maxComboTimer time limit
                print("first attack!");
                comboCounter = 1; // resets combo counter back to 1
                comboTimer = Time.time;
                LightAttack(baseAttackPower);
            }
        }

        // Start heavy Attack
        if (Input.GetButtonDown("HeavyAttack") && heavyAttackCDTimer <= 0 && charController.state != CharController.State.ATTACKING) // Harsha and Justin
        {
            attackDurationTimer = heavyAttackDuration;
            charController.state = CharController.State.ATTACKING;

            // start heavy attack function after 'heavyDelay' delay. This imitates a wind up feature
            Invoke(nameof(HeavyAttack), heavyDelay);
        }

        if(Input.GetButtonDown("SpecialAttack") && specialAttackCDTimer <= 0 && charController.state != CharController.State.ATTACKING)
        {
            print("START SPECIAL ATTACK");
            attackDurationTimer = specialAttackDuration;
            charController.state = CharController.State.ATTACKING;

            // start special attack funtion after 'specialDelay' delay. This imitates a wind up feature
            Invoke(nameof(SpecialAttack), specialDelay);
        }

        // End Light Attack | End Heavy Attack | End Special Attack
        if(charController.state == CharController.State.ATTACKING && attackDurationTimer <= 0) 
        {
            print("IN DIS BITCH");
            
            // resets drag
            _rb.drag = 20;
            charController.state = CharController.State.IDLE;
            placeholderSpear.SetActive(false);
        }

        //Checks to see if the player is dead
        if(currHP <= 0) 
        {
            print("Player is dead");
            Destroy(gameObject);
        }

    }

    private void LightAttack(int ap) 
    {
        print("in light attack");
        lightAttackCDTimer = lightAttackSpeed;
        attackDurationTimer = lightAttackDuration;
        attackPower = ap; //the Spear script references this variable when determining how much damage to do. It will use attackPower at the moment the collision starts.
        placeholderSpear.SetActive(true);
        charController.state = CharController.State.ATTACKING;
    }

    private void HeavyAttack() // Harsha and Justin
    {
        print("in heavy attack");
        heavyAttackCDTimer = heavyAttackSpeed;
        attackPower = baseAttackPower * heavyMult; //the Spear script references this variable when determining how much damage to do. It will use attackPower at the moment the collision starts.
        placeholderSpear.SetActive(true);

        // push player forward a bit | remove drag from player
        Vector3 forceToApply = transform.forward * heavyForce;
        _rb.drag = 0;
        _rb.AddForce(forceToApply, ForceMode.Impulse);
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

    private IEnumerator ShockwaveEffect() // Harsha and Justin
    {
        shockwaveParticles.SetActive(true);
        yield return new WaitForSeconds(specialAttackDuration - specialDelay);
        shockwaveParticles.SetActive(false);
    }

    public void TakeDamage(int dmg) // Justin
    {
        if(Time.time - invulnTimer >= invulnLength && charController.canTakeDmg == true)
        {
            // does dmg
            currHP -= dmg;
            invulnTimer = Time.time;
            print("took damage");
        }
    }
}
