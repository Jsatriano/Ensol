using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class OldPlayerCombatController : MonoBehaviour
{
    //this whole class is deprecated in favor of PlayerController
    //its still in the files in order to have notes on how it used to work vs now, but do not use it for anything
    [Header("References")]
    public GameObject weapon;
    public GameObject weaponHead;
    public GameObject weaponBase;
    public GameObject backpack;
    public GameObject weaponProjectilePrefab;
    public GameObject weaponCatchTarget;
    public GameObject FX1;
    public GameObject FX2;
    public GameObject lightHitbox;
    public GameObject heavyHitbox;
    private OldCharController charController;
    private Rigidbody _rb;
    //public FadeOnDeath fadeOnDeath;
    public HealthBar healthBar;
    public ElectricVials electricVials;
    public DamageFlash damageFlash;
    public Material backpackVialMaterial;
    private Queue<GameObject> activeDamageVFX = new Queue<GameObject>();
    public GameObject shieldVisual;


    [Header("Player Stats & Variables")]
    public float maxHP = 10;
    public float currentHP = PlayerData.currHP;
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

    [Header("Shield Variables")]
    /*public float shieldDuration = 2f;
    public float shieldMaxHealth = 10f;
    private float shieldCurrHealth = 0f;
    public float shieldCooldown = 10f;
    public bool shieldCanActivate = true;
    public bool shieldIsActive = false;*/
    
    [Header("Other Variables")]
    [HideInInspector] public float attackPower;
    private float invulnTimer = 0f;
    [HideInInspector] public bool comboChain = false;
    [HideInInspector] public int comboCounter = 0;
    private float comboTimer = -1f;
    [HideInInspector] public bool comboTimerActive = false;
    private bool acceptingInput = true;
    private bool isNextAttackBuffered = false;
    private GameObject activeWeaponProjectile;
    private Vector3 throwAim;
    private bool dying = false;
    [HideInInspector] public bool isMidGrab = false;

    [Header("VFX References")]
    public GameObject[] lightSlashVFX;
    public GameObject heavySlashVFX;
    public GameObject damageVFX;
    public Transform damageVFXLocation;
    public GameObject shieldBreakVFX;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerData.currHP == -1){
            PlayerData.currHP = maxHP;
        }
        healthBar.SetMaxHealth(maxHP);
        vialTimer = vialRechargeSpeed;
        charController = gameObject.GetComponent<OldCharController>();
        attackPower = baseAttackPower;
        _rb = GetComponent<Rigidbody>();
        if (!PlayerData.currentlyHasBroom && !PlayerData.currentlyHasSolar)
        {
            charController.animator.SetBool("hasBroom", false);
            charController.animator.SetBool("hasWeapon", false);
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
            charController.animator.SetBool("hasBroom", true);
            charController.animator.SetBool("hasWeapon", true);
            weapon.SetActive(true);
            weaponHead.SetActive(false);
            weaponBase.SetActive(false);
            backpack.SetActive(false);
            FX1.SetActive(false);
            FX2.SetActive(false);
        }
        else
        {
            charController.animator.SetBool("hasBroom", true);
            charController.animator.SetBool("hasWeapon", true);
            weapon.SetActive(true);
            weaponHead.SetActive(true);
            weaponBase.SetActive(true);
            backpack.SetActive(true);
            FX1.SetActive(true);
            FX2.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*makes hp visible for debugging purposes*/
        currentHP = PlayerData.currHP;

        if (PauseMenu.isPaused)
        {
            return;
        }
        if (activeWeaponProjectile != null && !activeWeaponProjectile.activeSelf) {
            activeWeaponProjectile.SetActive(true);
            Destroy(activeWeaponProjectile);
        }
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

        shieldVisual.SetActive(PlayerData.hasShield);

        charController.animator.SetBool("hasWeapon", hasWeapon);
        
        if(isCatching) {
            Collider[] weaponSearch = Physics.OverlapSphere(weaponCatchTarget.transform.position, weaponCatchDistance);
            foreach(Collider col in weaponSearch) {
                if(col.gameObject.tag == "WeaponProjectile") {
                    //print("located catchable weapon");
                    if(charController.state != OldCharController.State.DASHING) {
                        charController.animator.SetBool("isCatching", true);
                    }
                    else {
                        GrabWeapon();
                    }
                    isCatching = false;
                }
            }

        }

        if(charController.animator.GetBool("isCatching") && hasWeapon && charController.state != OldCharController.State.ATTACKING) {
            charController.animator.SetBool("isCatching", false);
        } 
        if(charController.animator.GetBool("isThrowing") && hasWeapon && charController.state != OldCharController.State.ATTACKING) {
            charController.animator.SetBool("isThrowing", false);
        } 

        if(charController.state == OldCharController.State.KNOCKBACK) {
            ResetLightAttackCombo();
            charController.animator.SetBool("isHeavyAttacking", false);
        }

        if(comboChain && comboTimerActive && Input.GetButtonDown("Dash")) {
            ResetLightAttackCombo();
        }

        /* Shield ability disabled in favor of shield working as a pickup
        //Activate Shield- Elizabeth
        if(Input.GetButtonDown("Shield") && shieldCanActivate && electricVials.enoughVials(2) && PlayerData.hasShield) {
            shieldCanActivate = false; 
            shieldIsActive = true;
            shieldCurrHealth = shieldMaxHealth;
            shieldVisual.SetActive(true);
            StartCoroutine(ShieldCooldown());
            StartCoroutine(DisableShield());
            electricVials.RemoveVials(2);
        }*/

        

        //Start Light Attack //Harsha Justin and Elizabeth
        if(Input.GetButtonDown("LightAttack") 
           && charController.state != OldCharController.State.PAUSED
           && charController.state != OldCharController.State.DIALOGUE 
           && charController.state != OldCharController.State.DASHING
           && !charController.animator.GetBool("isHeavyAttacking")
           && acceptingInput && hasWeapon && !isNextAttackBuffered && comboCounter < 3) 
        {
            PlayerData.lightAttacks += 1;
            charController.state = OldCharController.State.ATTACKING;
            comboCounter++;
            charController.animator.SetInteger("lightAttackCombo", comboCounter);
            comboTimer = -1f;
            comboTimerActive = false;
            acceptingInput = false;
            isNextAttackBuffered = true;
            //print("input taken COMBO COUNTER " + comboCounter.ToString());
        }

        if (comboChain && comboTimerActive && comboTimer != -1f) { // Harsha and Elizabeth
            if (Time.time - comboTimer >= maxComboTimer) { // If the combo chain is activated (as in a combo was started), this checks if the next button was pressed within the time window alloted
                //print("broken combo, COMBO COUNTER " + comboCounter.ToString());
                ResetLightAttackCombo();
            }
        } 

        if(charController.state != OldCharController.State.ATTACKING && hasWeapon) {
            charController.animator.SetInteger("lightAttackCombo", 0);
            if(!isNextAttackBuffered || !comboChain) {
                acceptingInput = true;
            }
            if(lightHitbox.activeSelf) {
                lightHitbox.SetActive(false);
            }
            if(heavyHitbox.activeSelf) {
                heavyHitbox.SetActive(false);
            }
            isCatching = false;
        }


        // Start heavy Attack
        if (Input.GetButtonDown("HeavyAttack") && charController.state != OldCharController.State.PAUSED 
            && charController.state != OldCharController.State.ATTACKING && charController.state != OldCharController.State.DASHING 
            && hasWeapon && PlayerData.hasSolarUpgrade && electricVials.enoughVials(2)) // Harsha and Justin and Elizabeth
        {
            ResetLightAttackCombo();
            PlayerData.heavyAttacks += 1;

            charController.state = OldCharController.State.ATTACKING;

            charController.animator.SetBool("isHeavyAttacking", true);
            
            // remove 1 electric vial
            electricVials.RemoveVials(2);

            //sfx
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponHeavyPrep, this.transform.position);
        }
        if(PlayerData.currHP <= 0) 
        {
            //print("Player is dead");
            if (SceneManager.GetActiveScene().name == "PlaytestingScene")
            {
                PlayerData.currHP = -1;
                SceneManager.LoadScene("PlaytestingScene");
                return;
            }
            charController.state = OldCharController.State.DEAD;
            PlayerData.currentlyHasBroom = false;
            PlayerData.currentlyHasSolar = false;
            PlayerData.currentNode = 1;
            charController.animator.SetBool("isDead", true);
            //sfx
            if (dying == false){
                PlayerData.deaths += 1;
                dying = true;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.playerDeath, this.transform.position);
                AudioManager.instance.PlayOneShot(FMODEvents.instance.deathCut, this.transform.position);
            }
        } else {
            dying = false;
        }

        if(Input.GetButtonDown("SpecialAttack") && !hasWeapon && !isCatching &&
        !charController.animator.GetBool("isCatching") && !charController.animator.GetBool("isThrowing")
           && charController.state != OldCharController.State.DIALOGUE 
           && charController.state != OldCharController.State.DASHING && PlayerData.hasThrowUpgrade) {
           // print("activated catching");
            isCatching = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecial, this.transform.position);
        }

        if(Input.GetButtonDown("SpecialAttack") && hasWeapon && !isCatching && 
        !charController.animator.GetBool("isThrowing") && !charController.animator.GetBool("isCatching") 
           && charController.state != OldCharController.State.DIALOGUE 
           && charController.state != OldCharController.State.DASHING && PlayerData.hasThrowUpgrade && electricVials.enoughVials(2)) {
            PlayerData.throwAttacks += 1;
            charController.state = OldCharController.State.ATTACKING;
            hasWeapon = false;
            charController.animator.SetBool("hasWeapon", hasWeapon);
            LookAtMouse();
            charController.animator.SetBool("isThrowing", true);
            electricVials.RemoveVials(2);
            acceptingInput = false;
            //sfx
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecialPrep, this.transform.position);

        }
    }

    public void PickedUpBroom()
    {
        charController.animator.SetBool("hasBroom", true);
        charController.animator.SetBool("hasWeapon", true);
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
        charController.animator.SetBool("hasBroom", true);
        charController.animator.SetBool("hasWeapon", true);
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
        charController.animator.SetBool("hasBroom", true);
        charController.animator.SetBool("hasWeapon", true);
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
        charController.animator.SetBool("hasBroom", true);
        charController.animator.SetBool("hasWeapon", true);
        weapon.SetActive(true);
        weaponHead.SetActive(true);
        weaponBase.SetActive(true);
        backpack.SetActive(true);
        FX1.SetActive(true);
        FX2.SetActive(true);
    }

    private void SpawnSpecialAttackProjectile() {
        attackPower = baseAttackPower * specialAttackMult;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecial, this.transform.position);

        LookAtMouse();
        activeWeaponProjectile = Instantiate(weaponProjectilePrefab, weaponCatchTarget.transform.position, charController.transform.rotation);
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
        charController.state = OldCharController.State.IDLE;
        isMidGrab = false;
    }

    private void BeginCatch() {
        isMidGrab = true;
        charController.state = OldCharController.State.ATTACKING;
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerWeaponSpecialReturn, this.transform.position);
    }

    private void EndCatch() {
       // print("ending catch");
        isCatching = false;
        acceptingInput = true;
        charController.animator.SetBool("isCatching", false);
        charController.state = OldCharController.State.IDLE;
    }

    private void EnableLightAttackHitbox() {
        isNextAttackBuffered = false;
        acceptingInput = false;
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
            //AudioManager.instance.PlayOneShot(FMODEvents.instance.playerSpin, this.transform.position);
        }
            
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
        //print("enabled attack hitbox COMBO COUNTER " + comboCounter.ToString());
    }

    private void MarkComboTimer() {
        charController.state = OldCharController.State.IDLE;
        if(!isNextAttackBuffered) {
           comboTimer = Time.time;
            comboTimerActive = true;
            //print("marked combo timer COMBO COUNTER " + comboCounter.ToString()); 
        }
        else{
            //print("combo timer not needed COMBO COUNTER " + comboCounter.ToString());
            if(comboCounter == 3) {
                ResetLightAttackCombo();
                //print("reset because we wanted the next attack coming from the third hit of the combo");
            }
        }
        
    }


    private void AllowInput() {
        acceptingInput = true;
        isNextAttackBuffered = false;
        //print("allowing input COMBO COUNTER " + comboCounter.ToString());
    }

    private void DisableLightAttackHitbox() {
        lightHitbox.SetActive(false);
    }

    public void ResetLightAttackCombo() {
        //print("reset light attack combo- start func- COMBO COUNTER " + comboCounter.ToString());
        comboCounter = 0;
        charController.animator.SetInteger("lightAttackCombo", 0);
        attackPower = baseAttackPower;
        if(charController.state == OldCharController.State.ATTACKING) {
            charController.state = OldCharController.State.IDLE;
        }
        comboChain = false;
        comboTimerActive = false;
        acceptingInput = true;
        isNextAttackBuffered = false;
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
        charController.state = OldCharController.State.IDLE;
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
        if(comboCounter > 0 && comboCounter <= 3) {
            lightSlashVFX[comboCounter - 1].SetActive(true);
        }
        else{
            lightSlashVFX[2].SetActive(true);
        }
    }

    private void EndLightSlash()
    {
        if(comboCounter > 0 && comboCounter <= 3) {
            lightSlashVFX[comboCounter - 1].SetActive(false);
        }
        else{
            foreach(GameObject vfx in lightSlashVFX) {
                vfx.SetActive(false);
            }
        }
       // print("ended light slash vfx COMBO COUNTER " + comboCounter.ToString());
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

    // how much forward force is added to every attack
    private void AttackForce(float multiplier)
    {
        _rb.velocity = Vector3.zero;
        Vector3 forceToApply = transform.forward * force;
        _rb.drag = 1;
        _rb.AddForce(forceToApply * multiplier, ForceMode.Impulse);
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
                charController.state = OldCharController.State.KNOCKBACK;
                // print("took damage");

                AudioManager.instance.PlayOneShot(FMODEvents.instance.minorCut, this.transform.position);

                //play the damage vfx
                GameObject newDamageVFX = Instantiate(damageVFX, damageVFXLocation);
                activeDamageVFX.Enqueue(newDamageVFX);
                StartCoroutine(DeleteDamageVFX());
            }
            else{
                GameObject newDamageVFX = Instantiate(shieldBreakVFX, damageVFXLocation);
                ShieldPickup.playerShieldOn.stop(STOP_MODE.ALLOWFADEOUT);
                AudioManager.instance.PlayOneShot(FMODEvents.instance.playerShieldBreak, this.transform.position);
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

  /*  private IEnumerator ShieldCooldown() {
        yield return new WaitForSeconds(shieldCooldown);
        shieldCanActivate = true;
    }
    
    private IEnumerator DisableShield() {
        yield return new WaitForSeconds(shieldDuration);
        shieldIsActive = false;
        shieldVisual.SetActive(false);
    }*/

    private void ManageVialShader() {
        if(electricVials.currVial + 1 >= 3) {
            backpackVialMaterial.SetFloat("_Gradient_Clipping_Amount", 1f);
        }
        else if(electricVials.currVial + 1 == 2) {
            backpackVialMaterial.SetFloat("_Gradient_Clipping_Amount", 0.2f);
        }
        else if(electricVials.currVial + 1 == 1) {
            backpackVialMaterial.SetFloat("_Gradient_Clipping_Amount", 0.015f);
        }
        else {
            backpackVialMaterial.SetFloat("_Gradient_Clipping_Amount", -1f);
        }
    }
}