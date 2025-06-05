using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public float baseDamage = 10f;

    public abstract void OnHit(CharacterBase_ShadowGrid target);
}
