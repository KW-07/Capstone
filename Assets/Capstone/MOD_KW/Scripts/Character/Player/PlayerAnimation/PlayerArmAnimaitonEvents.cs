using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmAnimaitonEvents : MonoBehaviour
{
    [Header("�ִϸ��̼� �߰���")]
    public PlayerController_ShadowGrid playerController;

    [Header("WeaponManager �߰���")]
    public WeaponManager weaponManager;

    private void Awake()
    {
        weaponManager = GetComponentInParent<WeaponManager>();
    }
    public void OnAnimationEnd()
    {
        if (playerController != null)
        {
            playerController.OnAnimationEnd();
        }
    }
    public void EnableWeapon()
    {
        weaponManager.GetCurrentWeapon()?.EnableWeapon();
    }

    public void DisableWeapon()
    {
        weaponManager.GetCurrentWeapon()?.DisableWeapon();
    }

}
