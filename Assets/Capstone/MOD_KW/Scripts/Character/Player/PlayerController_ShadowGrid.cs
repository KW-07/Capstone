using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_ShadowGrid : CharacterBase_ShadowGrid
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float currentSpeed;

    [Header("Jump")]
    public float jumpForce = 5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    private bool isGrounded;

    [Header("Attack")]
    public int maxAttackCount = 3;
    public int currentAttackCount = 0;
    [SerializeField]private float initAttackCountTime = 1f;
    public float initAttackCountTimer;

    [Header("Arm")]
    public GameObject arm;

    private IPlayerState currentState;
    private Vector3 moveInput;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        rb.freezeRotation = true;
        animator = arm.GetComponent<Animator>();
        ChangeState(new PlayerIdleState(this));
    }

    private void Update()
    {
        currentState?.Update();
        GroundCheck();
        Timer();
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (HasMovementInput())
        {
            Vector3 velocity = moveInput.normalized * currentSpeed;
            Vector3 currentVelocity = rb.velocity;
            rb.velocity = new Vector3(velocity.x, currentVelocity.y, velocity.z);
        }
    }

    public void ChangeState(IPlayerState newState)
    {
        if (currentState != null && !currentState.CanTransitionTo(newState)) return;

        currentState?.Exit();
        currentState = newState;
        Debug.Log(newState);
        currentState.Enter();

        ApplyAnimationParameters(currentState.GetAnimationParameters());
    }

    void ApplyAnimationParameters(AnimationParameters parameters)
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(param.name, false);
            }
        }

        foreach (var param in parameters.Bools) animator.SetBool(param.Key, param.Value);
        foreach (var param in parameters.Ints) animator.SetInteger(param.Key, param.Value);
        foreach (var param in parameters.Floats) animator.SetFloat(param.Key, param.Value);
        foreach (var param in parameters.Triggers) animator.SetTrigger(param);
    }

    void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        moveInput = transform.right * x + transform.forward * z;

        currentSpeed = IsRunning() ? runSpeed : walkSpeed;
    }

    void Timer()
    {
        initAttackCountTimer -= Time.deltaTime;

        if (initAttackCountTimer < 0 && currentAttackCount != 0)
        {
            currentAttackCount = 0;
            animator.SetBool("isAttack", false);

            if (currentState is PlayerAttackState)
            {
                ChangeState(new PlayerIdleState(this));
            }
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void TriggerAttack()
    {
        if (currentAttackCount < maxAttackCount)
        {
            currentAttackCount++;
        }
        else
        {
            currentAttackCount = 0;
        }

        initAttackCountTimer = initAttackCountTime;

        animator.SetBool("isAttack", true);
        animator.SetInteger("attackCount", currentAttackCount);
    }

    public void ExecuteCommand()
    {
        // Command execution logic
    }

    public void OnAnimationEnd()
    {
        currentState?.OnAnimationEnd();
    }


    public bool IsGrounded() => isGrounded;
    public bool HasMovementInput() => moveInput != Vector3.zero;
    public bool IsRunning() => Input.GetKey(KeyCode.LeftShift);
    public bool IsJumpPressed() => Input.GetButtonDown("Jump");
    public bool IsAttackPressed() => Input.GetKeyDown(KeyCode.Mouse0);
    public bool IsCommandPressed() => Input.GetKeyDown(KeyCode.F);
    public void SetSpeed(float speed) => currentSpeed = speed;

    protected override void Die()
    {
        base.Die();

    }
}
