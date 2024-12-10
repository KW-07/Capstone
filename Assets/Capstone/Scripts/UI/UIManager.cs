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

    // UI ���� Off
    void OffUI()
    {
        option.SetActive(false);
        information.SetActive(false);
    }

    // Ű�� ���� �ش�UI Ű��/����
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
        // UI ���������� ���� ����
        if(onUI)
        {
            OffUI();
        }
        // UI ���������� �ɼ� Ű��
        else if(!onUI)
        {
            UIOnOff(option);
        }
    }
}
