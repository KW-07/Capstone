using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandState : IPlayerState
{
    private PlayerController_ShadowGrid player;

    public PlayerCommandState(PlayerController_ShadowGrid player) { this.player = player; }

    public void Enter()
    {
        player.ExecuteCommand();
    }

    public void Exit() { }

    public void Update() { }

    public bool CanTransitionTo(IPlayerState newState) => false;

    public void OnAnimationEnd()
    {
        player.ChangeState(new PlayerIdleState(player));
    }

    public AnimationParameters GetAnimationParameters()
    {
        var param = new AnimationParameters();
        param.Bools["isCommand"] = true;
        return param;
    }
}
