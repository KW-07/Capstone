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
    public int dotDamage;
    public float dotDuration;
    public float dotCoolTime;
}
