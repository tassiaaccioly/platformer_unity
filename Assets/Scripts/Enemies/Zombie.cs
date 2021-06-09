using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private int layerMask;
    private SpriteRenderer sprite;
    private Animator anim;

    private bool touchedWall;
    public Transform groundCheck;
    private bool facingRight;

    public float moveSpeed;
    public int health;
    public bool isDead;

    void Start()
    {
        touchedWall = false;
        facingRight = true;
        sprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        layerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
        if (isDead)
            return;

        if(touchedWall)
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            moveSpeed *= -1;
        }

        touchedWall = Physics2D.Linecast(transform.position, groundCheck.position, layerMask);
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Attack"))
        {
            DamageEnemy();
        }
    }

    private void DamageEnemy()
    {
        health--;

        //to call coroutine function we need to call the coroutine function inside the Unity function StartCoroutine()
        StartCoroutine(DamageEffect());

        if (health <= 0)
        {
            isDead = true;
            rb2d.velocity = new Vector2(0f, 0f);
            anim.SetBool("isDead", true);
            StartCoroutine(isDeadAnimation());
        }
    }

    IEnumerator DamageEffect()
    {
        float auxSpeed = moveSpeed;
        moveSpeed *= -1;
        sprite.color = Color.red;
        rb2d.AddForce(new Vector2(0f, 100f));
        //this next line makes the function wait for 0.1 seconds before continuing (setTimeout)
        yield return new WaitForSeconds(.2f);
        sprite.color = Color.white;
        moveSpeed *= -1;
        moveSpeed = auxSpeed;
    }

    IEnumerator isDeadAnimation()
    {
        yield return new WaitForSeconds(.8f);
        Destroy(gameObject);
    }
}
