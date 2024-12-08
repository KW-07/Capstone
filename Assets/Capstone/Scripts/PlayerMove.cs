using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    float dir;
    public float speed = 1f;
    public float jumpPower = 1f;
    bool isjump;
    int jumpcount;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (jumpcount < 2)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpcount++;
            Debug.Log(jumpcount);
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
