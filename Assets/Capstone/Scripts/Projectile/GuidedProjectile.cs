using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedProjectile : MonoBehaviour
{
    GameObject target;
    [SerializeField] GameObject explosion;
    [SerializeField] float speed = 2f, rotSpeed = 2f;

    Quaternion rotTarget;
    Rigidbody2D rb;
    Vector3 dir;

    [SerializeField] private float gDamagePer;
    [SerializeField] private float sDamagePer;
    public float totalDamage = 0;

    [SerializeField] private float destroyTime = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Enemy");
    }

    void Update()
    {
        Destroy(gameObject, destroyTime);
    }

    private void GuidedMissile()
    {
        dir = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rotTarget = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, Time.deltaTime * rotSpeed);
        rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log(collision.gameObject.tag);
            Debug.Log(totalDamage);
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