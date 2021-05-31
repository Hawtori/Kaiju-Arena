using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private string moveUp, moveDown, moveLeft, moveRight;
    [SerializeField] private string normalAttack, rangedAttack, specialAttack, blockAttack;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject damagedAnim;
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject lazer;
    [SerializeField] private GameObject shield;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaTime;
    [SerializeField] private float blockRatio;
    private GameObject lazerObject;
    private GameObject shieldObject;
    private Animator anim;
    private Vector2 movement;
    private Vector2 lastMovement;
    private PlayerEnums attacks = new PlayerEnums();
    private bool canMove = true;
    private bool canAttack = true;
    private Stamina stamina;

    [SerializeField] private bool facingLeft;
    
    public bool hit = false; //raycast, if this object is hit, change to true from other script
    public float hitDamage; //raycast, if this object is hit, change value from other script
    public HealthBar healthBar;
    public HealthBar staminaBar;

    #region timers
    public float attackResetTime;
    private float attackResetTimer = 0;
    private bool startAttackTimer = false;

    public float staminaResetTime;
    private float staminaResetTimer = 0;
    private bool startStaminaTimer = false;

    private float staminaTimer = 0;
    #endregion

    private void Start()
    {
        stamina = new Stamina(staminaTime, maxStamina);
        healthBar.setMaxHealth(GetComponent<Health>().maxHealth);
        staminaBar.setMaxHealth(stamina.getMaxStamina());
        anim = GetComponent<Animator>();
    }

    private void Update() {
        movement = Vector2.zero;
        if (canMove)
        {
            getInputs();
            if(canAttack)
                getAttacks();
            if (movement != Vector2.zero) lastMovement = movement;
        }
        resetAttacks();
        if (staminaTimer >= 2f)
        {
            startStaminaTimer = true;
            resetStamina();
        }
        else
        {
            staminaTimer += Time.deltaTime;
            startStaminaTimer = false;
        }

        if (hit && attacks.getMode() == 4)
        {
            GetComponent<Health>().decreaseHealth(hitDamage * blockRatio);
            stamina.useStamina(5);
            hit = false;
            healthBar.setHealth(GetComponent<Health>().getHealth());
        }
        else if (hit)
        {
            Vector3 pos = transform.position;
            pos.z -= 2;
            Instantiate(damagedAnim, pos, Quaternion.identity);
            GetComponent<Health>().decreaseHealth(hitDamage);
            hit = false;
            healthBar.setHealth(GetComponent<Health>().getHealth());
        }
        updateStaminaBar();
        if (!facingLeft)
        {
            if(lastMovement.x > 0)
                indicator.transform.localPosition = new Vector3(lastMovement.x / 2f, lastMovement.y / 2f, -3);
            else
                indicator.transform.localPosition = new Vector3(lastMovement.x / -2f, lastMovement.y / 2f, -3);
        }
        if (facingLeft)
        {
            if(lastMovement.x < 0)
                indicator.transform.localPosition = new Vector3(lastMovement.x / 2f, lastMovement.y / 2f, -3);
            else 
                indicator.transform.localPosition = new Vector3(lastMovement.x / -2f, lastMovement.y / 2f, -3);
        }
    }

    private void FixedUpdate() {
        movePlayer();
    }


    void getInputs() {
        if (Input.GetKey(moveUp)) movement.y++;
        if (Input.GetKey(moveDown)) movement.y--;
        if (Input.GetKey(moveLeft))
        {
            if((!facingLeft && transform.localScale.x > 0) || (facingLeft && transform.localScale.x < 0))
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
            movement.x--;
        }
        if (Input.GetKey(moveRight))
        {
            if((!facingLeft && transform.localScale.x < 0) || (facingLeft && transform.localScale.x > 0))
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
            movement.x++;
        }
    }

    void getAttacks() {
        if (Input.GetKeyDown(normalAttack)) {
            attacks.setMode(1);
            if(stamina.getStamina() > attacks.getStamina())
            {
                anim.SetInteger("Attack", 1);
                StartCoroutine(waitToAttack(0.25f));
            }

        }
        if (Input.GetKeyDown(rangedAttack)) {
            attacks.setMode(2);
            if (stamina.getStamina() > attacks.getStamina())
            {
                anim.SetInteger("Attack", 2);
                lazerObject = Instantiate(lazer, transform.position, Quaternion.identity);
                lazerObject.transform.parent = gameObject.transform;

                if (!facingLeft) //player 1
                {
                    if(lastMovement.normalized.y == 1 || lastMovement.normalized.y == -1)
                    {
                        lazerObject.transform.Rotate(0, 0, 90);
                    }
                    if((lastMovement.x > 0 && lastMovement.y > 0) || (lastMovement.x < 0 && lastMovement.y > 0))
                    {
                        lazerObject.transform.Rotate(0, 0, 45);
                    }
                    if ((lastMovement.x > 0 && lastMovement.y < 0) || (lastMovement.x < 0 && lastMovement.y < 0))
                    {
                        lazerObject.transform.Rotate(0, 0, -45);
                    }

                    lazerObject.transform.localPosition = new Vector3(lastMovement.normalized.x > 0 ? lastMovement.normalized.x : -lastMovement.normalized.x, lastMovement.normalized.y, -2);
                }
                else
                {
                    if (lastMovement.normalized.y == 1 || lastMovement.normalized.y == -1)
                    {
                        lazerObject.transform.Rotate(0, 0, 90);
                    }
                    if((lastMovement.x > 0 && lastMovement.y > 0) || (lastMovement.x < 0 && lastMovement.y > 0))
                    {
                        lazerObject.transform.Rotate(0, 0, -45);
                    }
                    if ((lastMovement.x > 0 && lastMovement.y < 0) || (lastMovement.x < 0 && lastMovement.y < 0))
                    {
                        lazerObject.transform.Rotate(0, 0, 45);
                    }
                    lazerObject.transform.localPosition = new Vector3(lastMovement.normalized.x > 0 ? -lastMovement.normalized.x : lastMovement.normalized.x, lastMovement.normalized.y, -2);

                }
                Destroy(lazerObject, 0.5f);
                StartCoroutine(waitToAttack(0.5f));
            }
        }
        if (Input.GetKeyDown(specialAttack) && false) {
            attacks.setMode(3);
            if (stamina.getStamina() > attacks.getStamina())
            {
                StartCoroutine(waitToAttack(1.5f));
            }
        }
        if (Input.GetKeyDown(blockAttack)) {
            attacks.setMode(4);
            if (stamina.getStamina() > attacks.getStamina())
            {
                //take some stamina
                shieldObject = Instantiate(shield, transform.position, Quaternion.identity);
                shieldObject.transform.parent = transform;
                Destroy(shieldObject, attackResetTime);
                stamina.useStamina(attacks.getStamina());
                staminaTimer = 0;
                //if they got hit, take more stamina and decrease hp

                startAttackTimer = true;
                startStaminaTimer = false;
                canMove = false;
                resetStamina();
            }
        }
    }

    void movePlayer() {
        if (rb.velocity.magnitude < maxSpeed)
            rb.AddForce(movement.normalized * speed);
        else movement = Vector2.zero;
        if (movement == Vector2.zero)
        {
            rb.velocity -= rb.velocity * 0.035f;
            if (rb.velocity.magnitude < 0.25) rb.velocity = Vector2.zero;
        }
    }

    void resetAttacks() {
        if (startAttackTimer)
        {
            attackResetTimer += Time.deltaTime;
            if(attackResetTimer >= attackResetTime)
            {
                startAttackTimer = false;
                attackResetTimer = 0;
                canMove = true;
                attacks.setMode(0);
            }
        }
    }

    void resetStamina()
    {
        if (startStaminaTimer)
        {
            if(staminaResetTimer >= staminaResetTime)
            {
                StartCoroutine(replenishStamina());
                //Debug.Log("STARTING TO ADD STAMINAAAA");
                startStaminaTimer = false;
                staminaResetTimer = 0;
            }
            else
                staminaResetTimer += Time.deltaTime;
        }
    }

    void attack() {
        //raycast some distance to see if they hit
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)lastMovement, lastMovement.normalized, attacks.getDistance());
        //if they hit, deal damage to it
        if (hit.collider != null)
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.gameObject.GetComponent<PlayerMovement>().hit = true;
                hit.collider.gameObject.GetComponent<PlayerMovement>().hitDamage = attacks.getDamage();
            }

        startAttackTimer = true;
        canMove = false;
        
        //take stamina
        stamina.useStamina(attacks.getStamina());
        staminaTimer = 0;
        StopCoroutine(replenishStamina());
    }

    void updateStaminaBar()
    {
        staminaBar.setHealth(stamina.getStamina());
    }

    IEnumerator replenishStamina()
    {
        staminaTimer = 0;
        while(stamina.getStamina() < stamina.getMaxStamina()) {
            yield return new WaitForSeconds(1f);
            stamina.addStamina();
            //Debug.Log("ADDING STAMINA: " + stamina.getStamina());
        }
        StopCoroutine(replenishStamina());
        
    }

    IEnumerator waitToAttack(float waitTime)
    {
        canAttack = false;
        canMove = false;
        yield return new WaitForSeconds(waitTime);
        attack();
        canAttack = true;
        anim.SetInteger("Attack", 0);
    }
}
