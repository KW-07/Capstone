using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : MonoBehaviour
{
    [SerializeField]private float damage = 20f;
    [SerializeField] private float speed = 10f;

    private Rigidbody2D rb;

    private bool playerRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * speed;
    }

    void Update()
    {
        Destroy(gameObject, 5f);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log(collision.gameObject.tag);
    //    switch(collision.gameObject.tag)
    //    {
    //        case ("Ground"):
    //        case ("Platform"):
    //            Destroy(gameObject);
    //            break;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
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

    void Damage(GameObject enemyObject)
    {

    }
}
