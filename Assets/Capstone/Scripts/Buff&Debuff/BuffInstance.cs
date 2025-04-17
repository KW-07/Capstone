using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInstance
{
    public Buff Buff { get; private set; }
    private CharacterStats target;
    private float timeRemaining;

    public bool IsFinished => timeRemaining <= 0;

    public BuffInstance(Buff buff, CharacterStats target)
    {
        this.Buff = buff;
        this.target = target;
        timeRemaining = buff.duration;

        foreach (var effect in buff.effects)
            effect.OnApply(target);
    }

    public void Update(float deltaTime)
    {
        timeRemaining -= deltaTime;

        foreach (var effect in Buff.effects)
            effect.OnUpdate(target, deltaTime);

        if (IsFinished)
        {
            foreach (var effect in Buff.effects)
                effect.OnRemove(target);
        }
    }

    public void Refresh()
    {
        timeRemaining = Buff.duration;
        Debug.Log($"버프 {Buff.buffName} 지속시간 갱신됨: {timeRemaining}초");
    }
}
