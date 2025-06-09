using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmAnimaitonEvents : MonoBehaviour
{
    [Header("애니메이션 중계점")]
    public PlayerController_ShadowGrid playerController;

    [Header("WeaponManager 중계점")]
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
