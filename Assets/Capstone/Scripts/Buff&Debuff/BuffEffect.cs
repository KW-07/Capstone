using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffEffect : ScriptableObject
{
    public abstract void OnApply(CharacterStats target);
    public abstract void OnUpdate(CharacterStats target, float deltaTime);
    public abstract void OnRemove(CharacterStats target);
}
