using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator anim;

    [SerializeField]
    private int moveSpeed = 5;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Transform groundCheck;

    private bool facingRight;

    private bool jumping;
    public int totalJump;
    private int maxJump;

    private bool grounded;

    // Attack
    public float attackRate;
    public Transform spawnAttack;
    public GameObject attackPrefab;
    private float nextAttack;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        grounded = true;
        anim = GetComponent<Animator>();
        facingRight = true;
        nextAttack = 0;
    }

    //update is called once per frame
    private void Update()
    {
        setAnimations();

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && maxJump > 0)
        {
            grounded = false;
            jumping = true;
        }

        if (grounded)
        {
            maxJump = totalJump;
        }

        if(Input.GetButtonDown("Fire1") && grounded && Time.time > nextAttack)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        float direction = Input.GetAxis("Horizontal");

        rb2D.velocity = new Vector2((Input.GetAxis("Horizontal")) * moveSpeed, rb2D.velocity.y);

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
        anim.SetTrigger("Attack");
        nextAttack = Time.time + attackRate;

        GameObject cloneAtk = Instantiate(attackPrefab, spawnAttack.position, spawnAttack.rotation);

        if(!facingRight)
        {
            cloneAtk.transform.eulerAngles = new Vector3(180, 0, 180);
        }
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
