using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public float dir;
    public float speed = 1f;
    public float jumpPower = 1f;
    bool isjump;
    int jumpcount;

    Rigidbody2D rb;
    Transform playerTransform;

    bool facingRight = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<float>();
    }
    void Move()
    {
        // spriteFlip
        if (dir < 0 && facingRight)
        {
            Flip();
        }
        else if (dir > 0 && !facingRight)
        {
            Flip();
        }

        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (jumpcount < 2)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpcount++;
                Debug.Log(jumpcount);
            }
        }
    }
    public void OnDownJump(InputAction.CallbackContext context)
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            jumpcount = 0;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
    }
}
