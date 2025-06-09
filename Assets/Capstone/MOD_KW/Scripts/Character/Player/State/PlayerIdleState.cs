using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    private PlayerController_ShadowGrid player;

    public PlayerIdleState(PlayerController_ShadowGrid player) { this.player = player; }

    public void Enter() { }
    public void Exit() { }
    public void Update()
    {
        if (player.IsGrounded())
        {
            if (player.HasMovementInput())
            {
                if (player.IsRunning()) player.ChangeState(new PlayerRunState(player));
                else player.ChangeState(new PlayerWalkState(player));
            }
        }

        if (player.IsJumpPressed() && player.IsGrounded())
            player.ChangeState(new PlayerJumpState(player));

        if (player.IsAttackPressed())
            player.ChangeState(new PlayerAttackState(player));

        if (player.IsCommandPressed())
            player.ChangeState(new PlayerCommandState(player));
    }

    public bool CanTransitionTo(IPlayerState newState) => true;
    public void OnAnimationEnd() { }
    public AnimationParameters GetAnimationParameters() => new();
}
