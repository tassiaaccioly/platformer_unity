using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool touchedWall;
    public Transform groundCheck;
    private bool facingRight;

    public float moveSpeed;

    public int layerMask;

    void Start()
    {
        touchedWall = false;
        facingRight = true;
        rb2d = GetComponent<Rigidbody2D>();

        layerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
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
        rb2d.velocity = new Vector2(moveSpeed, 0f);
    }
}
