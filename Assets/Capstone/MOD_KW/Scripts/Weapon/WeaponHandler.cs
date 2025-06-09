using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    protected WeaponData weaponData;
    [SerializeField]protected Collider weaponCollider;

    protected virtual void Awake()
    {
        if (weaponCollider == null)
            weaponCollider = GetComponent<Collider>();

        weaponCollider.enabled = false;
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
            //µ¥¹ÌÁö
            weaponData.weaponPrefab.GetComponent<WeaponBase>().OnHit(target);
        }
    }
}
