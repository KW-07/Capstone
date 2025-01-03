using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance { get; private set; }

    public float dir;
    public float teleportdis;
    public float speed = 1f;
    public float jumpPower = 1f;
    bool isjump;
    int jumpcount;

    Rigidbody2D rb;
    CapsuleCollider2D capsule;
    Transform playerTransform;

    public bool facingRight = true;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;

        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        playerTransform = GetComponent<Transform>();
    }

    private void Start()
    {

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
        // 대화 중 움직임 제어
        if(UIManager.instance.isConversaiton != true)
        {
            // spriteFlip
            if (dir < 0 && facingRight)
            {
                Flip();
                PlayerCommand.instance.commandTimeUI.GetComponent<Transform>().Rotate(0, 180f, 0);
            }
            else if (dir > 0 && !facingRight)
            {
                Flip();
                PlayerCommand.instance.commandTimeUI.GetComponent<Transform>().Rotate(0, 180f, 0);
            }

            rb.velocity = new Vector2(dir * speed, rb.velocity.y);
        }
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
        if(context.performed)
        {
            StartCoroutine("coDownJump");
        }
    }
    IEnumerator coDownJump()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        capsule.isTrigger = true;
        float y = transform.position.y;
        while (transform.position.y > y - 1.6f && transform.position.y <= y)
        {
            yield return wait;
        }
        capsule.isTrigger = false;
       /* while (GameObject.FindWithTag("Platform").GetComponent<Platform>().isPlayer == true)
        {
            yield return wait;
            if(GameObject.FindWithTag("Platform").GetComponent<Platform>().isPlayer == false)
            {
                break;
            }
        }*/
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            transform.position = new Vector2(dir * teleportdis, rb.position.y);
            Debug.Log("Dash");
        }
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
