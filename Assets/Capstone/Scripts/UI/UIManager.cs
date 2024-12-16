using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool onUI;

    [SerializeField] private GameObject option;
    [SerializeField] private GameObject information;
    [SerializeField] private GameObject GO_commandTimeUI;
    [SerializeField] private Image commandTimeUI;

    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        onUI = false;

        OffUI();
    }

    void Update()
    {
        // Player�� commandingTIme�� ���� commandTimeUI �̹��� ����
        if(PlayerCommand.instance.isCommanding)
        {
            commandTimeUI.fillAmount = PlayerCommand.instance.commandingTime;
        }
        else
        {
            commandTimeUI.fillAmount = 1;
        }

        GO_commandTimeUI.transform.position = player.transform.position + new Vector3(0,-1.5f,0);
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
