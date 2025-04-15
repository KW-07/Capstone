using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossDaru : LivingEntity
{
    public Transform playerTransform;

    [Header("Range")]
    public float attackRange = 1.5f;
    public float detectionRange = 5.0f; // 플레이어를 감지하는 거리

    [Header("Move")]
    public float moveSpeed = 2.0f;

    [Header("PatrolJump")]
    public float jumpForce = 5.0f;
    public float jumpInterval = 2.0f;
    private float nextJumpTime = 0f;

    [Header("AttackBase")]
    public Transform attackPoint;
    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;
    private bool isattack = false;

    [Header("Attack1")]
    public Vector2 attack1BoxSize;
    public float attackDelay = 1.0f;  // 공격 1의 대기 시간

    [Header("Attack2")]
    public Vector2 attack2BoxSize;    // 공격 범위
    public float jumpHeight = 3.0f;      // 점프 높이
    public float slamSpeed = 10.0f;      // 내려찍기 속도
    public float groundCheckDistance = 0.5f;  // 지면 감지 거리
    private bool isSlamming = false;     // 내려찍기 상태 여부
    private bool isJumping = false;      // 점프 상태 여부

    [Header("Attack3")]
    public Transform leftHorn;
    public Transform rightHorn;
    public GameObject fireballPrefab;
    public float fireballSpeed = 5.0f;
    public float fireballAngle = 45.0f;
    public int fireballCount = 3;           // 발사할 불꽃 개수
    public float fireballInterval = 0.2f;

    [Header("SpecialPattern")]
    public float teleportHeight = 2.5f;   // 플레이어 머리 위로 어느정도 뜰 것인지
    public float teleportCooldown = 5.0f; // 텔레포트 재사용 대기시간
    private float nextTeleportTime = 0f;

    [Header("Item")]
    public bool itemDrop;

    public int nextMoveTime = 3;
    private int nextMove;

    private Rigidbody2D rb;
    private BTSelector root;

    private bool isDead = false;

    private void Start()                                            // tlqkf
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Invoke("Think", nextMoveTime);

        root = new BTSelector();

        BTSequence attackSequence = new BTSequence();
        BTSequence lowHealthattackSequence = new BTSequence();
        BTSequence specialPatternSequence = new BTSequence();
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

        BTCondition playerInRange = new BTCondition(IsPlayerInRange);
        BTCondition playerDetected = new BTCondition(IsPlayerDetected);

        BTCondition lowHealth = new BTCondition(IsLowHealth);
        BTCondition notLowHealth = new BTCondition(NotLowHealth);

        BTCondition isdeadCondition = new BTCondition(() => base.dead);

        BTCondition canAttack = new BTCondition(CanAttack);
        BTCondition canTeleport = new BTCondition(CanTeleport);

        normalAttackSelector.AddChild(attack1);
        normalAttackSelector.AddChild(attack2);

        lowHealthAttackSelector.AddChild(attack1);
        lowHealthAttackSelector.AddChild(attack2);
        lowHealthAttackSelector.AddChild(attack3);

        attackSequence.AddChild(notLowHealth);
        attackSequence.AddChild(playerInRange);
        attackSequence.AddChild(canAttack);
        attackSequence.AddChild(normalAttackSelector);

        specialPatternSequence.AddChild(lowHealth);
        specialPatternSequence.AddChild(playerDetected);
        specialPatternSequence.AddChild(canTeleport);  // 텔레포트 가능할 때만 실행
        specialPatternSequence.AddChild(teleportAbovePlayer);

        lowHealthattackSequence.AddChild(lowHealth);
        lowHealthattackSequence.AddChild(playerInRange);
        lowHealthattackSequence.AddChild(canAttack);
        lowHealthattackSequence.AddChild(lowHealthAttackSelector);

        chaseSequence.AddChild(playerDetected);
        chaseSequence.AddChild(chaseAction);

        patrolSequence.AddChild(patrolAction);

        deathSequence.AddChild(isdeadCondition);
        deathSequence.AddChild(dieAction);

        root.AddChild(deathSequence);
        root.AddChild(attackSequence);
        root.AddChild(specialPatternSequence);
        root.AddChild(lowHealthattackSequence);
        root.AddChild(chaseSequence);
        root.AddChild(patrolSequence);
    }

    private void Update()
    {
        root.Evaluate();
    }
    private float Get2DDistance(Vector3 a, Vector3 b) // 아오 z값
    {
        a.z = 0;
        b.z = 0;
        return Vector3.Distance(a, b);
    }
    private bool IsPlayerDetected()
    {
        float dist = Get2DDistance(transform.position, playerTransform.position);
        //Debug.Log($"IsPlayerDetected? Distance: {dist} / DetectionRange: {detectionRange} / Result: {dist <= detectionRange}");
        return dist <= detectionRange;
    }

    private bool IsPlayerInRange()
    {
        float dist = Get2DDistance(transform.position, playerTransform.position);
        //Debug.Log($"IsPlayerInRange? Distance: {dist} / AttackRange: {attackRange} / Result: {dist <= attackRange}");
        return dist <= attackRange;
    }
    private bool IsLowHealth() => currentHealth <= maxHealth * 0.4f; // 람?다식
    private bool NotLowHealth() => currentHealth >= maxHealth * 0.4f;
    private bool CanAttack() => Time.time >= nextAttackTime;
    private void SetNextAttackTime() => nextAttackTime = Time.time + attackCooldown;
    private bool CanTeleport() => Time.time >= nextTeleportTime;

    #region Attack1 Code
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

        Debug.Log("Performing Attack 1 after delay!");
        Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(attackPoint.position, attack1BoxSize, 0);
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
    #region Attack2 Code
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

        // 위로 점프
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y * rb.gravityScale)));

        // 점프한 후 일정 높이에 도달할 때까지 대기
        while (rb.velocity.y > 0)
        {
            yield return null;
        }

        // 내려찍기 시작
        isJumping = false;
        isSlamming = true;
        Debug.Log("Slamming Down!");

        rb.velocity = new Vector2(0, -slamSpeed);  // 빠르게 내려찍기

        // 지면에 도착할 때까지 대기
        while (!IsGrounded())
        {
            yield return null;
        }

        // 충격파 공격 실행
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
        Vector2 attack2Position = transform.position + transform.up * -1f;
        // 주변 범위 공격
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
    #endregion
    #region Attack3 Code
    private BTNodeState Attack3()
    {
        LookAtPlayer();
        Debug.Log("Performing Attack 3");
        StartCoroutine(FireballBurst());
        SetNextAttackTime();
        return BTNodeState.Success;
    }
    private IEnumerator FireballBurst()
    {
        for (int i = 0; i < fireballCount; i++)
        {
            SpawnFireball(leftHorn);
            SpawnFireball(rightHorn);
            yield return new WaitForSeconds(fireballInterval);
        }
    }

    private void SpawnFireball(Transform spawnPoint)
    {
        GameObject fireball = Instantiate(fireballPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float angleRad = fireballAngle * Mathf.Deg2Rad;
            Vector2 force = new Vector2(Mathf.Cos(angleRad) * fireballSpeed, Mathf.Sin(angleRad) * fireballSpeed);
            rb.velocity = force;
        }
    }
    #endregion

    private BTNodeState TeleportAbovePlayer()
    {
        if (Time.time < nextTeleportTime)
        {
            return BTNodeState.Failure;  // 쿨다운이 끝나지 않으면 실행하지 않음
        }
        LookAtPlayer();
        Debug.Log("Teleporting above player!");
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + teleportHeight, transform.position.z);

        nextTeleportTime = Time.time + teleportCooldown; // 다음 텔레포트 시간 설정
        return BTNodeState.Success;
    }

    private BTNodeState Chase()
    {
        if (IsPlayerInRange())  // 플레이어가 공격 범위 안에 있다면 추격을 멈춤
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
        if (IsPlayerDetected())
        {
            return BTNodeState.Failure;  // 플레이어가 감지되면 순찰을 멈춤
        }

        rb.velocity = new Vector2(nextMove * moveSpeed, rb.velocity.y);

        if (Time.time >= nextJumpTime)
        {
            Jump();
            nextJumpTime = Time.time + jumpInterval;
        }
        return BTNodeState.Running;
    }

    private void Jump() // 패트롤 시 점프하는 함수
    {
        //Debug.Log("Monster is Jumping!");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void Think() // 패트롤 시 방향전환 관련 함수
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
        if (base.dead) return;  // 사망 시 피격 무효
        base.OnDamage(damage);
        Debug.Log(gameObject.name + " took damage! Current Health: " + currentHealth);
    }

    public override void OnDie()
    {
        Debug.Log("Monster is Dead!");
        rb.velocity = Vector2.zero;  // 움직임 정지
        GetComponent<Collider2D>().enabled = false;  // 충돌 제거
        Destroy(this.gameObject);
    }
    private void LookAtPlayer()
    {
        if (playerTransform == null) return;
        transform.localScale = new Vector3(playerTransform.position.x < transform.position.x ? -1 : 1, 1, 1);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPoint.position, attack1BoxSize);
            Gizmos.DrawWireCube(transform.position + transform.up * -1f, attack2BoxSize);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}