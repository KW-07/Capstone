using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private PlayerController_ShadowGrid player;

    public PlayerAttackState(PlayerController_ShadowGrid player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.TriggerAttack();
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (player.IsAttackPressed())
        {
            player.TriggerAttack();
        }

        if (player.initAttackCountTimer > 0 && player.IsAttackPressed())
        {
            player.ChangeState(new PlayerAttackState(player));
        }
    }

    public void OnAnimationEnd()
    {
        if (player.currentAttackCount >= player.maxAttackCount)
        {
            player.currentAttackCount = 0;
            player.ChangeState(new PlayerIdleState(player));
        }
        else
        {
            if (player.IsAttackPressed())
            {
                //player.TriggerAttack();
                player.ChangeState(new PlayerAttackState(player));
            }
            else
            {
                player.ChangeState(new PlayerIdleState(player));
            }
        }
    }

    public bool CanTransitionTo(IPlayerState newState) => true;

    public AnimationParameters GetAnimationParameters()
    {
        var param = new AnimationParameters();
        param.Bools["isAttack"] = true;
        param.Ints["attackCount"] = player.currentAttackCount;
        return param;
    }
}
