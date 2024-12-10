using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private bool onUI;

    [SerializeField] private GameObject option;
    [SerializeField] private GameObject information;
    
    void Start()
    {
        onUI = false;

        OffUI();
    }

    void Update()
    {
        
    }

    // UI 상태 Off
    void OffUI()
    {
        option.SetActive(false);
        information.SetActive(false);
    }

    // 키에 따른 해당UI 키기/끄기
    void UIOnOff(GameObject uiObject)
    {
        if(onUI)
        {
            onUI = false;
            uiObject.SetActive(false);

            Time.timeScale = 1.0f;
        }
        else
        {
            OffUI();

            onUI = true;
            uiObject.SetActive(true);

            Time.timeScale = 0f;
        }
    }

    void OnInformation()
    {
        OffUI();
        UIOnOff(information);
    }

    void OnOption()
    {
        // UI 켜져있으면 전부 끄기
        if(onUI)
        {
            OffUI();
        }
        // UI 꺼져있으면 옵션 키기
        else if(!onUI)
        {
            UIOnOff(option);
        }
    }
}
