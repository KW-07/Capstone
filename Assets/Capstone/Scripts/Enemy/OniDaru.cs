using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OniDaru : LivingEntity
{
    public Transform playerTransform;
    public GameObject healthBar; // 몬스터 방향전환시 HP바가 회전하지 않게 하기 위해 받아옴

    [Header("Range")]
    public float attackRange = 1.5f;
    public float detectionRange = 5.0f; // 플레이어를 감지하는 거리

    [Header("Itemdrop")]
    public bool ItemDrop;

    public float moveSpeed = 2.0f;

    [Header("Attack")]
    /*    public Transform attackPoint;
        public Vector2 attackBoxSize;
        public float attackDelay = 1.0f;  // 공격의 선딜? 아래의 쿨다운과는 다른것
        public float attackCooldown = 2.0f;
        private float nextAttackTime = 0f;*/

    public Transform attackPoint;
    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;
    private bool isattack = false;
    public Vector2 attack2BoxSize;    // 공격 범위
    public float jumpHeight = 3.0f;      // 점프 높이
    public float slamSpeed = 10.0f;      // 내려찍기 속도
    public float groundCheckDistance = 0.5f;  // 지면 감지 거리
    private bool isSlamming = false;     // 내려찍기 상태 여부
    private bool isJumping = false;      // 점프 상태 여부

    public int nextThinkTime = 3;
    private int nextMove;

    private Animator animator;
    private Rigidbody2D rb;
    private BTSelector root;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Invoke("Think", nextThinkTime);

        root = new BTSelector();

        BTSequence attackSequence = new BTSequence();
        attackSequence.AddChild(new BTCondition(IsPlayerInRange));
        attackSequence.AddChild(new BTCondition(CanAttack));
        attackSequence.AddChild(new BTAction(Attack));

        BTSequence chaseSequence = new BTSequence();
        chaseSequence.AddChild(new BTCondition(IsPlayerDetected));
        chaseSequence.AddChild(new BTAction(Chase));

        BTSequence patrolSequence = new BTSequence();
        patrolSequence.AddChild(new BTAction(Patrol));

        BTSequence deathSequence = new BTSequence();
        deathSequence.AddChild(new BTCondition(() => dead));
        deathSequence.AddChild(new BTAction(Die));

        root.AddChild(deathSequence);
        root.AddChild(attackSequence);
        root.AddChild(chaseSequence);
        root.AddChild(patrolSequence);
    }

    private void Update()
    {
        root.Evaluate();
        if (healthBar != null)
        {
            healthBar.transform.rotation = Quaternion.identity;

            Transform barVisual = healthBar.transform.Find("BarVisual");
            if (barVisual != null)
            {
                Vector3 scale = barVisual.localScale;
                scale.x = Mathf.Abs(scale.x); // 항상 양수
                barVisual.localScale = scale;
            }
        }
    }
    private void FixedUpdate()
    {
        Vector2 frontVec = new Vector2(rb.position.x + nextMove * 0.5f, rb.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
        if (rayHit.collider == null)
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", nextThinkTime);
        }
    }
    private float Get2DDistance(Vector3 a, Vector3 b)
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

    // private bool IsPlayerInRange() => Vector3.Distance(transform.position, playerTransform.position) <= attackRange;
    // private bool IsPlayerDetected() => Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    private bool CanAttack() => Time.time >= nextAttackTime;
    private void SetNextAttackTime() => nextAttackTime = Time.time + attackCooldown;

    #region attack
    private BTNodeState Attack()
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
    /*    private BTNodeState Attack()
        {
            LookAtPlayer();
            Debug.Log("Preparing Attack...");
            StartCoroutine(DelayedAttack());
            SetNextAttackTime();
            return BTNodeState.Success;
        }
        private IEnumerator DelayedAttack()
        {
            yield return new WaitForSeconds(attackDelay);

            // 실제 공격 실행 (전방 공격)
            Debug.Log("Performing Attack after delay!");
            PerformForwardAttack();
        }
        private void PerformForwardAttack()
        {
            // 전방 공격 로직 (예: 히트박스 검사)
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackBoxSize, 0);
            foreach (var enemy in hitEnemies)
            {
                if (enemy.CompareTag("Player"))
                {
                    Debug.Log("Hit Player!");
                    enemy.GetComponent<LivingEntity>().OnDamage(10);
                }
            }
        }*/
    #endregion

    private BTNodeState Chase()
    {
        if (IsPlayerInRange())  // 플레이어가 공격 범위 안에 있다면 추격을 멈춤
        {
            //Debug.Log("Player is in attack range, stopping chase.");
            return BTNodeState.Failure;
        }
        LookAtPlayer();
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        return BTNodeState.Running;
    }
    private BTNodeState Patrol()
    {
        if (IsPlayerDetected())
        {
            return BTNodeState.Failure;  // 플레이어가 감지되면 순찰을 멈춘다.
        }
        rb.velocity = new Vector2(nextMove * moveSpeed, rb.velocity.y);
        return BTNodeState.Running;
    }
    private void Think()
    {
        nextMove = Random.Range(-1, 2);
        float yRotation = nextMove == -1 ? 180f : 0f;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        Invoke("Think", nextThinkTime);
    }
    private BTNodeState Die()
    {
        OnDie();
        return BTNodeState.Success;
    }
    public override void OnDamage(float damage)
    {
        //Debug.Log("Monster took damage! Current Health: " + currentHealth);
        base.OnDamage(damage);
    }
    public override void OnDie()
    {
        base.OnDie();
        rb.velocity = Vector2.zero;  // 움직임 정지
        GetComponent<Collider2D>().enabled = false;  // 충돌 제거
        Destroy(this.gameObject);
    }
    private void LookAtPlayer()
    {
        if (playerTransform == null) return;

        bool lookLeft = playerTransform.position.x < transform.position.x;
        transform.rotation = Quaternion.Euler(0, lookLeft ? 180f : 0f, 0);
    }
    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPoint.position, attack2BoxSize);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
