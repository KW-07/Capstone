using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public float baseDamage = 10f;

    public float knockbackForce = 8f;
    public float knockbackDuration = 0.25f;

    public abstract void OnHit(CharacterBase_ShadowGrid target);
}
