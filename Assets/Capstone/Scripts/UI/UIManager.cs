using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    private TypewriterEffect typewriterEffect;
    private GameObject player;

    [Header("Option/Information")]
    [SerializeField] private GameObject option;
    [SerializeField] private GameObject information;

    [Header("Command")]
    [SerializeField] private GameObject GO_commandTimeUI;
    [SerializeField] private Image commandTimeUI;

    [Header("Conversation")]
    [SerializeField] private GameObject conversationBox;
    [SerializeField] private GameObject letterBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueSO testDialogue;
    public bool isConversaiton;

    [Header("Shop")]
    [SerializeField] private int numProducts = 18;
    [SerializeField] private GameObject productGrid;
    [SerializeField] private GameObject shopItemUIObject;

    private bool onUI;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

        void Start()
    {
        player = GameObject.FindWithTag("Player");
        typewriterEffect = GetComponent<TypewriterEffect>();

        isConversaiton = false;
        onUI = false;
        CloseConversaiotnBox();

        OffUI();
    }

    void Update()
    {
        // Player의 commandingTIme에 따른 commandTimeUI 이미지 변경
        if(PlayerCommand.instance.isCommanding)
        {
            commandTimeUI.fillAmount = PlayerCommand.instance.commandingTime;
        }
        else
        {
            commandTimeUI.fillAmount = 1;
        }

        GO_commandTimeUI.transform.position = player.transform.position + new Vector3(0,-1.5f,0);

        if (Input.GetKeyDown(KeyCode.L) && !isConversaiton)
            showDialogue(testDialogue);

        ShopConfiguration();
    }

    // 대화 시작
    public void showDialogue(DialogueSO dialogueSO)
    {
        conversationBox.SetActive(true);
        letterBox.SetActive(true);
        isConversaiton = true;

        StartCoroutine(StepThroughDialogue(dialogueSO));
    }

    // 대화SO 길이만큼 실행
    private IEnumerator StepThroughDialogue(DialogueSO dialogueSO)
    {
        foreach(string dialogue in dialogueSO.Dialogue)
        {
            yield return typewriterEffect.Run(dialogue, textLabel);

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseConversaiotnBox();
                isConversaiton = false;
            }
            // space 입력 후 다음 대화 진행
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        CloseConversaiotnBox();
    }

    private void CloseConversaiotnBox()
    {
        conversationBox.SetActive(false);
        letterBox.SetActive(false);
        textLabel.text = string.Empty;
        isConversaiton = false;
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

    void ShopConfiguration()
    {
        if(productGrid.transform.childCount != numProducts)
        {
            GameObject instance = Instantiate(shopItemUIObject);
            instance.transform.parent = productGrid.transform;
            instance.name = "Product";
        }
    }
}
