using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using System.Data.Common;
using System.IO.Enumeration;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    private TypewriterEffect typewriterEffect;
    private GameObject player;
    [SerializeField] private Image playerHp;

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
    [SerializeField] private Dialogue testDialogue;

    [Header("Shop")]
    [SerializeField] private Item[] items;
    private int prevChildCount;
    [SerializeField] private int numProducts = 18;
    [SerializeField] private GameObject productGrid;
    [SerializeField] private GameObject shopItemUIObject;
    [SerializeField] private GameObject selectedGameobject;
    public int selectedIndex;
    [SerializeField] private int onePageProductCount = 6;

    [Space(10f)]
    // Description
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Image synergySprite_1;
    [SerializeField] private Image synergySprite_2;
    [SerializeField] private TMP_Text synergyKeyword_1;
    [SerializeField] private TMP_Text synergyKeyword_2;

    [Space(10f)]
    // Buy
    [SerializeField] private GameObject buyTapGameObject;
    [SerializeField] public bool buyTapOnOff;

    [Header("CommandCandidate")]
    [SerializeField] private GameObject candidateGrid;
    [SerializeField] private GameObject candidatePrefab;
    private TMP_Text candidateText;
    [SerializeField]private Sprite[] candidateSprite = new Sprite[8];
    private Image[] candidateUIImage = new Image[8];

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

        GameManager.instance.isConversation = false;
        onUI = false;
        numProducts = items.Length;

        CloseConversaiotnBox();
        //OffUI();

        prevChildCount = productGrid.transform.childCount;

        ShopConfiguration();
        InputItem();

        selectedIndex = 0;
        selectedGameobject = productGrid.transform.GetChild(selectedIndex).gameObject;

        productGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        buyTapOnOff = false;

        for(int i=0;i< items.Length; i++)
        {
            productGrid.transform.GetChild(i).Find("BuyTap").gameObject.SetActive(false);
        }

        candidateGrid.SetActive(false);
    }

    void Update()
    {
        // Player의 commandingTIme에 따른 commandTimeUI 이미지 변경
        if(GameManager.instance.isCommand)
        {
            commandTimeUI.fillAmount = PlayerCommand.instance.commandingTime;
        }
        else
        {
            commandTimeUI.fillAmount = 1;
        }

        GO_commandTimeUI.transform.position = player.transform.position + new Vector3(0,-1.5f,0);

        // 플레이어 HP바
        playerHp.fillAmount = GameManager.instance.currentHealthPoint / GameManager.instance.maxHealthPoint;

        if (Input.GetKeyDown(KeyCode.L) && !GameManager.instance.isConversation)
            showDialogue(testDialogue);

        ItemSelect();

        Description();
        BuyTap();

        if (GameManager.instance.isCommand)
            candidateGrid.SetActive(GameManager.instance.isCommand);
        else
            candidateGrid.SetActive(GameManager.instance.isCommand);

    }
    // 대화 시작
    public void showDialogue(Dialogue dialogue)
    {
        conversationBox.SetActive(true);
        letterBox.SetActive(true);
        GameManager.instance.isConversation = true;

        StartCoroutine(StepThroughDialogue(dialogue));
    }

    // 대화SO 길이만큼 실행
    private IEnumerator StepThroughDialogue(Dialogue Dialogue)
    {
        foreach(string dialogue in Dialogue.dialogueSO)
        {
            yield return typewriterEffect.Run(dialogue, textLabel);

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseConversaiotnBox();
                GameManager.instance.isConversation = false;
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
        GameManager.instance.isConversation = false;
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
        OffUI();
        UIOnOff(option);
    }

    void ShopConfiguration()
    {
        for(int i = prevChildCount; i < numProducts; i++)
        {
            GameObject instance = Instantiate(shopItemUIObject);
            instance.transform.SetParent(productGrid.transform);
            instance.name = "Product";
        }
    }

    void InputItem()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(productGrid.transform.GetChild(i).GetComponent<Product>().item == null)
            {
                productGrid.transform.GetChild(i).GetComponent<Product>().item = items[i];
            }
        }
    }

    void ItemSelect()
    {
        // 상점일 경우에만 적용
        if (GameManager.instance.isShop)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && !buyTapOnOff)
            {
                if (selectedIndex > 0)
                {
                    selectedIndex--;


                    ShopScrollDown('U');
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && !buyTapOnOff)
            {
                if (selectedIndex < numProducts - 1)
                {
                    selectedIndex++;


                    ShopScrollDown('D');
                }
            }
            selectedGameobject = productGrid.transform.GetChild(selectedIndex).gameObject;
            
            // 색칠
            for (int i = 0; i < numProducts; i++)
            {
                productGrid.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.gray;
            }

            selectedGameobject.GetComponent<Image>().color = Color.white;

        }
    }

    void ShopScrollDown(char a)
    {
        RectTransform selectedRectTransform = selectedGameobject.GetComponent<RectTransform>();
        float buttomLength = productGrid.GetComponent<RectTransform>().sizeDelta.y - productGrid.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        float topLength = productGrid.GetComponent<RectTransform>().sizeDelta.y - productGrid.transform.GetChild(numProducts - 1).gameObject.GetComponent<RectTransform>().anchoredPosition.y;

        float selectedRectPosYUp = selectedGameobject.GetComponent<RectTransform>().anchoredPosition.y - 160;
        float selectedRectPosYAbsoluteUp = selectedRectPosYUp < 0 ? selectedRectPosYUp *= -1 : selectedRectPosYUp *= 1;

        float selectedRectPosYDown = selectedGameobject.GetComponent<RectTransform>().anchoredPosition.y + 160;
        float selectedRectPosYAbsoluteDown = selectedRectPosYDown < 0 ? selectedRectPosYDown *= -1 : selectedRectPosYDown *= 1;

        float increaseFomula = 0;
        float decreaseFomula = 0;

        Debug.Log("buttomLength : " + buttomLength);
        Debug.Log("topLength : " + topLength);

        // increase
        if (selectedRectPosYAbsoluteUp > productGrid.GetComponent<RectTransform>().sizeDelta.y)
        {
            increaseFomula = buttomLength - selectedRectPosYAbsoluteUp + productGrid.GetComponent<GridLayoutGroup>().spacing.y + (320 * (selectedIndex - onePageProductCount + 1));
        }

        // decrease
        if (selectedRectPosYAbsoluteDown > productGrid.GetComponent<RectTransform>().sizeDelta.y)
        {
            decreaseFomula = topLength - selectedRectPosYAbsoluteDown + productGrid.GetComponent<GridLayoutGroup>().spacing.y - (320 * (onePageProductCount - selectedIndex + 3));
        }



        Debug.Log("selectedIndex : " + selectedIndex + ", increaseFomula : " + increaseFomula + ", decreaseFomula : " + decreaseFomula);

        switch (a)
        {
            case 'U':
                if(selectedIndex < numProducts - (onePageProductCount - 1))
                {
                    productGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, decreaseFomula);
                }
                break;
            case 'D':
                if(selectedIndex > onePageProductCount - 1)
                {
                    productGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, increaseFomula);
                }
                break;
            default:
                Debug.Log("It's Nothing.");
                break;
        }

    }

    void Description()
    {
        Item selectedItem = selectedGameobject.GetComponent<Product>().item;

        itemName.text = selectedItem.itemName;
        itemDescription.text = selectedItem.description;
    }

    void BuyTap()
    {
        buyTapGameObject = selectedGameobject.transform.Find("BuyTap").gameObject;
        bool isprocessed = false;

        if(GameManager.instance.isShop)
        {
            if(Input.GetKeyDown(KeyCode.Space) && !buyTapGameObject.activeSelf && !isprocessed)
            {
                buyTapOnOff = true;
                buyTapGameObject.SetActive(buyTapOnOff);

                isprocessed = true;
            }

            if(Input.GetKeyUp(KeyCode.Space) && buyTapGameObject.activeSelf && isprocessed)
            {
                isprocessed = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && buyTapGameObject.activeSelf && !isprocessed)
            {
                // 해당 아이템의 돈 만큼 빼고 인벤에 넣을 것
                buyTapOnOff = false;
                buyTapGameObject.SetActive(buyTapOnOff);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                buyTapOnOff = false;
                buyTapGameObject.SetActive(buyTapOnOff);
            }
        }
    }

    public void deleteCommandCandidate()
    {
        for(int i = 0; i< candidateGrid.transform.childCount;i++)
        {
            Destroy(candidateGrid.transform.GetChild(i).gameObject);
        }
    }

    public void CommandCandidate()
    {
        int j = 0;
        int commandCount = 0;

        // 일부 일치하는 커맨드의 수만큼 생성 및 Grid의 자식으로 부착
        for (int i = candidateGrid.transform.childCount; i < PlayerCommand.instance.usableCommandList.Length; i++)
        {
            //Debug.Log("생성!");
            GameObject candidateObj = Instantiate(candidatePrefab);
            candidateObj.transform.SetParent(candidateGrid.transform);

            // 텍스트 삽입
            candidateText = candidateObj.transform.Find("CandidateText").gameObject.GetComponent<TMP_Text>();
            candidateText.text = PlayerCommand.instance.usableCommandList[j].commandName;

            // 이미지 초기화
            for (int imgNum = 0; imgNum < candidateSprite.Length; imgNum++)
            {
                candidateSprite[imgNum] = null;
            }

            // 이미지 삽입
            for (int k =0;k< candidateSprite.Length;k++)
            {
                switch (PlayerCommand.instance.usableCommandList[j].command[k])
                {
                    case 1:
                        candidateSprite[k] = 
                            PlayerCommand.instance.commandIcon[PlayerCommand.instance.usableCommandList[j].command[k] - 1];
                        commandCount = PlayerCommand.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 2:
                        candidateSprite[k] =
                            PlayerCommand.instance.commandIcon[PlayerCommand.instance.usableCommandList[j].command[k] - 1];
                        commandCount = PlayerCommand.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 3:
                        candidateSprite[k] =
                            PlayerCommand.instance.commandIcon[PlayerCommand.instance.usableCommandList[j].command[k] - 1];
                        commandCount = PlayerCommand.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 4:
                        candidateSprite[k] =
                            PlayerCommand.instance.commandIcon[PlayerCommand.instance.usableCommandList[j].command[k] - 1];
                        commandCount = PlayerCommand.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 5:
                        candidateSprite[k] =
                            PlayerCommand.instance.commandIcon[PlayerCommand.instance.usableCommandList[j].command[k] - 1];
                        commandCount = PlayerCommand.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 6:
                        candidateSprite[k] =
                            PlayerCommand.instance.commandIcon[PlayerCommand.instance.usableCommandList[j].command[k] - 1];
                        commandCount = PlayerCommand.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 7:
                        candidateSprite[k] =
                            PlayerCommand.instance.commandIcon[PlayerCommand.instance.usableCommandList[j].command[k] - 1];
                        commandCount = PlayerCommand.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 8:
                        candidateSprite[k] =
                            PlayerCommand.instance.commandIcon[PlayerCommand.instance.usableCommandList[j].command[k] - 1];
                        commandCount = PlayerCommand.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    default:
                        break;
                }
            }

            GameObject candidateImageGrid = candidateObj.transform.Find("CandidateImageGrid").gameObject;

            // UI 동기화
            for (int k = 0; k< commandCount; k++)
            {
                candidateImageGrid.transform.GetChild(k).GetComponent<Image>().color = new Color(255, 255, 255, 255);
                candidateImageGrid.transform.GetChild(k).GetComponent<Image>().sprite = candidateSprite[k];
            }

            commandCount = 0;
            j++;
        }
    }
}
