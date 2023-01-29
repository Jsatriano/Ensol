using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    //This entire class is for the most part very extremely placeholder. I intend to do some more work later to find a better system to use for attack combos.
    //Elizabeth
    [Header("Combat Stats")]
    [SerializeField] public int maxHP = 10;
    public int currHP;
    [SerializeField] private int baseAttackPower = 5;
    [SerializeField] private float lightAttackSpeed = 1f;
    [SerializeField] private float heavyAttackSpeed = 1.5f;
    [SerializeField] private float lightAttackDuration = 0.3f;
    [SerializeField] private float heavyAttackDuration = 0.5f;
    [SerializeField] private float heavyDelay = 0.5f;
    private float attackCDTimer;
    private float attackDurationTimer;
    public int attackPower; //used to calculate the real damage value of different attacks
    public int heavyMult;

    [Header("Other Variables")]
    [SerializeField] public GameObject placeholderSpear;
    private Collider spearHitbox;
    private CharController charController;
    public float invulnLength;
    private float invulnTimer = 0f;
    private int comboCounter = 0;
    public float maxComboTimer = 2.0f;
    private float comboTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
        spearHitbox = placeholderSpear.GetComponent<Collider>();
        placeholderSpear.SetActive(false);
        charController = gameObject.GetComponent<CharController>();
        attackPower = baseAttackPower;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(attackCDTimer > 0) {
            attackCDTimer -= Time.deltaTime;
        }
        if(attackDurationTimer > 0) {
            attackDurationTimer -= Time.deltaTime;
        }
        if(Input.GetButtonDown("LightAttack") && attackCDTimer <= 0 && charController.state != CharController.State.ATTACKING) { // Justin and Harsha
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

        if (Input.GetButtonDown("HeavyAttack") && attackCDTimer <= 0 && charController.state != CharController.State.ATTACKING) { // Harsha and Justin
            print("at line 74");
            attackDurationTimer = heavyAttackDuration;
            charController.state = CharController.State.ATTACKING;
            Invoke(nameof(HeavyAttack), heavyDelay);
        }

        if(charController.state == CharController.State.ATTACKING && attackDurationTimer <= 0) {
            print(attackDurationTimer);
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
        attackCDTimer = lightAttackSpeed;
        attackDurationTimer = lightAttackDuration;
        attackPower = ap; //the Spear script references this variable when determining how much damage to do. It will use attackPower at the moment the collision starts.
        placeholderSpear.SetActive(true);
        charController.state = CharController.State.ATTACKING;
    }

    private void HeavyAttack() // Harsha and Justin
    {
        print("in heavy attack");
        attackCDTimer = heavyAttackSpeed;
        
        attackPower = baseAttackPower * heavyMult; //the Spear script references this variable when determining how much damage to do. It will use attackPower at the moment the collision starts.
        placeholderSpear.SetActive(true);
    }

    private void Special(int ap) // 
    {
        attackPower = ap;
        return;
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
