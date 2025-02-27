using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_F_Chase : BasicCommand
{
    [SerializeField] private Vector2 hitBoxSize;
    [SerializeField] private Transform attackPoint;

    private void Start()
    {
        attackPoint = PlayerAttack.instance.shootPoint;
    }

    protected override void Update()
    {
        if (base.CanUseCommand())
        {
            UseCommand();
        }
    }

    public override void UseCommand()
    {
        // ���� �ڵ� ��
        PlayerAttack.instance.boxSize = hitBoxSize;
    }
}
