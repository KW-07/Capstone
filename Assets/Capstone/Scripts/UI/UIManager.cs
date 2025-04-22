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
    [SerializeField] private GameObject skillTree;

    [Header("Command")]
    [SerializeField] private GameObject GO_commandTimeUI;
    [SerializeField] private Image commandTimeUI;

    [Header("Conversation")]
    [SerializeField] private GameObject conversationBox;
    [SerializeField] private GameObject letterBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private Dialogue testDialogue;

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
    PlayerStats playerstats;
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
        playerstats = GameObject.Find("Player").GetComponent<PlayerStats>();

        GameManager.instance.isConversation = false;
        onUI = false;

        CloseConversaiotnBox();

        candidateGrid.SetActive(false);
    }

    void Update()
    {
        // Player의 commandingTIme에 따른 commandTimeUI 이미지 변경
        if(GameManager.instance.isCommand)
        {
            commandTimeUI.fillAmount = Player.instance.commandingTime;
        }
        else
        {
            commandTimeUI.fillAmount = 1;
        }

        GO_commandTimeUI.transform.position = player.transform.position + new Vector3(0,-1.5f,0);

        // 플레이어 HP바
        playerHp.fillAmount = playerstats.currentHealth / playerstats.maxHealth;

        if (Input.GetKeyDown(KeyCode.L) && !GameManager.instance.isConversation)
            showDialogue(testDialogue);

        if (GameManager.instance.isCommand)
            candidateGrid.SetActive(GameManager.instance.isCommand);
        else
            candidateGrid.SetActive(GameManager.instance.isCommand);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(option.activeSelf)
            {
                option.SetActive(false);
            }
            else
            {
                option.SetActive(true);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (skillTree.activeSelf)
            {
                skillTree.SetActive(false);
            }
            else
            {
                skillTree.SetActive(true);
            }
        }

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
        for (int i = candidateGrid.transform.childCount; i < Player.instance.usableCommandList.Length; i++)
        {
            //Debug.Log("생성!");
            GameObject candidateObj = Instantiate(candidatePrefab);
            candidateObj.transform.SetParent(candidateGrid.transform);

            // 텍스트 삽입
            candidateText = candidateObj.transform.Find("CandidateText").gameObject.GetComponent<TMP_Text>();
            candidateText.text = Player.instance.usableCommandList[j].commandName;

            // 이미지 초기화
            for (int imgNum = 0; imgNum < candidateSprite.Length; imgNum++)
            {
                candidateSprite[imgNum] = null;
            }

            // 이미지 삽입
            for (int k =0;k< candidateSprite.Length;k++)
            {
                switch (Player.instance.usableCommandList[j].command[k])
                {
                    case 1:
                        candidateSprite[k] =
                            Player.instance.commandIcon[Player.instance.usableCommandList[j].command[k] - 1];
                        commandCount = Player.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 2:
                        candidateSprite[k] =
                            Player.instance.commandIcon[Player.instance.usableCommandList[j].command[k] - 1];
                        commandCount = Player.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 3:
                        candidateSprite[k] =
                            Player.instance.commandIcon[Player.instance.usableCommandList[j].command[k] - 1];
                        commandCount = Player.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 4:
                        candidateSprite[k] =
                            Player.instance.commandIcon[Player.instance.usableCommandList[j].command[k] - 1];
                        commandCount = Player.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 5:
                        candidateSprite[k] =
                            Player.instance.commandIcon[Player.instance.usableCommandList[j].command[k] - 1];
                        commandCount = Player.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 6:
                        candidateSprite[k] =
                            Player.instance.commandIcon[Player.instance.usableCommandList[j].command[k] - 1];
                        commandCount = Player.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 7:
                        candidateSprite[k] =
                            Player.instance.commandIcon[Player.instance.usableCommandList[j].command[k] - 1];
                        commandCount = Player.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
                        break;
                    case 8:
                        candidateSprite[k] =
                            Player.instance.commandIcon[Player.instance.usableCommandList[j].command[k] - 1];
                        commandCount = Player.instance.usableCommandList[j].command[k] - 1 > 0 ? commandCount + 1 : commandCount + 0;
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
