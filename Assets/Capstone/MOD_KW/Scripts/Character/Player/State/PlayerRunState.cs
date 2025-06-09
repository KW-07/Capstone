using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IPlayerState
{
    private PlayerController_ShadowGrid player;

    public PlayerRunState(PlayerController_ShadowGrid player) { this.player = player; }

    public void Enter() => player.SetSpeed(player.runSpeed);
    public void Exit() { }
    public void Update()
    {
        if (!player.HasMovementInput())
            player.ChangeState(new PlayerIdleState(player));

        if (!player.IsRunning())
            player.ChangeState(new PlayerWalkState(player));

        if (player.IsJumpPressed() && player.IsGrounded())
            player.ChangeState(new PlayerJumpState(player));

        if (player.IsAttackPressed())
            player.ChangeState(new PlayerAttackState(player));

        if (player.IsCommandPressed())
            player.ChangeState(new PlayerCommandState(player));
    }
    public bool CanTransitionTo(IPlayerState newState) => true;
    public void OnAnimationEnd() { }
    public AnimationParameters GetAnimationParameters()
    {
        var param = new AnimationParameters();
        param.Bools["isWalk"] = true;
        param.Bools["isRun"] = true;
        return param;
    }
}
