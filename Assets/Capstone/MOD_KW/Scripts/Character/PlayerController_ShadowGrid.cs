using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_ShadowGrid : CharacterBase_ShadowGrid
{
    public float moveSpeed;
    private Vector3 moveInput;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        transform.Translate(moveInput * moveSpeed * Time.deltaTime);
    }

    protected override void Die()
    {
        base.Die();

    }
}
