using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Kasa : MonoBehaviour, LivingEntity
{
    public Transform playerTransform;
    public GameObject healthBar; // ���� ������ȯ�� HP�ٰ� ȸ������ �ʰ� �ϱ� ���� �޾ƿ�

    [Header("HP")]
    public Image currentHealthBar;
    public float maxHealth = 100f; //���� ü��
    private float currentHealth;//���� ü��
    private bool isdead;

    [Header("Range")]
    public float attackRange = 1.5f;
    public float detectionRange = 5.0f;
    public float retreatDistance = 1.0f;  // �÷��̾� ���� �� ȸ�� �Ÿ�

    [Header("Itemdrop")]
    public bool ItemDrop;

    public float moveSpeed = 2.0f;

    [Header("Attack")]
    public Transform attackPoint;
    public Vector2 attackBoxSize;
    public float damage = 10f;
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

    private void Awake()
    {
        InitialSet();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Invoke("Think", nextThinkTime);

        root = new BTSelector();

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

        root.AddChild(chaseSequence);
        root.AddChild(retreatSequence); // ���� ȸ�� �ൿ �õ�
        root.AddChild(attackSequence);
        root.AddChild(patrolSequence);
    }

    private void Update()
    {
        root.Evaluate();
        if (healthBar != null)
        {
            healthBar.transform.rotation = Quaternion.identity;

            /*            Transform barVisual = healthBar.transform.Find("BarVisual");
                        if (barVisual != null)
                        {
                            Vector3 scale = barVisual.localScale;
                            scale.x = Mathf.Abs(scale.x); // �׻� ���
                            barVisual.localScale = scale;
                        }*/
        }
    }
    private void FixedUpdate()
    {
        Vector2 frontVec = new Vector2(rb.position.x + nextMove * 0.5f, rb.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
        if (rayHit.collider == null)
        {
            nextMove *= -1;
            float yRotation = nextMove == -1 ? 180f : 0f;
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
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
        return dist <= detectionRange;
    }

    private bool IsPlayerInRange()
    {
        float dist = Get2DDistance(transform.position, playerTransform.position);
        return dist <= attackRange;
    }

    private bool IsPlayerTooClose()
    {
        float dist = Get2DDistance(transform.position, playerTransform.position);
        return dist <= retreatDistance;
    }
    //private bool IsPlayerInRange() => Vector3.Distance(transform.position, playerTransform.position) <= attackRange;
    //private bool IsPlayerDetected() => Vector3.Distance(transform.position, playerTransform.position) <= detectionRange;
    //private bool IsPlayerTooClose() => Vector3.Distance(transform.position, playerTransform.position) <= retreatDistance;
    private bool CanAttack() => Time.time >= nextAttackTime;
    private bool CanRetreat() => Time.time >= nextRetreatTime;
    private void SetNextAttackTime() => nextAttackTime = Time.time + attackCooldown;
    public void InitialSet()
    {
        currentHealth = maxHealth;
        isdead = false;
    }
    public void CheckHp()
    {
        if (currentHealthBar != null)
            currentHealthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            isdead = true;
        }
        Debug.Log($"ü�¹� ���� fillAmount : {currentHealthBar.fillAmount}");
    }
    private BTNodeState Attack()
    {
        LookAtPlayer();
        animator.SetTrigger("Attack");
        SetNextAttackTime();
        return BTNodeState.Success;
    }

    private void PerformAttack()
    {
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(attackPoint.position, attackBoxSize, 0f);
        foreach (var target in hitTargets)
        {
            if (target.CompareTag("Player"))
            {
                Debug.Log("Player hit!");
                target.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }
    private BTNodeState RetreatJump()
    {
        // �÷��̾ �� ���ʿ� ������ ������(+1), �����ʿ� ������ ����(-1)
        float direction = (playerTransform.position.x < transform.position.x) ? 1 : -1;

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
        animator.SetInteger("Think", nextMove);
        Debug.Log(nextMove);

        if (nextMove != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = nextMove == -1 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        Invoke("Think", nextThinkTime);
    }

    /*    private void Think()
        {
            nextMove = Random.Range(-1, 2);
            animator.SetInteger("Think", nextMove);
            Debug.Log(nextMove);
            if (nextMove != 0)
            {
                float yRotation = nextMove == -1 ? 180f : 0f;
                transform.rotation = Quaternion.Euler(0, yRotation, 0);
            }
            Invoke("Think", nextThinkTime);
        }*/

    public void OnDamage(float damage)
    {
        currentHealth -= damage;
        CheckHp();
        Debug.Log(gameObject.name + " took damage! Current Health: " + currentHealth);

        if (currentHealth <= 0 && isdead)
        {
            animator.SetTrigger("Die");
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

        bool lookLeft = playerTransform.position.x < transform.position.x;
        Vector3 scale = transform.localScale;
        scale.x = lookLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    /*    private void LookAtPlayer()
        {
            if (playerTransform == null) return;

            bool lookLeft = playerTransform.position.x < transform.position.x;
            transform.rotation = Quaternion.Euler(0, lookLeft ? 180f : 0f, 0);
        }*/

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPoint.position, attackBoxSize);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, retreatDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}