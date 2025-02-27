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

    // ��밡�� ���� Ȯ��
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

    // ��ų ����
    public virtual void UseCommand()
    {

    }
}
