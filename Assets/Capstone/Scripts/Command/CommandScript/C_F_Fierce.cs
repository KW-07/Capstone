using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_F_Fierce : BasicCommand
{
    protected override void Update()
    {
        if (base.CanUseCommand())
        {
            UseCommand();
        }
    }

    public override void UseCommand()
    {
        // 실행 코드 ↓

    }
}
