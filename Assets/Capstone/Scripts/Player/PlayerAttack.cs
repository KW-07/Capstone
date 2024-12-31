using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private Transform shootPoint;

    public Transform pos;
    public Vector2 boxSize;
    public int damage;

    private void Start()
    {
    }
    public void RangeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            if(PlayerCommand.instance.isCommanding == false)
            {
                Instantiate(normalProjectile, shootPoint.position, shootPoint.rotation);
            }
    }

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
}
