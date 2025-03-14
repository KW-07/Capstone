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

    // 이동
    public float moveSpeed = 1f;
    private float defaultSpeed;
    [SerializeField]private List<float> activeSpeedMultipliers = new List<float>();

    // 점프
    public float jumpPower = 1f;
    bool isjump = true;

    // 대쉬
    public float teleportdis;

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
        defaultSpeed = moveSpeed;
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

            rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
        }
    }

    // 버프 사용
    public void ApplySpeedBuff(float multiplier, float duration)
    {
        if (multiplier <= 0) return;

        // List에 버프 효과 추가
        activeSpeedMultipliers.Add(multiplier);
        // 가장 높은 배율의 버프효과 적용
        UpdateMoveSpeed();

        StartCoroutine(RemoveSpeedBuffAfterDelay(multiplier, duration));
    }

    private IEnumerator RemoveSpeedBuffAfterDelay(float multiplier, float duration)
    {
        // 버프 시간 초과 후 적용중이던 버프 삭제
        yield return new WaitForSeconds(duration);
        activeSpeedMultipliers.Remove(multiplier);

        // 기존 버프가 해제되고 다음 순위의 가장 높은 버프 사용
        UpdateMoveSpeed();
    }

    // 이동속도 증감
    private void UpdateMoveSpeed()
    {
        if (activeSpeedMultipliers.Count > 0)
        {
            float maxMultiplier = Mathf.Max(activeSpeedMultipliers.ToArray());
            moveSpeed = defaultSpeed * maxMultiplier;
        }
        else
        {
            moveSpeed = defaultSpeed;
        }

        Debug.Log($"현재 이동 속도: {moveSpeed}");
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
                    animator.SetBool("jumping", true); 
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
                rb.MovePosition(new Vector2((teleportdis) + rb.position.x, rb.position.y));
            }
            else
            {
                rb.MovePosition(new Vector2((teleportdis * -1) + rb.position.x, rb.position.y));
            }
            Debug.Log("Dash");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            isjump = false;
            animator.SetBool("jumping", false);
            jumpCount = 0;

        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
    }
}
