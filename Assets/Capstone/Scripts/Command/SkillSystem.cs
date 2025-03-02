using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    public static SkillSystem instance { get; private set; }

    public CommandData command;

    [SerializeField] private float cooldown;
    float cooldownTimer;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    private void Update()
    {
        if(command != null)
        {
            cooldown = command.cooldown;
        }

        cooldownTimer -= Time.deltaTime;
    }

    // 사용가능 여부 확인
    private bool CanUseCommand()
    {
        if (cooldownTimer < 0)
        {
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    public void UseSkill(GameObject caster, GameObject target)
    {
        if(CanUseCommand())
        {
            command.ActivateSkill(caster, target);
        }
        else
        {
            Debug.Log($"{command.commandName}은 쿨타임중입니다. ");
        }
    }
}
