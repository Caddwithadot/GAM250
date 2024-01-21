using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementNEW : MonoBehaviour
{
    public bool amKnight = false;

    private Rigidbody2D rb;
    private float horizontalInput;
    public float moveSpeed = 7f;
    public float jumpForce = 18f;

    public bool isGrounded;
    public float coyoteTime = 0.075f;
    private float coyoteTimer = 0f;
    public float juffTime = 0.15f;
    private float juffTimer = 0f;

    private bool jumped;
    public float jumpHeldTime = 0f;

    private float jumpBoost = 5f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            juffTimer = juffTime;

            if (isGrounded || coyoteTimer > 0f)
            {
                Jump();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumped = false;

            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            jumpHeldTime = 0f;
        }

        if (isGrounded && juffTimer > 0f)
        {
            Jump();
        }

        if (!isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
            juffTimer -= Time.deltaTime;
        }

        if (jumped)
        {
            jumpHeldTime += Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (Time.timeScale > 0)
        {
            jumped = true;

            float jumpVelocity = Mathf.Sqrt(2 * jumpForce * Mathf.Abs(Physics2D.gravity.y));
            float additionalVelocity = jumpVelocity * (1 - Mathf.Exp(-jumpHeldTime * jumpBoost));

            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity + additionalVelocity);

            coyoteTimer = 0f;
            juffTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Environment" || collision.tag == "JumpThrough")
        {
            isGrounded = true;
            coyoteTimer = coyoteTime;
        }

        if(collision.tag == "Hazard" && amKnight)
        {
            isGrounded = true;
            coyoteTimer = coyoteTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Environment" || collision.tag == "JumpThrough")
        {
            isGrounded = false;
        }

        if (collision.tag == "Hazard" && amKnight)
        {
            isGrounded = false;
        }
    }
}
