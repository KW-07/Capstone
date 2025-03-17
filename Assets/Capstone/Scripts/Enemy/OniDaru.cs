using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniDaru : MonoBehaviour
{
    public Transform playerTransform;
    public float attackRange = 1.5f;
    public float detectionRange = 5.0f; // �÷��̾ �����ϴ� �Ÿ�

    public bool ItemDrop;
    public float maxHealth = 100f;
    private float currentHealth;

    public float moveSpeed = 2.0f;

    public Vector2 attack1BoxSize;
    public float attackDelay = 1.0f;  // ������ ����? �Ʒ��� ��ٿ���� �ٸ���
    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;

    public int nextMoveTime = 3;
    private int nextMove;

    private Rigidbody2D rb;

    private bool isDead = false;

    private BTSelector root;

    private void Start()
    {
        currentHealth = maxHealth;
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
        BTCondition isdeadCondition = new BTCondition(() => isDead);
        BTCondition canAttack = new BTCondition(CanAttack);;

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
        Vector2 attackPosition = transform.position + transform.right * 1.5f; // ĳ���� ����

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attack1BoxSize, 0);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Player"))
            {
                Debug.Log("Hit Player!");
                //enemy.GetComponent<Player>().TakeDamage(10);
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

        Invoke("Think", nextMoveTime);
    }
    private BTNodeState Die()
    {
        return BTNodeState.Success;
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
}
