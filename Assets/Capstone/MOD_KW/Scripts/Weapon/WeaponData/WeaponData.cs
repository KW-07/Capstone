using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Normal,
    Fire
}

[CreateAssetMenu(fileName = "New WeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public WeaponType weaponType;
    public int damage;
    [Header("Knockback Effect")]
    public float knockbackForce = 8f;
    public float knockbackDuration = 0.25f;
    [Header("Dot Effect")]
    public float dotDamage = 2f;
    public float dotDuration = 5f;
    public float dotCoolTime = 1f;
}
