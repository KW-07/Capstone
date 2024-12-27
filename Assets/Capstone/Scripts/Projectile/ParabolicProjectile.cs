using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float bulletspeed;

    [SerializeField] private float gDamagePer;
    [SerializeField] private float sDamagePer;
    public float totalDamage = 0;

    [SerializeField] private float destroyTime = 5;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.position * bulletspeed);
    }

    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        Destroy(gameObject, destroyTime);
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
