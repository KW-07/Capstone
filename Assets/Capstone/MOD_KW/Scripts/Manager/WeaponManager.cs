using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Settings")]
    public Transform weaponPoint;

    private GameObject currentWeaponObject;
    private WeaponHandler currentWeaponHandler;

    [Header("Temporary")]
    [SerializeField] private GameObject tempcurrentWeaponObject;
    [SerializeField] private WeaponData tempWeaponData;

    private void Start()
    {
        //임시
        currentWeaponHandler = tempcurrentWeaponObject.GetComponent<WeaponHandler>();
        if (currentWeaponHandler != null)
        {
            currentWeaponHandler.SetWeaponData(tempWeaponData);
        }
    }

    // 장착함수
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
