using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private Transform shootPoint;


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
}
