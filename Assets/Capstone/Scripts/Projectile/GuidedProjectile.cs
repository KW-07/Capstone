using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform target;
    private float distanceToTargetToDestroyProjectile = 1f;


    [SerializeField] private float gDamagePer;
    [SerializeField] private float sDamagePer;
    public float totalDamage = 0;

    [SerializeField] private float destroyTime = 5;

    private void Update()
    {
        Vector3 moveDirNormalized = (target.position - transform.position).normalized;
        transform.position += moveDirNormalized * speed;

        if (Vector3.Distance(transform.position, target.position) < distanceToTargetToDestroyProjectile)
        {
            Destroy(gameObject);
        }
    }

    public void InitializeProjectile(Transform target, float speed)
    {
        this.target = target;
        this.speed = speed;
    }
    
}