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

    public float teleportCooldown = 5.0f; // �ڷ���Ʈ ���� ���ð�
    private float nextTeleportTime = 0f;

    public float maxHealth = 100f;
    private float currentHealth;

    public bool itemDrop;

    private Rigidbody2D rb;
    private float nextJumpTime = 0f;

    private BTSelector root;
    private bool isLowHealthMode = false;
    private bool isDead = false;

    private bool isPlayerDetected = false;  // �÷��̾� ���� ����
    private bool isPlayerInRange = false;   // ���� ���� �ȿ� �ִ��� ����

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

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

        root.AddChild(specialPatternLoop);
        root.AddChild(attackSequence);
        root.AddChild(chaseSequence);
        root.AddChild(patrolSequence);

        root.Evaluate();
    }

    private void Update()
    {
        if (!isDead)
        {
            root.Evaluate();
        }
    }

    private bool IsLowHealth()
    {
        if (currentHealth <= maxHealth * 0.4f)
        {
            isLowHealthMode = true;
        }
        return isLowHealthMode;
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

    private BTNodeState Attack1()
    {
        Debug.Log("Performing Attack 1");
        SetNextAttackTime();
        return BTNodeState.Success;
    }

    private BTNodeState Attack2()
    {
        Debug.Log("Performing Attack 2");
        SetNextAttackTime();
        return BTNodeState.Success;
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
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        return BTNodeState.Running;
    }

    private BTNodeState Patrol()
    {
        if (isPlayerDetected)
        {
            return BTNodeState.Failure;  // �÷��̾ �����Ǹ� ������ �����.
        }

        transform.position += new Vector3(Mathf.Sin(Time.time), 0, Mathf.Cos(Time.time)) * moveSpeed * Time.deltaTime;
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

    public void TakeDamage(float damage)
    {
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
        Destroy(gameObject);
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
}