using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_ShadowGrid : CharacterBase_ShadowGrid
{
    public Transform target;
    public float moveSpeed = 3f;

    private void Update()
    {
        if(target == null)
        {
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += (direction * moveSpeed * Time.deltaTime);
    }

    protected override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}
