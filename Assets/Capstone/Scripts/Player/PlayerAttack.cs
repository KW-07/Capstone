using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private Transform shootPoint;
    public void RangeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            Instantiate(normalProjectile, shootPoint.position, shootPoint.rotation);
    }
}
