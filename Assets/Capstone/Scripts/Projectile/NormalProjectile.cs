using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        playerRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().playerFlipx;

        if(playerRenderer)
        {
            rb.velocity = -transform.right * speed;
        }
        else
        {
            rb.velocity = transform.right * speed;
        }
        //rb.velocity = transform.right * speed;
    }
}
