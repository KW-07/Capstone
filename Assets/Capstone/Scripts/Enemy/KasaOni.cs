using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasaOni : MonoBehaviour
{
    public bool ItemDrop;
    public float maxHP = 100f;
    private float currentHP;

    public float moveSpeed = 2.0f;

    public float attackCooldown = 2.0f;
    private float nextAttackTime = 0f;

    public float jumpCooldown = 3.0f;
    private float nextJumpTime = 0f;

    private Rigidbody2D rb;

    private bool isDead = false;

    private bool isPlayerDetected = false;  // 플레이어 감지 여부
    private bool isPlayerInRange = false;   // 공격 범위 안에 있는지 여부

    private BTSelector root;

    private void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();

        root = new BTSelector();

        BTSequence attackSequence = new BTSequence();
        BTSequence chaseSequence = new BTSequence();
        BTSequence patrolSequence = new BTSequence();
        BTSequence deathSequence = new BTSequence();
    }
    private BTNodeState Attack()
    {
        return BTNodeState.Success;
    }
    private BTNodeState Chase()
    {
        return BTNodeState.Running;
    }
    private BTNodeState Patrol()
    {
        return BTNodeState.Running;
    }
    private BTNodeState Die()
    {
        return BTNodeState.Success;
    }
}
