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
    [SerializeField]
    private bool _isMoving = false;
    public bool isMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }
    [SerializeField]
    private int _jumpCount = 0;
    public int jumpCount
    {
        get
        {
            return _jumpCount;
        }
        set
        {
            _jumpCount = value;
            if(value == 1)
            {
                animator.SetTrigger("isJump");
            }
            else if(value == 2)
            {
                animator.SetTrigger("isDoubleJump");
            }
        }
    }

    public float teleportdis;
    public float speed = 1f;
    public float jumpPower = 1f;
    bool isjump = true;

    Rigidbody2D rb;
    CapsuleCollider2D capsule;
    Transform playerTransform;
    Animator animator;

    public bool facingRight = true;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;

        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        playerTransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        isjump = true;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<float>();

        isMoving = dir != 0;
    }
    void Move()
    {
        // 대화 중 움직임 제어
        if(GameManager.instance.nothingState())
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
            if (GameManager.instance.nothingState())
            {
                if (jumpCount < 2)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    isjump = true;
                    jumpCount++;
                }
            }
        }
    }
    public void OnDownJump(InputAction.CallbackContext context)
    {
        if(context.performed && GameObject.FindWithTag("Platform").GetComponent<Platform>().isPlayer == true)
        {
            if(GameManager.instance.nothingState())
            {
                StartCoroutine("coDownJump");
            }
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
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (facingRight)
            {
                transform.position = new Vector2(teleportdis + rb.position.x, rb.position.y);
            }
            else
            {
                transform.position = new Vector2((teleportdis * -1) + rb.position.x, rb.position.y);
            }
            Debug.Log("Dash");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            isjump = false;
            jumpCount = 0;


        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
    }
}
