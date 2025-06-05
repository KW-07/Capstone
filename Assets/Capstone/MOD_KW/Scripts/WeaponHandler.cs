using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    protected WeaponData weaponData;
    protected Collider weaponCollider;

    protected virtual void Awake()
    {
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false; // 기본 비활성화
    }

    public void SetWeaponData(WeaponData data)
    {
        weaponData = data;
    }

    public virtual void EnableWeapon()
    {
        Debug.Log("ColliderEnabled");
        weaponCollider.enabled = true;
    }

    public virtual void DisableWeapon()
    {
        Debug.Log("ColliderDisabled");
        weaponCollider.enabled = false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterBase_ShadowGrid>(out CharacterBase_ShadowGrid target))
        {
            target.TakeDamage(weaponData.damage);

            if (weaponData.weaponType == WeaponType.Fire)
            {
                if (other.TryGetComponent<FireEffectHandler>(out var fireHandler))
                {
                    fireHandler.ApplyDotDamage(weaponData.dotDamage, weaponData.dotDuration, weaponData.dotCoolTime);
                }
            }
        }
    }
}
