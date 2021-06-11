using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Instances of Objects
    private Rigidbody2D rb2D;
    private Animator anim;
    private SpriteRenderer sprite;

    // Life
    public bool isDead;
    public int health;

    // Movement
    [SerializeField]
    private int moveSpeed = 5;
    private bool facingRight;

    // Ground
    [SerializeField]
    private Transform groundCheck;
    private bool grounded;

    //Jump
    [SerializeField]
    private float jumpForce;
    private bool jumping;
    public int totalJump;
    private int maxJump;

    // Attack
    public float attackRate;
    public Transform spawnAttack;
    public GameObject attackPrefab;
    private float nextAttack;

    //particles
    public GameObject particleDeath;
    public GameObject particleDamage;
    public GameObject particleRevive;
    public GameObject particleAttack;

    // public GameObject particleJump;

    // Audio
    public AudioClip fxHurt;
    public AudioClip fxJump;
    public AudioClip fxAttack;



    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        isDead = false;
        grounded = true;
        anim = GetComponent<Animator>();
        facingRight = true;
        maxJump = 0;
        nextAttack = 0;

        HudLife.instance.RefreshLife(health);
    }

    //update is called once per frame
    private void Update()
    {
        if (isDead)
            return;

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && maxJump > 0)
        {
            // Instantiate(particleJump, gameObject.transform.position, gameObject.transform.rotation);
            AudioManager.instance.PlaySound(fxJump);
            grounded = false;
            jumping = true;
        }

        if (grounded)
        {
            maxJump = totalJump;
        }

        //if button attack is pressed and character is on the ground and the time t
        if(Input.GetButtonDown("Fire1") && grounded && Time.time > nextAttack)
        {
            Attack();
        }

        setAnimations();
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        float direction = Input.GetAxis("Horizontal");

        rb2D.velocity = new Vector2(direction * moveSpeed, rb2D.velocity.y);

        //to change direction of character animation

        if ((direction < 0f && facingRight) || (direction > 0f && !facingRight)) 
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        //JUMPING
        if(jumping)
        {
            maxJump--;
            jumping = false;

            //rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
            rb2D.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void Attack()
    {
        AudioManager.instance.PlaySound(fxAttack);
        Instantiate(particleAttack, gameObject.transform.position, gameObject.transform.rotation);

        anim.SetTrigger("Attack");
        nextAttack = Time.time + attackRate;

        GameObject cloneAtk = Instantiate(attackPrefab, spawnAttack.position, spawnAttack.rotation);

        if(!facingRight)
        {
            cloneAtk.transform.eulerAngles = new Vector3(180, 0, 180);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (collision.CompareTag("Zombie"))
        {
            DamagePlayer();
        }
    }

    IEnumerator DamageEffect()
    {
        Instantiate(particleDamage, gameObject.transform.position, gameObject.transform.rotation);
        sprite.enabled = false;
        yield return new WaitForSeconds(.1f);
        sprite.enabled = true;
        yield return new WaitForSeconds(.1f);
        sprite.enabled = false;
        yield return new WaitForSeconds(.1f);
        sprite.enabled = true;

    }

    private void DamagePlayer()
    {
        health--;
        AudioManager.instance.PlaySound(fxHurt);

        HudLife.instance.RefreshLife(health);

        if(health == 0)
        {
            isDead = true;
            moveSpeed = 0;
            rb2D.velocity = new Vector2(0f, 0f);
            anim.SetTrigger("Dead");
            Instantiate(particleDeath, gameObject.transform.position, gameObject.transform.rotation);

            //this function runs a function after some defined time.
            Invoke("ReloadLevel", 1f);
        }
        else
        {
            StartCoroutine(DamageEffect());
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        Instantiate(particleRevive, gameObject.transform.position, gameObject.transform.rotation);
    }

    private void setAnimations()
    {
        anim.SetBool("isWalking", (rb2D.velocity.x != 0f));
        anim.SetFloat("VelY", rb2D.velocity.y);
        anim.SetBool("isJumping", !grounded);
    }
}


//if using playlearn way (not always recommended)
//rb2D.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb2D.velocity.y);
//it could be separated into two directions + a else with speed 0 using:
//rb2D.velocity = new Vector2(moveSpeed, 0) for right
//rb2D.velocity = new Vector2(-moveSpeed, 0) for left
//rb2D.velocity = new Vector2(0, 0) for stopping

//string buttonPressed;

/*public const string RIGHT = "right";
public const string LEFT = "left";

if(buttonPressed == RIGHT)
{
    //to give it a sense of sliding on ice, use "ForceMode2d.Force"
    //rb2D.AddForce(new Vector2(moveSpeed, 0), ForceMode2D.Impulse);
    rb2D.velocity = new Vector2(moveSpeed, 0);
}
else if(buttonPressed == LEFT)
{
    //rb2D.AddForce(new Vector2(-moveSpeed * -(Input.GetAxis("Horizontal")), 0), ForceMode2D.Impulse);
    rb2D.velocity = new Vector2(-moveSpeed, 0);
}
else if(buttonPressed == null)
{
    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
}

//getting player key presses and setting the direction of the movement 
if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
{
    buttonPressed = RIGHT;
}
else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
{
    buttonPressed = LEFT;
}
else
{
    buttonPressed = null;
}*/
