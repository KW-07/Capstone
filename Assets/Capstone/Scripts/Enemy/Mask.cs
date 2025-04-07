using System.Collections;
using UnityEngine;

public class Mask: LivingEntity
{
    public Transform playerTransform;

    [Header("Ranges")]
    public float detectRange = 6f;
    public float dashRange = 2f;

    [Header("Movement Speeds")]
    public float patrolSpeed = 1.5f;
    public float approachSpeed = 2.5f;
    public float dashSpeed = 12f;

    [SerializeField]
    [Header("Patrol")]
    private float patrolTime;
    private float desiredAltitude = 4.0f;       // 지면과 유지하고 싶은 평균 거리
    private float maxAltitude = 10.0f;           // 이보다 높으면 하강 시작
    private float groundCheckDistance = 8.0f;   // 지면 찾기 최대 거리


    private Rigidbody2D rb;
    private Vector2 dashTarget;
    private bool isDashing = false;
    private bool isAttached = false;
    private Vector2 patrolDirection;


    private BTSelector root;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        patrolDirection = Random.insideUnitCircle.normalized;

        root = new BTSelector();

        // 부착 상태이면 아무것도 안함
        BTSequence attachSeq = new BTSequence();
        attachSeq.AddChild(new BTCondition(() => isAttached));
        attachSeq.AddChild(new BTAction(Idle));

        // 공격 범위에 들어오면 대쉬
        BTSequence dashSeq = new BTSequence();
        dashSeq.AddChild(new BTCondition(IsInDashRange));
        dashSeq.AddChild(new BTAction(DashToPlayer));

        // 감지 범위에 있으면 곡선 무빙으로 접근
        BTSequence approachSeq = new BTSequence();
        approachSeq.AddChild(new BTCondition(IsInDetectRange));
        approachSeq.AddChild(new BTAction(ApproachPlayer));

        // 그 외에는 자유 비행 패트롤
        root.AddChild(attachSeq);
        root.AddChild(dashSeq);
        root.AddChild(approachSeq);
        root.AddChild(new BTAction(Patrol));
    }

    private void Update()
    {
        root.Evaluate();
    }
    private void FixedUpdate()
    {
        if (isAttached)
        {
            transform.position = playerTransform.position;
        }
    }

    private bool IsInDetectRange()
    {
        return Vector2.Distance(transform.position, playerTransform.position) <= detectRange;
    }

    private bool IsInDashRange()
    {
        return Vector2.Distance(transform.position, playerTransform.position) <= dashRange;
    }

    private BTNodeState Idle()
    {
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;  // 충돌 제거
        return BTNodeState.Running;
    }

    private BTNodeState Patrol()
    {
        if (isDashing || isAttached) return BTNodeState.Failure;

        patrolTime += Time.deltaTime;

        Vector2 forward = patrolDirection.normalized;
        Vector2 oscillation = new Vector2(-forward.y, forward.x) * Mathf.Sin(patrolTime * 3f) * 0.5f;
        Vector2 moveDir = (forward + oscillation).normalized;

        // 지면 체크
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            float altitude = hit.distance;

            // 너무 낮으면 상승
            if (altitude < desiredAltitude)
            {
                moveDir += Vector2.up * (desiredAltitude - altitude);
            }
            // 너무 높으면 하강
            else if (altitude > maxAltitude)
            {
                moveDir += Vector2.down * (altitude - maxAltitude);
            }
        }

        rb.velocity = moveDir.normalized * patrolSpeed;

        // 방향 갱신
        if (patrolTime > 3f)
        {
            patrolTime = 0f;
            patrolDirection = Random.insideUnitCircle.normalized;
        }

        return BTNodeState.Running;
    }

    /*    private BTNodeState Patrol()
        {
            if (isDashing || isAttached) return BTNodeState.Failure;

            rb.velocity = patrolDirection * patrolSpeed;

            // 일정 시간마다 방향 바꿈
            if (Random.value < 0.01f)
            {
                patrolDirection = Random.insideUnitCircle.normalized;
            }

            return BTNodeState.Running;
        }*/

    private BTNodeState ApproachPlayer()
    {
        if (isDashing || isAttached) return BTNodeState.Failure;

        Vector2 toPlayer = (playerTransform.position - transform.position).normalized;
        Vector2 randomOffset = new Vector2(Mathf.Sin(Time.time * 3f), Mathf.Cos(Time.time * 2f)) * 0.5f;
        Vector2 moveDir = (toPlayer + randomOffset).normalized;

        rb.velocity = moveDir * approachSpeed;

        LookAtPlayer();

        return BTNodeState.Running;
    }

    private BTNodeState DashToPlayer()
    {
        if (isDashing || isAttached) return BTNodeState.Failure;

        isDashing = true;
        dashTarget = playerTransform.position;
        StartCoroutine(DashRoutine());

        return BTNodeState.Success;
    }

    private IEnumerator DashRoutine()
    {
        Vector2 direction = (dashTarget - (Vector2)transform.position).normalized;

        while (!isAttached)
        {
            rb.velocity = direction * dashSpeed;

            if (!IsInDashRange())
            {
                break; // 공격 범위에서 벗어나면 멈춤
            }

            yield return null;
        }

        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && collision.collider.CompareTag("Player"))
        {
            AttachToPlayer(collision.collider.transform);
        }
    }

    private void AttachToPlayer(Transform player)
    {
        isAttached = true;
        isDashing = false;
        rb.velocity = Vector2.zero;

        transform.SetParent(player);
        transform.localPosition = new Vector3(0, 0, 0); // 얼굴 위치쯤

        Debug.Log("MaskMonster 부착됨 - 플레이어에게 디버프 및 데미지!");
    }

    private void LookAtPlayer()
    {
        if (playerTransform == null) return;
        transform.localScale = new Vector3(
            playerTransform.position.x < transform.position.x ? -1 : 1,
            1, 1
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dashRange);
    }

    public void Detach()
    {
        isAttached = false;
        transform.SetParent(null);
    }
}
