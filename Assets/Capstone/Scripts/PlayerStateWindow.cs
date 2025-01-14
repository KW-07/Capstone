using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;

public class PlayerStateWindow : MonoBehaviour
{
    private QuickSlot quickSlot;
    private GameObject itemSlot_Up;
    private GameObject itemSlot_Down;
    private GameObject itemSlot_Right;
    private GameObject itemSlot_Left;

    void OnQuickSlot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.interaction is HoldInteraction)
            {
                
            }else if (context.interaction is PressInteraction)
            {
                quickSlot.UseItem(0);
            }
        }
    }
}
