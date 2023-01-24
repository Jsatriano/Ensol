using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    //This entire class is for the most part very extremely placeholder. I intend to do some more work later to find a better system to use for attack combos.
    //Elizabeth
    [Header("Combat Stats")]
    [SerializeField] public int maxHP = 10;
    [HideInInspector] public int currHP;
    [SerializeField] private int baseAttackPower = 5;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float attackDuration = 0.3f;
    private float attackCDTimer;
    private float attackDurationTimer;
    public int attackPower; //used to calculate the real damage value of different attacks

    [Header("Other Variables")]
    [SerializeField] public GameObject placeholderSpear;
    private Collider spearHitbox;
    private CharController charController;


    // Start is called before the first frame update
    void Start()
    {
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
        if(Input.GetButtonDown("LightAttack") && attackCDTimer <= 0 
        && charController.state != CharController.State.ATTACKING) {
            LightAttack(baseAttackPower);
        }

        if(charController.state == CharController.State.ATTACKING && attackDurationTimer <= 0) {
            charController.state = CharController.State.IDLE;
            placeholderSpear.SetActive(false);
        }


    }

    private void LightAttack(int ap) {
        print("used light attack");
        attackCDTimer = attackSpeed;
        attackDurationTimer = attackDuration;
        attackPower = ap; //the Spear script references this variable when determining how much damage to do. It will use attackPower at the moment the collision starts.
        placeholderSpear.SetActive(true);
        charController.state = CharController.State.ATTACKING;
    }

    private void HeavyAttack(int ap) {
        attackPower = ap;
        return;
    }

    private void Special(int ap) {
        attackPower = ap;
        return;
    }
}
