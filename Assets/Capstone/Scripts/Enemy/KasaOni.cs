using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasaOni : LivingEntity
{
    public Transform playerTransform;
    public GameObject healthBar;

    [Header("Range")]
    public float attackRange = 1.5f;
    public float detectionRange = 5.0f;
    public float retreatDistance = 1.0f;  // 플레이어 근접 시 회피 거리

    [Header("Itemdrop")]
    public bool ItemDrop;

    public float moveSpeed = 2.0f;

    [Header("Attack")]
    public Transform attackPoint;
    public Vector2 attackBoxSize;
    public float attackDelay = 1.0f;
    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;

    [Header("Retreat Jump")]
    public float jumpForce = 5f;
    public float retreatCooldown = 3.0f;
    private float nextRetreatTime = 0f;

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

        BTSequence deathSequence = new BTSequence();
        deathSequence.AddChild(new BTCondition(() => dead));
        deathSequence.AddChild(new BTAction(Die));

        BTSequence retreatSequence = new BTSequence();
        retreatSequence.AddChild(new BTCondition(IsPlayerTooClose));
        retreatSequence.AddChild(new BTCondition(CanRetreat));
        retreatSequence.AddChild(new BTAction(RetreatJump));

        BTSequence attackSequence = new BTSequence();
        attackSequence.AddChild(new BTCondition(IsPlayerInRange));
        attackSequence.AddChild(new BTCondition(CanAttack));
        attackSequence.AddChild(new BTAction(Attack));

        BTSequence chaseSequence = new BTSequence();
        chaseSequence.AddChild(new BTCondition(IsPlayerDetected));
        chaseSequence.AddChild(new BTAction(Chase));

        BTSequence patrolSequence = new BTSequence();
        patrolSequence.AddChild(new BTAction(Patrol));

        root.AddChild(deathSequence);
        root.AddChild(retreatSequence); // 먼저 회피 행동 시도
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

    private bool IsPlayerTooClose()
    {
        float dist = Get2DDistance(transform.position, playerTransform.position);
        //Debug.Log($"IsPlayerTooClose? Distance: {dist} / RetreatDistance: {retreatDistance} / Result: {dist <= retreatDistance}");
        return dist <= retreatDistance;
    }
    //private bool IsPlayerInRange() => Vector3.Distance(transform.position, playerTransform.position) <= attackRange;
    //private bool IsPlayerDetected() => Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    //private bool IsPlayerTooClose() => Vector3.Distance(transform.position, playerTransform.position) <= retreatDistance;
    private bool CanAttack() => Time.time >= nextAttackTime;
    private bool CanRetreat() => Time.time >= nextRetreatTime;

    private void SetNextAttackTime() => nextAttackTime = Time.time + attackCooldown;

    private BTNodeState Attack()
    {
        LookAtPlayer();
        Debug.Log("Preparing Tongue Smash Attack (Test Mode)...");
        StartCoroutine(DelayedTongueSmashTest());
        SetNextAttackTime();
        return BTNodeState.Success;
    }

    private IEnumerator DelayedTongueSmashTest()
    {
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("Executing PerformTongueSmash()");
        PerformTongueSmash();
    }

    public void PerformTongueSmash()
    {
        // 바라보는 방향(왼쪽이면 -1, 오른쪽이면 1) 
        float direction = Mathf.Sign(transform.localScale.x);

        // 방향에 따라 smashPoint 계산
        Vector2 smashPoint = (Vector2)transform.position + new Vector2(direction * 2.5f, 0f);
        Vector2 smashBoxSize = new Vector2(3f, 1.5f);

       // Debug.Log($"[TongueSmash] Attack area at {smashPoint}, direction: {direction}");

        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(smashPoint, smashBoxSize, 0f);
        //Debug.Log($"[TongueSmash] Colliders found: {hitTargets.Length}");

        foreach (var target in hitTargets)
        {
            //Debug.Log($"[TongueSmash] Hit check on: {target.name}");
            if (target.CompareTag("Player"))
            {
                //Debug.Log("[TongueSmash] Player hit!");
                target.GetComponent<LivingEntity>().OnDamage(25);

                Rigidbody2D playerRb = target.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 knockbackDir = (target.transform.position - transform.position).normalized;
                    playerRb.AddForce(knockbackDir * 400f, ForceMode2D.Impulse);
                }
            }
        }
    }

    private BTNodeState RetreatJump()
    {
        float direction = transform.localScale.x > 0 ? -1 : 1; // 현재 바라보는 반대 방향으로 점프
        rb.velocity = new Vector2(direction * moveSpeed * 1.5f, jumpForce);
        nextRetreatTime = Time.time + retreatCooldown;
        return BTNodeState.Success;
    }

    private BTNodeState Chase()
    {
        if (IsPlayerInRange()) return BTNodeState.Failure;
        LookAtPlayer();
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        return BTNodeState.Running;
    }

    private BTNodeState Patrol()
    {
        if (IsPlayerDetected()) return BTNodeState.Failure;
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

    public override void OnDie()
    {
        base.OnDie();
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
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
            Gizmos.DrawWireCube(attackPoint.position, attackBoxSize);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, retreatDistance);
    }
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;

        float direction = Mathf.Sign(transform.localScale.x);
        Vector2 smashPoint = (Vector2)transform.position + new Vector2(direction * 2.5f, 0f);
        Vector2 smashBoxSize = new Vector2(3f, 1.5f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(smashPoint, smashBoxSize);
    }
}