using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossDaru : LivingEntity
{
    public Transform playerTransform;

    [Header("Move")]
    public float moveSpeed = 2.0f;

    [Header("PatrolJump")]
    public float jumpForce = 5.0f;
    public float jumpInterval = 2.0f;
    private float nextJumpTime = 0f;

    [Header("AttackBase")]
    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;
    private bool isattack = false;

    [Header("Attack1")]
    public Vector2 attack1BoxSize;
    public float attackDelay = 1.0f;  // ���� 1�� ��� �ð�

    [Header("Attack2")]
    public Vector2 attack2BoxSize;    // ���� ����
    public float jumpHeight = 3.0f;      // ���� ����
    public float slamSpeed = 10.0f;      // ������� �ӵ�
    public float groundCheckDistance = 0.5f;  // ���� ���� �Ÿ�
    private bool isSlamming = false;     // ������� ���� ����
    private bool isJumping = false;      // ���� ���� ����

    [Header("SpecialPattern")]
    public float teleportHeight = 2.5f;   // �÷��̾� �Ӹ� ���� ������� �� ������
    public float teleportCooldown = 5.0f; // �ڷ���Ʈ ���� ���ð�
    private float nextTeleportTime = 0f;

    [Header("Item")]
    public bool itemDrop;

    public int nextMoveTime = 3;
    private int nextMove;

    private Rigidbody2D rb;
    private BTSelector root;

    private bool isDead = false;
    private bool isPlayerDetected = false;  // �÷��̾� ���� ����
    private bool isPlayerInRange = false;   // ���� ���� �ȿ� �ִ��� ����

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Invoke("Think", nextMoveTime);

        root = new BTSelector();

        BTSequence attackSequence = new BTSequence();
        BTSequence specialPatternLoop = new BTSequence();
        BTSequence chaseSequence = new BTSequence();
        BTSequence patrolSequence = new BTSequence();
        BTSequence deathSequence = new BTSequence();

        BTRandomSelector normalAttackSelector = new BTRandomSelector();
        BTRandomSelector lowHealthAttackSelector = new BTRandomSelector();

        BTAction attack1 = new BTAction(Attack1);
        BTAction attack2 = new BTAction(Attack2);
        BTAction attack3 = new BTAction(Attack3);
        BTAction teleportAbovePlayer = new BTAction(TeleportAbovePlayer);
        BTAction chaseAction = new BTAction(Chase);
        BTAction patrolAction = new BTAction(Patrol);
        BTAction dieAction = new BTAction(Die);

        BTCondition playerInRange = new BTCondition(() => isPlayerInRange);
        BTCondition playerDetected = new BTCondition(() => isPlayerDetected);
        BTCondition lowHealth = new BTCondition(IsLowHealth);
        BTCondition isdeadCondition = new BTCondition(() => base.dead);
        BTCondition canAttack = new BTCondition(CanAttack);
        BTCondition canTeleport = new BTCondition(CanTeleport);

        normalAttackSelector.AddChild(attack1);
        normalAttackSelector.AddChild(attack2);

        lowHealthAttackSelector.AddChild(attack1);
        lowHealthAttackSelector.AddChild(attack2);
        lowHealthAttackSelector.AddChild(attack3);

        specialPatternLoop.AddChild(lowHealth);
        specialPatternLoop.AddChild(playerDetected);
        specialPatternLoop.AddChild(canTeleport);  // �ڷ���Ʈ ������ ���� ����
        specialPatternLoop.AddChild(teleportAbovePlayer);
        specialPatternLoop.AddChild(playerInRange);
        specialPatternLoop.AddChild(canAttack);
        specialPatternLoop.AddChild(lowHealthAttackSelector);

        attackSequence.AddChild(playerInRange);
        attackSequence.AddChild(canAttack);
        attackSequence.AddChild(normalAttackSelector);

        chaseSequence.AddChild(playerDetected);
        chaseSequence.AddChild(chaseAction);

        patrolSequence.AddChild(patrolAction);

        deathSequence.AddChild(isdeadCondition);
        deathSequence.AddChild(dieAction);

        root.AddChild(deathSequence); // ���� �߿���
        root.AddChild(specialPatternLoop);
        root.AddChild(attackSequence);
        root.AddChild(chaseSequence);
        root.AddChild(patrolSequence);
    }

    private void Update()
    {
        root.Evaluate();
    }

    private bool IsLowHealth() => base.currentHealth <= base.maxHealth * 0.4f; // ���ٽ�
    private bool CanAttack() => Time.time >= nextAttackTime;
    private void SetNextAttackTime() => nextAttackTime = Time.time + attackCooldown;
    private bool CanTeleport() => Time.time >= nextTeleportTime;

    #region Attack1code
    private BTNodeState Attack1()
    {
        LookAtPlayer();
        Debug.Log("Preparing Attack 1...");
        Invoke(nameof(PerformForwardAttack), attackDelay);
        isattack = false;
        SetNextAttackTime();
        return BTNodeState.Success;
    }
    private void PerformForwardAttack()
    {
        isattack = true;
        Vector2 attackPosition = transform.position + transform.right * 2f; // ĳ���� ����

        Debug.Log("Performing Attack 1 after delay!");
        Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(attackPosition, attack1BoxSize, 0);
        foreach (var player in hitPlayer)
        {
            if (player.CompareTag("Player"))
            {
                Debug.Log("Hit Player!");
                player.GetComponent<LivingEntity>().OnDamage(10);
            }
        }
    }
    #endregion

    private BTNodeState Attack2()
    {
        //if (!isJumping && !isSlamming)
        LookAtPlayer();
        Debug.Log("Jumping for Attack 2...");
        StartCoroutine(JumpAndSlam());
        isattack = false;
        SetNextAttackTime();
        return BTNodeState.Success;
    }
    private IEnumerator JumpAndSlam()
    {
        isattack = true;
        isJumping = true;

        // ���� ����
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y * rb.gravityScale)));

        // ������ �� ���� ���̿� ������ ������ ���
        while (rb.velocity.y > 0)
        {
            yield return null;
        }

        // ������� ����
        isJumping = false;
        isSlamming = true;
        Debug.Log("Slamming Down!");

        rb.velocity = new Vector2(0, -slamSpeed);  // ������ �������

        // ���鿡 ������ ������ ���
        while (!IsGrounded())
        {
            yield return null;
        }

        // ����� ���� ����
        PerformSlamAttack();
        isSlamming = false;
    }
    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
    }
    private void PerformSlamAttack()
    {
        Debug.Log("Performing Ground Slam Attack!");
        Vector2 attack2Position = transform.position + transform.up * -2f;
        // �ֺ� ���� ����
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attack2Position, attack2BoxSize, 0);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Player"))
            {
                Debug.Log("Hit Player with Slam Attack!");
                enemy.GetComponent<LivingEntity>().OnDamage(10);
            }
        }
    }

    private BTNodeState Attack3()
    {
        LookAtPlayer();
        Debug.Log("Performing Attack 3");
        SetNextAttackTime();
        return BTNodeState.Success;
    }

    private BTNodeState TeleportAbovePlayer()
    {
        if (Time.time < nextTeleportTime)
        {
            return BTNodeState.Failure;  // ��ٿ��� ������ ������ �������� ����
        }
        LookAtPlayer();
        Debug.Log("Teleporting above player!");
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + teleportHeight, transform.position.z);

        nextTeleportTime = Time.time + teleportCooldown; // ���� �ڷ���Ʈ �ð� ����
        return BTNodeState.Success;
    }

    private BTNodeState Chase()
    {
        if (isPlayerInRange)  // �÷��̾ ���� ���� �ȿ� �ִٸ� �߰��� ����
        {
            //Debug.Log("Player is in attack range, stopping chase.");
            return BTNodeState.Failure;
        }

        //Debug.Log("Chasing Player");
        LookAtPlayer();
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        return BTNodeState.Running;
    }

    private BTNodeState Patrol()
    {
        if (isPlayerDetected)
        {
            return BTNodeState.Failure;  // �÷��̾ �����Ǹ� ������ �����.
        }

        rb.velocity = new Vector2(nextMove * moveSpeed, rb.velocity.y);

        if (Time.time >= nextJumpTime)
        {
            Jump();
            nextJumpTime = Time.time + jumpInterval;
        }
        return BTNodeState.Running;
    }

    private void Jump()
    {
        //Debug.Log("Monster is Jumping!");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void Think()
    {
        nextMove = Random.Range(-1, 2);
        transform.localScale = new Vector3(nextMove == -1 ? -1 : 1, 1, 1);
        Invoke("Think", nextMoveTime);
    }
    private BTNodeState Die()
    {
        OnDie();
        return BTNodeState.Success;
    }

    public override void OnDamage(float damage)
    {
        if (base.dead) return;  // ��� �� �ǰ� ��ȿ
        base.OnDamage(damage);
        Debug.Log(gameObject.name + " took damage! Current Health: " + currentHealth);
    }

    public override void OnDie()
    {
        Debug.Log("Monster is Dead!");
        rb.velocity = Vector2.zero;  // ������ ����
        GetComponent<Collider2D>().enabled = false;  // �浹 ����
        Destroy(this.gameObject);
    }
    private void LookAtPlayer()
    {
        if (playerTransform == null) return;
        transform.localScale = new Vector3(playerTransform.position.x < transform.position.x ? -1 : 1, 1, 1);
    }

    public void PlayerDetected(string colliderType, Collider2D player) // �ݶ��̴� ����
    {
        if (colliderType == "attackCollider")
        {
            isPlayerInRange = true;
        }
        else if (colliderType == "detectionCollider")
        {
            isPlayerDetected = true;
        }
    }
    public void PlayerExited(string colliderType, Collider2D player)
    {
        if (colliderType == "attackCollider")
        {
            isPlayerInRange = false;
        }
        else if (colliderType == "detectionCollider")
        {
            isPlayerDetected = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.right * 2f, attack1BoxSize);
        Gizmos.DrawWireCube(transform.position + transform.up * -2f, attack2BoxSize);
    }
}