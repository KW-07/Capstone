using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // �̵�
    public float originalMoveSpeed = 1f;
    private float moveSpeed;
    private Vector3 stopPosition;
    [SerializeField]private List<float> activeSpeedMultipliers = new List<float>();

    // ����
    public float jumpPower = 1f;

    // �뽬
    public float teleportdis;

    Rigidbody2D rb;
    CapsuleCollider2D capsule;
    Transform playerTransform;
    Animator animator;

    // ��ųƮ��
    private PlayerSkills playerSkills;


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

        // ��ųƮ��
        playerSkills = new PlayerSkills();
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        switch(e.skillType)
        {
            case PlayerSkills.SkillType.MoveSpeed_1:
                SetMovementSpeed(1.5f);
                break;
            case PlayerSkills.SkillType.MoveSpeed_2:
                SetMovementSpeed(2.0f);
                break;
        }
    }

    private void Start()
    {
        moveSpeed = originalMoveSpeed;
    }

    private void Update()
    {
        if (GameManager.instance.isCommand)
            playerTransform.position = new Vector3(stopPosition.x, playerTransform.position.y, -5);
        else
            playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -5);
    }

    private void FixedUpdate()
    {
        Move();
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    // ��ųƮ��

    public PlayerSkills GetPlayerSkills()
    {
        return playerSkills;
    }

    public bool CanUseDoubleJump()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.DoubleJump);
    }

    private void SetMovementSpeed(float changeMoveSpeed)
    {
        this.moveSpeed = changeMoveSpeed;
    }

    // �̵�
    public void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<float>();
    }
    void Move()
    {
        // ��ȭ �� ������ ����
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

    public void moveStop()
    {
        stopPosition = new Vector3(transform.position.x, transform.position.y, -5);
    }

    // ���� ���
    public void ApplySpeedBuff(float multiplier, float duration)
    {
        if (multiplier <= 0) return;

        // List�� ���� ȿ�� �߰�
        activeSpeedMultipliers.Add(multiplier);
        // ���� ���� ������ ����ȿ�� ����
        UpdateMoveSpeed();

        StartCoroutine(RemoveSpeedBuffAfterDelay(multiplier, duration));
    }

    private IEnumerator RemoveSpeedBuffAfterDelay(float multiplier, float duration)
    {
        // ���� �ð� �ʰ� �� �������̴� ���� ����
        yield return new WaitForSeconds(duration);
        activeSpeedMultipliers.Remove(multiplier);

        // ���� ������ �����ǰ� ���� ������ ���� ���� ���� ���
        UpdateMoveSpeed();
    }

    // �̵��ӵ� ����
    private void UpdateMoveSpeed()
    {
        if (activeSpeedMultipliers.Count > 0)
        {
            float maxMultiplier = Mathf.Max(activeSpeedMultipliers.ToArray());
            moveSpeed = moveSpeed * maxMultiplier;
        }
        else
        {
            //moveSpeed = moveSpeed;
        }

        Debug.Log($"���� �̵� �ӵ�: {moveSpeed}");
    }

    // ����
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (GameManager.instance.nothingUI())
            {
                if(CanUseDoubleJump())
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
                else
                {
                    if (jumpCount < 1)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                        GameManager.instance.isGrounded = false;

                        jumpCount += 1;
                        animator.SetBool("isJumping", !GameManager.instance.isGrounded);
                    }
                }

                //if (jumpCount < 2)
                //{
                //    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                //    GameManager.instance.isGrounded = false;

                //    jumpCount += 1;
                //    animator.SetFloat("jumpCount", jumpCount);

                //    animator.SetBool("isJumping", !GameManager.instance.isGrounded);
                //}
            }
        }
    }

    // �Ʒ�����
    public void OnDownJump(InputAction.CallbackContext context)
    {
        if (context.performed && GameObject.FindWithTag("Platform").GetComponent<Platform>().isPlayer == true)
        {
            if (GameManager.instance.nothingUI())
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

    // �뽬
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

            // �뽬�� ���� �� �Ÿ�
            Vector2 dashDirection = facingRight ? Vector2.right : Vector2.left;
            float dashDistance = teleportdis;
            
            // �÷��̾��� �ݶ��̴� ũ�� üũ
            Vector2 playerColliderSize = gameObject.GetComponent<BoxCollider2D>().size;

            // �Ϲ� Raycast
            //RaycastHit2D hit = Physics2D.Raycast(rb.position, dashDirection, dashDistance, LayerMask.GetMask("Ground"));

            // �ڽ��� Raycast �� �� ������ ��
            RaycastHit2D hit = Physics2D.BoxCast(rb.position, playerColliderSize, 0f, dashDirection, dashDistance, LayerMask.GetMask("Ground"));

            // Ground�� ���̾ ���� ���𰡰� �����ٸ� �ش� ������Ʈ �ձ��� �뽬
            if (hit.collider != null)
            {
                float safeDistance = hit.distance - 0.1f;
                rb.MovePosition(rb.position + dashDirection * safeDistance);
                Debug.Log("Dash - �浹 ����!");
            }
            // ���ٸ� ���������� �뽬
            else
            {
                rb.MovePosition(rb.position + dashDirection * dashDistance);
                Debug.Log("Dash - ����");
            }
        }
    }

    // �浹
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            GameManager.instance.isGrounded = true;
            animator.SetBool("isJumping", !GameManager.instance.isGrounded);
            animator.SetBool("jumpCommanding", false);
            
            jumpCount = 0;
            animator.SetFloat("jumpCount", jumpCount);
        }
    }

    // ���� ������
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        PlayerCommand.instance.commandTimeUI.GetComponent<Transform>().Rotate(0, 180, 0);
        PlayerCommand.instance.pCommandUIGrid.GetComponent<Transform>().Rotate(0, 180, 0);
    }

    public void Save(ref PlayerMoveSavaData data)
    {
        data.position = transform.position;
    }

    public void Load(PlayerMoveSavaData data)
    {
        transform.position = data.position;
    }
}

[System.Serializable]
public struct PlayerMoveSavaData
{
    public Vector3 position;
}