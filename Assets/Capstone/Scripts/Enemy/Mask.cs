using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Mask : MonoBehaviour, LivingEntity
{
    public Transform playerTransform;
    public GameObject healthBar; // ���� ������ȯ�� HP�ٰ� ȸ������ �ʰ� �ϱ� ���� �޾ƿ�

    [Header("HP")]
    public Image currentHealthBar;
    public float maxHealth = 100f; //���� ü��
    private float currentHealth;//���� ü��
    private bool isdead; //��� ����

    [Header("Ranges")]
    public float detectRange = 6f;
    public float dashRange = 2f;

    [Header("Movement Speeds")]
    public float patrolSpeed = 1.5f;
    public float approachSpeed = 2.5f;
    public float dashSpeed = 12f;

    [Header("Reattach Time")]
    public float reattachCooldown = 2f;      // �ٽ� �ٱ���� �ɸ��� �ð�
    private float lastDetachTime = -999f;     // ���������� ������ �ð� ���

    private int directionSwitchCount = 0;
    private float directionCheckTime = 2.0f; // 2�� �ȿ� �Է��ؾ� ��
    private float directionTimer = 0f;
    private int lastDirection = 0;

    private PlayerInput playerInput; // Input System ����

    private Rigidbody2D rb;
    private Vector2 dashTarget;
    private bool isDashing = false;
    private bool isAttached = false;
    private Vector2 patrolDirection;


    private BTSelector root;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = playerTransform.GetComponent<PlayerInput>();
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
        if (healthBar != null)
        {
            healthBar.transform.rotation = Quaternion.identity;

            Transform barVisual = healthBar.transform.Find("BarVisual");
            if (barVisual != null)
            {
                Vector3 scale = barVisual.localScale;
                scale.x = Mathf.Abs(scale.x); // �׻� ���
                barVisual.localScale = scale;
            }
        }
        if (isAttached)
        {
            HandleDetachInput();
        }
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
    public void InitialSet()
    {
        isdead = false;
        currentHealth = maxHealth;
    }

    public void CheckHp()
    {
        // ������ ���� ��¼��... �� ������ ��¼��...
        if (currentHealthBar != null)
            currentHealthBar.fillAmount = currentHealth / maxHealth;

        Debug.Log($"ü�¹� ���� fillAmount : {currentHealthBar.fillAmount}");
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

        Vector2 adjustedDir = AvoidObstacles(patrolDirection);

        Vector2 flutter = new Vector2(
            Mathf.PerlinNoise(Time.time * 1.5f, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * 1.5f) - 0.5f
        ) * 0.3f;

        Vector2 movedir = (adjustedDir + flutter).normalized;

        rb.velocity = movedir * patrolSpeed;

        // ���� �ð����� ���� �ٲ�
        if (Random.value < 0.01f)
        {
            patrolDirection = Random.insideUnitCircle.normalized;
        }

        return BTNodeState.Running;
    }
    private Vector2 AvoidObstacles(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f, dir, 1.5f, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            // ��ֹ��� ������ ������ �����ϰ� ƨ��
            dir = Vector2.Reflect(dir, hit.normal);
        }

        return dir.normalized;
    }

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
    //private void OnColliderEnter2D(Collision2D collision)
    //{
    //    Debug.Log(collision.gameObject.name + "On OnTriggerEnter");
    //    if (isDashing && collision.collider.CompareTag("Player"))
    //    {
    //        AttachToPlayer(collision.transform);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + "On OnTriggerEnter");
        if (isDashing && collision.CompareTag("Player"))
        {
            AttachToPlayer(collision.transform);
        }
    }
    private void AttachToPlayer(Transform player)
    {
        if (Time.time < lastDetachTime + reattachCooldown)
        {
            Debug.Log("���� ���� ��Ÿ�� ���Դϴ�.");
            return;
        }

        isAttached = true;
        isDashing = false;
        rb.velocity = Vector2.zero;

        // ���� ��ġ �޾ƿ���
        Transform attachPoint = player.GetComponent<Player>()?.GetMaskAttachPoint();

        if (attachPoint != null)
        {
            transform.SetParent(attachPoint);
            transform.localPosition = Vector3.zero; // ���� ������ ��Ȯ�� ����
        }
        else
        {
            Debug.LogWarning("���� ��ġ�� ���ǵǾ� ���� �ʽ��ϴ�! �⺻ ��ġ�� �����˴ϴ�.");
            transform.SetParent(player);
            transform.localPosition = new Vector3(0, 0, 0);
        }

        Debug.Log("MaskMonster ������ - �÷��̾�� ����� �� ������!");
    }
    private void HandleDetachInput()
    {
        float moveInput = playerInput.actions["Move"].ReadValue<float>();
        int currentDir = Mathf.RoundToInt(moveInput);  // -1, 0, 1 �� �ϳ�

        if (currentDir != 0 && currentDir != lastDirection)
        {
            lastDirection = currentDir;
            directionSwitchCount++;
            directionTimer = 0f;
        }

        directionTimer += Time.deltaTime;

        if (directionSwitchCount >= 5)
        {
            Detach();
            ResetDetachInput();
        }

        if (directionTimer > directionCheckTime)
        {
            ResetDetachInput();
        }
    }

    private void ResetDetachInput()
    {
        directionSwitchCount = 0;
        directionTimer = 0f;
        lastDirection = 0;
    }

    public void OnDamage(float damage)
    {
        currentHealth -= damage;
        CheckHp();
        Debug.Log(gameObject.name + " took damage! Current Health: " + currentHealth);

        if (currentHealth <= 0 && isdead)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Monster is Dead!");
        rb.velocity = Vector2.zero;  // ������ ����
        GetComponent<Collider2D>().enabled = false;  // �浹 ����
        Destroy(this.gameObject);
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
        GetComponent<Collider2D>().enabled = true;

        lastDetachTime = Time.time; // ������ �ð� ����

        // ������ �� �ణ ƨ��� ȿ��
        rb.velocity = new Vector2(Random.Range(-1f, 1f), 1f) * 3f;
    }
}