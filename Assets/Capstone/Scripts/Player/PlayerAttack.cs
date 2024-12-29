using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance { get; private set; }

    [SerializeField] private Transform shootPoint;
    
    [SerializeField] private GameObject normalProjectile;

    [SerializeField] private float gDamage = 0;
    [SerializeField] private float sDamage = 0;


    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform target;
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
    public void RangeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (PlayerCommand.instance.isCommanding == false)
            {
                // Normal
                //Instantiate(normalProjectile, shootPoint.position, shootPoint.rotation);

                // Guided
                //GuidedProjectile projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity).GetComponent<GuidedProjectile>();
                //projectile.InitializeProjectile(target, projectileMoveSpeed);

                // Parabolic

            }
        }
    }

    public float SumDamage(float gDamagePer, float sDamagePer)
    {
        float totalDamage = 0;
        totalDamage = (this.gDamage * gDamagePer) + (this.sDamage * sDamagePer);
        return totalDamage;
    }
}
