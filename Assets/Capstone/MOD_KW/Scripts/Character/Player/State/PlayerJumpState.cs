using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : IPlayerState
{
    private PlayerController_ShadowGrid player;

    public PlayerJumpState(PlayerController_ShadowGrid player) { this.player = player; }

    public void Enter()
    {
        player.Jump();
    }

    public void Exit() { }

    public void Update()
    {
        if (player.IsAttackPressed())
            player.ChangeState(new PlayerAttackState(player));
    }

    public bool CanTransitionTo(IPlayerState newState)
    {
        return newState is PlayerAttackState;
    }

    public void OnAnimationEnd()
    {
        player.ChangeState(new PlayerIdleState(player));
    }

    public AnimationParameters GetAnimationParameters()
    {
        var param = new AnimationParameters();
        param.Bools["isJump"] = true;
        return param;
    }
}
