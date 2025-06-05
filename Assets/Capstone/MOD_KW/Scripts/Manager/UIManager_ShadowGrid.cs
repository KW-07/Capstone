using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_ShadowGrid : MonoBehaviour
{
    public MouseLook mouseLook;



    public void OpenMouseMovableUI()
    {
        mouseLook.SetUIActive(true);
    }

    public void CloseMouseMovableUI()
    {
        mouseLook.SetUIActive(false);
    }
}
