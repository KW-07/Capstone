using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool onUI;

    [Header("Option")]
    [SerializeField] private GameObject option;
    
    [Space(10f)]
    [Header("Information")]
    [SerializeField] private GameObject information;
    
    [Space(10f)]
    [Header("Command")]
    [SerializeField] private GameObject GO_commandTimeUI;
    [SerializeField] private Image commandTimeUI;

    [Space(10f)]
    [Header("Dialogue")]
    [SerializeField] private GameObject dialogueTap;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueSO textDialogue;

    private GameObject player;
    private TypewriterEffect typewriterEffect;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        typewriterEffect = GetComponent<TypewriterEffect>();
        
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
        CloseDialogueTap();
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

    // Conversation
    public void ShowDialogue(DialogueSO dialogueSO)
    {
        dialogueTap.SetActive(true);
        StartCoroutine(stepThroughDialogue(dialogueSO));
    }

    private IEnumerator stepThroughDialogue(DialogueSO dialogueSO)
    {
        foreach (string dialogue in dialogueSO.Dialogue)
        {
            yield return typewriterEffect.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        CloseDialogueTap();
    }

    private void CloseDialogueTap()
    {
        dialogueTap.SetActive(false);
        textLabel.text = string.Empty;
    }
}
