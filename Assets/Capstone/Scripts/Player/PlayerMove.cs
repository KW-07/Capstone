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
    private SpriteRenderer sRenderer;

    public bool playerFlipx;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sRenderer = gameObject.GetComponent<SpriteRenderer>();
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
        if (dir < 0)
        {
            playerFlipx = true;
            sRenderer.flipX = true;
        }
        else if (dir > 0)
        {
            playerFlipx = false;
            sRenderer.flipX = false;
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
       /* if (!isjump)
        {
            isjump = true;
        }*/
    }
    public void OnDownJump(InputAction.CallbackContext context)
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Ground")
        {
            jumpcount = 0;
            
        }
    }
}
