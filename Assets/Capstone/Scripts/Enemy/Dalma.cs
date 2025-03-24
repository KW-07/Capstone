using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Dalma : LivingEntity
{
    public Transform playerTransform;

    [Header("Range")]
    public float attackRange = 1.5f;
    public float detectionRange = 5.0f; // �÷��̾ �����ϴ� �Ÿ�

    [Header("Itemdrop")]
    public bool ItemDrop;

    public float moveSpeed = 2.0f;

    [Header("Attack")]
    public Transform attackPoint;
    public Vector2 attackBoxSize;
    public float attackDelay = 1.0f;  // ������ ����? �Ʒ��� ��ٿ���� �ٸ���
    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;

    public int nextMoveTime = 3;
    private int nextMove;

    private Rigidbody2D rb;

    private BTSelector root;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Invoke("Think", nextMoveTime);

        root = new BTSelector();

        BTSequence attackSequence = new BTSequence();
        BTSequence chaseSequence = new BTSequence();
        BTSequence patrolSequence = new BTSequence();
        BTSequence deathSequence = new BTSequence();

        BTAction attack = new BTAction(Attack);
        BTAction chaseAction = new BTAction(Chase);
        BTAction patrolAction = new BTAction(Patrol);
        BTAction dieAction = new BTAction(Die);

        BTCondition playerInRange = new BTCondition(IsPlayerInRange);
        BTCondition playerDetected = new BTCondition(IsPlayerDetected);
        BTCondition isdeadCondition = new BTCondition(() => dead);
        BTCondition canAttack = new BTCondition(CanAttack); ;

        attackSequence.AddChild(playerInRange);
        attackSequence.AddChild(canAttack);
        attackSequence.AddChild(attack);

        chaseSequence.AddChild(playerDetected);
        chaseSequence.AddChild(chaseAction);

        patrolSequence.AddChild(patrolAction);

        deathSequence.AddChild(isdeadCondition);
        deathSequence.AddChild(dieAction);

        root.AddChild(deathSequence);
        root.AddChild(attackSequence);
        root.AddChild(chaseSequence);
        root.AddChild(patrolSequence);

        root.Evaluate();
    }

    private void Update()
    {
        root.Evaluate();
    }

    private bool IsPlayerInRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        return distanceToPlayer <= attackRange;
    }

    private bool IsPlayerDetected()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        return distanceToPlayer <= detectionRange;
    }

    private bool CanAttack()
    {
        return Time.time >= nextAttackTime;
    }
    private void SetNextAttackTime()
    {
        nextAttackTime = Time.time + attackCooldown;
    }

    #region attack
    private BTNodeState Attack()
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

        // ���� ���� ���� (���� ����)
        Debug.Log("Performing Attack after delay!");

        // ���⿡ ���� ���� ���� �߰� (��: ��Ʈ�ڽ� Ȱ��ȭ, �ִϸ��̼� Ʈ����)
        PerformForwardAttack();
    }
    private void PerformForwardAttack()
    {
        // ���� ���� ���� (��: ��Ʈ�ڽ� �˻�)
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackBoxSize, 0);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Player"))
            {
                Debug.Log("Hit Player!");
                enemy.GetComponent<LivingEntity>().OnDamage(10);
                // ���⿡ �÷��̾�� ���ظ� �ִ� �ڵ� �߰�
            }
        }
    }
    #endregion 

    private BTNodeState Chase()
    {
        if (IsPlayerInRange())  // �÷��̾ ���� ���� �ȿ� �ִٸ� �߰��� ����
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
            return BTNodeState.Failure;  // �÷��̾ �����Ǹ� ������ �����.
        }
        rb.velocity = new Vector2(nextMove * moveSpeed, rb.velocity.y);
        return BTNodeState.Running;
    }
    private void Think()
    {
        nextMove = Random.Range(-1, 2);
        if (nextMove == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (nextMove == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        Invoke("Think", nextMoveTime);
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
        rb.velocity = Vector2.zero;  // ������ ����
        GetComponent<Collider2D>().enabled = false;  // �浹 ����
        Destroy(this.gameObject);
    }
    private void LookAtPlayer()
    {
        if (playerTransform == null) return;
        if (playerTransform.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackBoxSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
