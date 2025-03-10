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

    public float teleportCooldown = 5.0f; // 텔레포트 재사용 대기시간
    private float nextTeleportTime = 0f;

    public float maxHealth = 100f;
    private float currentHealth;

    public bool itemDrop;

    private Rigidbody2D rb;
    private float nextJumpTime = 0f;

    private BTSelector root;
    private bool isLowHealthMode = false;
    private bool isDead = false;

    private bool isPlayerDetected = false;  // 플레이어 감지 여부
    private bool isPlayerInRange = false;   // 공격 범위 안에 있는지 여부

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
        specialPatternLoop.AddChild(canTeleport);  // 텔레포트 가능할 때만 실행
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
        return Time.time >= nextTeleportTime; // 쿨다운이 끝났다면 true 반환
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
            return BTNodeState.Failure;  // 쿨다운이 끝나지 않으면 실행하지 않음
        }

        Debug.Log("Teleporting above player!");
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + teleportHeight, transform.position.z);

        nextTeleportTime = Time.time + teleportCooldown; // 다음 텔레포트 시간 설정
        return BTNodeState.Success;
    }

    private BTNodeState Chase()
    {
        if (isPlayerInRange)  // 플레이어가 공격 범위 안에 있다면 추격을 멈춤
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
            return BTNodeState.Failure;  // 플레이어가 감지되면 순찰을 멈춘다.
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
        rb.velocity = Vector2.zero;  // 움직임 정지
        GetComponent<Collider2D>().enabled = false;  // 충돌 제거
        Destroy(gameObject);
        return BTNodeState.Success;
    }

    // --------------- 콜라이더 감지 시스템 ---------------

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