using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance { get; private set; }

    public Transform shootPoint;
    
    [SerializeField] private GameObject normalProjectile;

    public Transform pos;
    public Vector2 boxSize;
    public int damage;

    [SerializeField] private GameObject projectilePrefab;
    public Transform target;
    [SerializeField] private float projectileMoveSpeed;


    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }
    private void Start()
    {
    }

    // Melee
    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Enemy")
                {

                }
            }
            Debug.Log("attack");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    // Range
    public void RangeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (PlayerCommand.instance.isCommanding == false)
            {
                ParabolicProjectileAttack();
            }
        }
    }

    
    // The code at the bottom is related to the attack
    public void NormalProjectileAttack()
    {
        Instantiate(normalProjectile, shootPoint.position, shootPoint.rotation);
    }

    public void GuidedProjectileAttack()
    {
        GuidedProjectile projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity).GetComponent<GuidedProjectile>();
        projectile.InitializeProjectile(target, projectileMoveSpeed);
    }

    public void ParabolicProjectileAttack()
    {
        Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
    }

    
}
