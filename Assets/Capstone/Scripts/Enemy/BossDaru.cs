using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossDaru : MonoBehaviour
{
    public Transform playerTransform;

    public float moveSpeed = 2.0f;
    public float jumpForce = 5.0f;
    public float jumpInterval = 2.0f;
    public float teleportHeight = 2.5f;

    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;
    public float attackDelay = 1.0f;  // ���� 1�� ��� �ð�

    public Vector2 attack1BoxSize;

    public float jumpHeight = 3.0f;      // ���� ����
    public float slamSpeed = 10.0f;      // ������� �ӵ�
    public Vector2 attack2BoxSize;    // ���� ����
    public float groundCheckDistance = 0.5f;  // ���� ���� �Ÿ�

    private bool isSlamming = false;     // ������� ���� ����
    private bool isJumping = false;      // ���� ���� ����

    public float teleportCooldown = 5.0f; // �ڷ���Ʈ ���� ���ð�
    private float nextTeleportTime = 0f;

    public float maxHealth = 100f;
    private float currentHealth;

    public bool itemDrop;

    public int nextMoveTime = 3;
    private int nextMove;

    private Rigidbody2D rb;
    private float nextJumpTime = 0f;

    private BTSelector root;
    private bool isDead = false;

    private bool isPlayerDetected = false;  // �÷��̾� ���� ����
    private bool isPlayerInRange = false;   // ���� ���� �ȿ� �ִ��� ����

    private void Start()
    {
        currentHealth = maxHealth;
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
        BTCondition isdeadCondition = new BTCondition(() => isDead);
        BTCondition canAttack = new BTCondition(CanAttack);
        BTCondition canTeleport = new BTCondition(CanTeleport);

        normalAttackSelector.AddChild(attack1);
        normalAttackSelector.AddChild(attack2);

        attackSequence.AddChild(playerInRange);
        attackSequence.AddChild(canAttack);
        attackSequence.AddChild(normalAttackSelector);

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

        chaseSequence.AddChild(playerDetected);
        chaseSequence.AddChild(chaseAction);

        patrolSequence.AddChild(patrolAction);

        deathSequence.AddChild(isdeadCondition);
        deathSequence.AddChild(dieAction);

        root.AddChild(deathSequence);
        root.AddChild(specialPatternLoop);
        root.AddChild(attackSequence);
        root.AddChild(chaseSequence);
        root.AddChild(patrolSequence);

        root.Evaluate();
    }

    private void Update()
    {
        root.Evaluate();
    }

    private bool IsLowHealth()
    {
        return currentHealth <= maxHealth * 0.4f;
    }
    private bool CanAttack()
    {
        return Time.time >= nextAttackTime;
    }
    private void SetNextAttackTime()
    {
        nextAttackTime = Time.time + attackCooldown;
    }
    private bool CanTeleport()
    {
        return Time.time >= nextTeleportTime; // ��ٿ��� �����ٸ� true ��ȯ
    }
    #region Attack1code
    private BTNodeState Attack1()
    {
        Debug.Log("Preparing Attack 1...");
        Invoke(nameof(PerformForwardAttack), attackDelay);
        SetNextAttackTime();
        return BTNodeState.Success;
    }
    private void PerformForwardAttack()
    {
        // ���� ���� ���� (��: ��Ʈ�ڽ� �˻�)
        Vector2 attackPosition = transform.position + transform.right * 1.5f; // ĳ���� ����

        Debug.Log("Performing Attack 1 after delay!");
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attack1BoxSize, 0);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Player"))
            {
                Debug.Log("Hit Player!");
                //enemy.GetComponent<Player>().TakeDamage(10);
                // ���⿡ �÷��̾�� ���ظ� �ִ� �ڵ� �߰�
            }
        }
    }
    #endregion

    private BTNodeState Attack2()
    {
        //if (!isJumping && !isSlamming)
        Debug.Log("Jumping for Attack 2...");
        StartCoroutine(JumpAndSlam());
        SetNextAttackTime();
        return BTNodeState.Success;
    }
    private IEnumerator JumpAndSlam()
    {
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
        Vector2 attackPosition = transform.position + transform.up * -1;
        // �ֺ� ���� ����
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attack2BoxSize, 0);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Player"))
            {
                Debug.Log("Hit Player with Slam Attack!");
                //enemy.GetComponent<Player>().TakeDamage(10);
                // �÷��̾�� ���ظ� �ִ� �ڵ� �߰�
            }
        }
    }

    private BTNodeState Attack3()
    {
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
        Debug.Log("Monster is Jumping!");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void Think()
    {
        nextMove = Random.Range(-1, 2);

        Invoke("Think", nextMoveTime);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;  // ��� �� �ǰ� ��ȿ
        currentHealth -= damage;
        Debug.Log("Monster took damage! Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("Monster has died!");
        }
    }

    private BTNodeState Die()
    {
        Debug.Log("Monster is Dead!");
        rb.velocity = Vector2.zero;  // ������ ����
        GetComponent<Collider2D>().enabled = false;  // �浹 ����
        Destroy(this.gameObject);
        return BTNodeState.Success;
    }

    // --------------- �ݶ��̴� ���� �ý��� ---------------

    public void PlayerDetected(string colliderType, Collider2D player)
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
        Gizmos.DrawWireCube(transform.position + transform.right * 1.5f, attack1BoxSize);
        Gizmos.DrawWireCube(transform.position + transform.up * -1, attack2BoxSize);
    }
}