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

    [Header("Option")]
    [SerializeField] private GameObject option;

    [Header("SkillTree")]
    SkillTreeManager skillTreeManager;
    [SerializeField] private GameObject skillTree;
    [SerializeField] private GameObject[] skillTree_Page;
    [SerializeField] private GameObject[] bookmark;
    [SerializeField] private GameObject bookmarkHighlight;
    public int skillPageCount;
    [SerializeField] private TMP_Text skillPointText;
    [SerializeField] private Image skillImage;
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillDescription;


    [Header("Command")]
    [SerializeField] private GameObject GO_commandTimeUI;
    [SerializeField] private Image commandTimeUI;

    [Header("Conversation")]
    [SerializeField] private GameObject conversationBox;
    [SerializeField] private GameObject letterBox;
    [SerializeField] private TMP_Text textLabel;

    [Header("CommandCandidate")]
    [SerializeField] private GameObject candidateGrid;
    [SerializeField] private GameObject candidatePrefab;
    private TMP_Text candidateText;
    [SerializeField]private Sprite[] candidateSprite = new Sprite[8];
    private Image[] candidateUIImage = new Image[8];

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
        skillTreeManager = GameObject.Find("SkillTreeManager").GetComponent<SkillTreeManager>();

        GameManager.instance.isConversation = false;

        CloseConversaiotnBox();

        candidateGrid.SetActive(false);

        option.SetActive(false);
        skillTree.SetActive(false);

        skillPageCount = 0;
    }

    void Update()
    {
        //if(GameManager.instance.isUI)
        //{
        //    Time.timeScale = 0;
        //}
        //else
        //{
        //    Time.timeScale = 1;
        //}

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
        //playerHp.fillAmount = playerstats.currentHealth / playerstats.maxHealth;


        if (GameManager.instance.isCommand)
            candidateGrid.SetActive(GameManager.instance.isCommand);
        else
            candidateGrid.SetActive(GameManager.instance.isCommand);

        // 옵션 OnOff
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            uiTapOnOff(option);
        }

        // 스킬트리 OnOff
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            uiTapOnOff(skillTree);
            skillTreeManager.ChangeNodeList();
        }

        moveSkillPage();
    }

    // 스킬트리 내부조작
    void moveSkillPage()
    {
        if (GameManager.instance.isUI && skillTree.activeSelf)
        {
            skillPointText.text = $"현재 포인트 : {skillTreeManager.availablePoints}";

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (skillPageCount <= 0)
                    skillPageCount = skillTree_Page.Length - 1;
                else
                    skillPageCount--;

                skillTreeManager.ChangeNodeList();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (skillPageCount >= skillTree_Page.Length - 1)
                    skillPageCount = 0;
                else
                    skillPageCount++;

                skillTreeManager.ChangeNodeList();
            }

            for (int i = 0; i < skillTree_Page.Length; i++)
            {
                if (i == skillPageCount)
                {
                    skillTree_Page[i].SetActive(true);
                    //bookmark[i].GetComponent<RectTransform>().position = new Vector2(-840f, 350f);
                    bookmarkHighlight.transform.position = bookmark[i].transform.position;
                }
                else
                {
                    skillTree_Page[i].SetActive(false);
                    //bookmark[i].GetComponent<RectTransform>().position = new Vector2(-800f, 350f);
                }
            }
        }
    }

    public void SkillNodeDescription(SkillNode currentNode)
    {
        skillImage.sprite = currentNode.GetComponent<SkillNode>().skill.skillImage;
        skillName.text = currentNode.GetComponent<SkillNode>().skill.skillName;
        skillDescription.text = currentNode.GetComponent<SkillNode>().skill.description;
    }

    void uiTapOnOff(GameObject uiTap)
    {
        if (uiTap.activeSelf)
        {
            uiTap.SetActive(false);
            GameManager.instance.isUI = false;
        }
        else
        {
            uiTap.SetActive(true);
            GameManager.instance.isUI = true;
        }
    }

    // 대화 시작
    public void showDialogue(Dialogue dialogue)
    {
        conversationBox.SetActive(true);
        letterBox.SetActive(true);
        GameManager.instance.isUI = true;

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
                GameManager.instance.isUI = false;
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
        GameManager.instance.isUI = false;
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
