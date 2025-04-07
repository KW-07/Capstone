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
    private float desiredAltitude = 4.0f;       // ����� �����ϰ� ���� ��� �Ÿ�
    private float maxAltitude = 10.0f;           // �̺��� ������ �ϰ� ����
    private float groundCheckDistance = 8.0f;   // ���� ã�� �ִ� �Ÿ�


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

        // ���� �����̸� �ƹ��͵� ����
        BTSequence attachSeq = new BTSequence();
        attachSeq.AddChild(new BTCondition(() => isAttached));
        attachSeq.AddChild(new BTAction(Idle));

        // ���� ������ ������ �뽬
        BTSequence dashSeq = new BTSequence();
        dashSeq.AddChild(new BTCondition(IsInDashRange));
        dashSeq.AddChild(new BTAction(DashToPlayer));

        // ���� ������ ������ � �������� ����
        BTSequence approachSeq = new BTSequence();
        approachSeq.AddChild(new BTCondition(IsInDetectRange));
        approachSeq.AddChild(new BTAction(ApproachPlayer));

        // �� �ܿ��� ���� ���� ��Ʈ��
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
        GetComponent<Collider2D>().enabled = false;  // �浹 ����
        return BTNodeState.Running;
    }

    private BTNodeState Patrol()
    {
        if (isDashing || isAttached) return BTNodeState.Failure;

        patrolTime += Time.deltaTime;

        Vector2 forward = patrolDirection.normalized;
        Vector2 oscillation = new Vector2(-forward.y, forward.x) * Mathf.Sin(patrolTime * 3f) * 0.5f;
        Vector2 moveDir = (forward + oscillation).normalized;

        // ���� üũ
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            float altitude = hit.distance;

            // �ʹ� ������ ���
            if (altitude < desiredAltitude)
            {
                moveDir += Vector2.up * (desiredAltitude - altitude);
            }
            // �ʹ� ������ �ϰ�
            else if (altitude > maxAltitude)
            {
                moveDir += Vector2.down * (altitude - maxAltitude);
            }
        }

        rb.velocity = moveDir.normalized * patrolSpeed;

        // ���� ����
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

            // ���� �ð����� ���� �ٲ�
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
                break; // ���� �������� ����� ����
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
        transform.localPosition = new Vector3(0, 0, 0); // �� ��ġ��

        Debug.Log("MaskMonster ������ - �÷��̾�� ����� �� ������!");
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
