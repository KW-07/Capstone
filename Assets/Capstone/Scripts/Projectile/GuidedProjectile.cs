using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    public Transform target;
    private float distanceToTargetToDestroyProjectile = 1f;

    private void Update()
    {
        Vector3 moveDirNormalized = (target.position - transform.position).normalized;
        transform.position += moveDirNormalized * speed;

        if (Vector3.Distance(transform.position, target.position) < distanceToTargetToDestroyProjectile)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log(collision.gameObject.tag);
            switch (collision.gameObject.tag)
            {
                case ("Ground"):
                case ("Platform"):
                    Destroy(gameObject);
                    break;
                case ("Enemy"):
                    break;
            }
        }
    }
}