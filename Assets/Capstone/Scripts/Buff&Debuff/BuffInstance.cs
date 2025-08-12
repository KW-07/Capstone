using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInstance
{
    public Buff Buff { get; private set; }
    private CharacterStats target;
    private float timeRemaining;

    public bool IsFinished => timeRemaining <= 0;

    // �� ������ �����ϴ� ��ġ�� (��: +10 ������)
    public float ModifierValue { get; private set; }

    public BuffInstance(Buff buff, CharacterStats target)
    {
        Buff = buff;
        this.target = target;
        timeRemaining = buff.duration;

        ModifierValue = buff.GetModifierValue();
        Debug.Log($"BuffInstance ����: {Buff.buffName}, ModifierValue: {ModifierValue}");

        target.AddBuffModifier(this); // ��� �� ���� ȣ��
    }

    public void Update(float deltaTime)
    {
        timeRemaining -= deltaTime;

        if (IsFinished)
        {
            target.RemoveBuffModifier(this); // ���� �� ���� ȣ��
        }
    }

    public void Refresh()
    {
        timeRemaining = Buff.duration;
    }
}
