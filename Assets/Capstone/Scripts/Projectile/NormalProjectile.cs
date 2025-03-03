using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //totalDamage = PlayerAttack.instance.SumDamage(gDamagePer, sDamagePer);

        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Player")
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
