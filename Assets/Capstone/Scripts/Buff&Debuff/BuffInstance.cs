using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInstance
{
    public Buff Buff { get; private set; }
    private CharacterStats target;
    private float timeRemaining;

    public bool IsFinished => timeRemaining <= 0;

    // 이 버프가 변경하는 수치값 (예: +10 데미지)
    public float ModifierValue { get; private set; }

    public BuffInstance(Buff buff, CharacterStats target)
    {
        Buff = buff;
        this.target = target;
        timeRemaining = buff.duration;

        ModifierValue = buff.GetModifierValue();
        Debug.Log($"BuffInstance 생성: {Buff.buffName}, ModifierValue: {ModifierValue}");

        target.AddBuffModifier(this); // 등록 및 재계산 호출
    }

    public void Update(float deltaTime)
    {
        timeRemaining -= deltaTime;

        if (IsFinished)
        {
            target.RemoveBuffModifier(this); // 해제 및 재계산 호출
        }
    }

    public void Refresh()
    {
        timeRemaining = Buff.duration;
    }
}
