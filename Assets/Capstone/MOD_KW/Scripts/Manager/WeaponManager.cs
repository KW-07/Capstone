using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Settings")]
    public Transform weaponPoint;

    private GameObject currentWeaponObject;
    private WeaponHandler currentWeaponHandler;

    [Header("Test")]
    public WeaponData equipWeaponData;

    private void Start()
    {
        EquipWeapon(equipWeaponData);
    }

    // ÀåÂøÇÔ¼ö
    public void EquipWeapon(WeaponData weaponData)
    {
        if (currentWeaponObject != null)
            Destroy(currentWeaponObject);

        currentWeaponObject = Instantiate(weaponData.weaponPrefab, weaponPoint);
        currentWeaponHandler = currentWeaponObject.GetComponent<WeaponHandler>();

        if (currentWeaponHandler != null)
        {
            currentWeaponHandler.SetWeaponData(weaponData);
        }
    }

    public WeaponHandler GetCurrentWeapon()
    {
        return currentWeaponHandler;
    }
}
