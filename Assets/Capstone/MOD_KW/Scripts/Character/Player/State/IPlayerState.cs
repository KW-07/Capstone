using UnityEngine;

public interface IPlayerState
{
    void Enter();
    void Update();
    void Exit();
    bool CanTransitionTo(IPlayerState newState);
    void OnAnimationEnd();

    AnimationParameters GetAnimationParameters();
}