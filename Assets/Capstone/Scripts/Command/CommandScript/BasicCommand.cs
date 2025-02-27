using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCommand : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    // 사용가능 여부 확인
    public virtual bool CanUseCommand()
    {
        if (cooldownTimer < 0)
        {
            UseCommand();
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    // 스킬 내용
    public virtual void UseCommand()
    {

    }
}
