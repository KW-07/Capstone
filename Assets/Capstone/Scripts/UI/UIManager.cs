using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public bool onUI;

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
            //if (uiObject.activeSelf)
            //{
            //    onUI = false;
            //    uiObject.SetActive(false);
            //}
            //else
            //{
            //    OffUI();

            //    onUI = true;
            //    uiObject.SetActive(true);
            //}
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
        OffUI();
        UIOnOff(option);
    }
}
