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
        }
    }

    // 이동
    public float moveSpeed = 1f;
    private float defaultSpeed;
    [SerializeField]private List<float> activeSpeedMultipliers = new List<float>();

    // 점프
    public float jumpPower = 1f;

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
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<float>();
    }
    void Move()
    {
        // 대화 중 움직임 제어
        if(GameManager.instance.nothingUI())
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

            rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

            animator.SetFloat("xVelocity", dir);
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
            if (GameManager.instance.nothingUI())
            {
                if (jumpCount < 2)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    GameManager.instance.isGrounded = false;

                    jumpCount += 1;
                    animator.SetFloat("jumpCount", jumpCount);

                    animator.SetBool("isJumping", !GameManager.instance.isGrounded);
                }
            }
        }
    }
    //public void OnDownJump(InputAction.CallbackContext context)
    //{
    //    if(context.performed && GameObject.FindWithTag("Platform").GetComponent<Platform>().isPlayer == true)
    //    {
    //        if(GameManager.instance.nothingUI())
    //        {
    //            StartCoroutine("coDownJump");
    //        }
    //    }
    //}
    //IEnumerator coDownJump()
    //{
    //    WaitForFixedUpdate wait = new WaitForFixedUpdate();
    //    capsule.isTrigger = true;
    //    float y = transform.position.y;
    //    while (transform.position.y > y - 1.6f && transform.position.y <= y)
    //    {
    //        yield return wait;
    //    }
    //    capsule.isTrigger = false;
    //}

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //if (facingRight)
            //{
            //    rb.MovePosition(new Vector2((teleportdis) + rb.position.x, rb.position.y));
            //}
            //else
            //{
            //    rb.MovePosition(new Vector2((teleportdis * -1) + rb.position.x, rb.position.y));
            //}
            //Debug.Log("Dash");

            // 대쉬의 방향 및 거리
            Vector2 dashDirection = facingRight ? Vector2.right : Vector2.left;
            float dashDistance = teleportdis;
            
            // 플레이어의 콜라이더 크기 체크
            Vector2 playerColliderSize = gameObject.GetComponent<BoxCollider2D>().size;

            // 일반 Raycast
            //RaycastHit2D hit = Physics2D.Raycast(rb.position, dashDirection, dashDistance, LayerMask.GetMask("Ground"));

            // 박스형 Raycast 좀 더 안전할 듯
            RaycastHit2D hit = Physics2D.BoxCast(rb.position, playerColliderSize, 0f, dashDirection, dashDistance, LayerMask.GetMask("Ground"));

            // Ground의 레이어를 가진 무언가가 잡힌다면 해당 오브젝트 앞까지 대쉬
            if (hit.collider != null)
            {
                float safeDistance = hit.distance - 0.1f;
                rb.MovePosition(rb.position + dashDirection * safeDistance);
                Debug.Log("Dash - 충돌 감지!");
            }
            // 없다면 정상적으로 대쉬
            else
            {
                rb.MovePosition(rb.position + dashDirection * dashDistance);
                Debug.Log("Dash - 정상");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            GameManager.instance.isGrounded = true;
            animator.SetBool("isJumping", !GameManager.instance.isGrounded);
            
            jumpCount = 0;
            animator.SetFloat("jumpCount", jumpCount);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        PlayerCommand.instance.commandTimeUI.GetComponent<Transform>().Rotate(0, 180, 0);
        PlayerCommand.instance.pCommandUIGrid.GetComponent<Transform>().Rotate(0, 180, 0);
    }
}
