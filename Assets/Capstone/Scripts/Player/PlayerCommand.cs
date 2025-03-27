using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCommand : MonoBehaviour
{
    public static PlayerCommand instance { get; private set; }

    public int[] pCommand = new int[8];

    [SerializeField]private float limitCommandTime;
    [SerializeField]private float inCommandingTimeScale;
    float initTime = 0;
    public float commandingTime = 0;
    [SerializeField]private int commandCount;
    private bool movePossible; // 합치고 이동관련 스크립트에 넣을 예정

    [Header("UI")]
    public GameObject commandTimeUI;
    private Vector2 currentPCommandSize = new Vector2(0,0);
    public GameObject pCommandUI;
    public GameObject pCommandUIGrid;
    public Sprite[] commandIcon;
    [SerializeField] private Image[] pCommandIcon;
    public CommandData[] usableCommandList;

    [Header("Skill")]
    public SkillSystem skillSystem;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }
    private void Start()
    {
        initTime = 0;
        commandCount = 0;
        GameManager.instance.isCommand = false;
        commandTimeUI.SetActive(false);
        pCommandUI.SetActive(false);
        CommandInitialization(pCommand);

        skillSystem = gameObject.GetComponent<SkillSystem>();
    }

    private void Update()
    {
        // 커맨드 시작 시
        if (GameManager.instance.isCommand)
        {
            if(initTime == 0)
            {
                // 기존 pCommand 값 초기화
                CommandInitialization(pCommand);

                currentPCommandSize = new Vector2(0, 0);
                // pCommandUI 사이즈 초기값 설정
                pCommandUI.GetComponent<RectTransform>().sizeDelta = currentPCommandSize;

                // pCommandGrid 사이즈 초기값 설정
                pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = pCommandUI.GetComponent<RectTransform>().sizeDelta;

                UIManager.instance.deleteCommandCandidate();

                initTime++;
            }
            else if(initTime > 0)
            {
                commandingTime -= Time.unscaledDeltaTime;
                movePossible = BooleanOnOff(movePossible);

                PCommandCandidate();

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 1;
                            commandCount++;
                            ShowPCommand(i);

                            UIManager.instance.deleteCommandCandidate();
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 2;
                            commandCount++;
                            ShowPCommand(i);

                            UIManager.instance.deleteCommandCandidate();
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 3;
                            commandCount++;
                            ShowPCommand(i);

                            UIManager.instance.deleteCommandCandidate();
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 4;
                            commandCount++;
                            ShowPCommand(i);

                            UIManager.instance.deleteCommandCandidate();
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 5;
                            commandCount++;
                            ShowPCommand(i);

                            UIManager.instance.deleteCommandCandidate();
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 6;
                            commandCount++;
                            ShowPCommand(i);

                            UIManager.instance.deleteCommandCandidate();
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 7;
                            commandCount++;
                            ShowPCommand(i);

                            UIManager.instance.deleteCommandCandidate();
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 8;
                            commandCount++;
                            ShowPCommand(i);

                            UIManager.instance.deleteCommandCandidate();
                            break;
                        }
                    }
                }
                UIManager.instance.CommandCandidate();

                // 커맨드 시간 초과 시
                if (commandingTime <= 0)
                {
                    bool bCommandCount = false;

                    GameManager.instance.isCommand = BooleanOnOff(GameManager.instance.isCommand);

                    commandTimeUI.SetActive(false);
                    pCommandUI.SetActive(false);
                    
                    if (commandCount <= 0)
                    {
                        
                    }
                    // 커맨드의 합이 0 초과하면 즉, 무언가가 눌렸다면
                    else
                    {

                        // 커맨드 리스트 탐색
                        for (int i = 0; i < CommandManager.instance.commandList.Length; i++)
                        {
                            // 커맨드가 존재한다면
                            if (Enumerable.SequenceEqual(pCommand, CommandManager.instance.commandList[i].command))
                            {
                                Debug.Log("커맨드 : " + CommandManager.instance.commandList[i].commandName);
                                bCommandCount = true;

                                SkillSystem.instance.command = CommandManager.instance.commandList[i];

                                // 스킬 사용
                                // 버프일 경우 플레이어 위치에서 생성
                                if (CommandManager.instance.commandList[i].castPlayerPosition)
                                {
                                    skillSystem.UseSkill(gameObject, PlayerAttack.instance.neareastEnemy);
                                }
                                // 버프가 아닐 경우 플레이어 전방에서 생성
                                else
                                {
                                    skillSystem.UseSkill(PlayerAttack.instance.shootPoint.gameObject, PlayerAttack.instance.neareastEnemy);
                                }

                                // 커맨드 타이머 삭제
                                commandTimeUI.SetActive(false);
                                pCommandUI.SetActive(false);

                                CommandInitialization(usableCommandList);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (!bCommandCount)
                        {
                            Debug.Log("Nothing Command!");

                            CommandInitialization(usableCommandList);
                        }
                    }
                    // End
                    initTime = 0;
                    //Time.timeScale = 1.0f;
                }
            }
        }
    }

    public void PlayerCommanding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // 아무 상황이 아닐 경우에만 커맨드 실행
            if (GameManager.instance.nothingUI())
            {
                GameManager.instance.isCommand = BooleanOnOff(GameManager.instance.isCommand);

                GameManager.instance.isCommand = true;

                if (GameManager.instance.isCommand)
                {
                    commandTimeUI.SetActive(true);
                    pCommandUI.SetActive(true);
                    commandingTime = limitCommandTime;
                    //Time.timeScale = inCommandingTimeScale;
                }
                else
                {
                    commandingTime = limitCommandTime;
                    //Time.timeScale = 1.0f;
                    commandTimeUI.SetActive(false);
                    pCommandUI.SetActive(false);

                }
            }
        }
    }

    private void CommandInitialization(int[] command)
    {
        for (int i = 0; i < command.Length; i++)
        {
            command[i] = 0;
        }
        commandCount = 0;
    }
    private void CommandInitialization(CommandData[] command)
    {
        for (int i = 0; i < command.Length; i++)
        {
            command[i] = null;
        }
        commandCount = 0;
    }

    // 초기 사이즈 변환
    private void PCommandInitSize()
    {
        currentPCommandSize = new Vector2(
            pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.left + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.right,
            pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.y + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.top + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.bottom);

        // pCommand값 초기값 조정
        pCommandUI.GetComponent<RectTransform>().sizeDelta = currentPCommandSize;

        // pCommandGrid값 초기값 조정
        pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
            pCommandUI.GetComponent<RectTransform>().sizeDelta.x, 
            pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void ShowPCommand(int i)
    {
        // 첫번째 크기은 기본값으로 설정
        if(commandCount == 1)
        {
            PCommandInitSize();
        }
        // 이후 크기는 일정 값에 따른 조정
        else
        {
            // 현재UI 길이값 + 다음 셀의 크기값 + 사이 간격
            pCommandUI.GetComponent<RectTransform>().sizeDelta = new Vector2(
                pCommandUI.GetComponent<RectTransform>().sizeDelta.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().spacing.x,
                pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
            // 현재UIGrid 길이값 + 다음 셀의 크기값 + 사이 간격
            pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
                pCommandUI.GetComponent<RectTransform>().sizeDelta.x,
                pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
        }

        // 이미지 삽입
        pCommandIcon[i].sprite = commandIcon[pCommand[i] - 1];
    }

    private bool BooleanOnOff(bool boolean)
    {
        if(boolean)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Candidate 탐색
    private void PCommandCandidate()
    {
        // 커맨드리스트의 개수만큼 할당
        usableCommandList = new CommandData[CommandManager.instance.commandList.Length];
        int j = 0;

        if(commandCount > 0)
        {
            for (int i = 0; i < CommandManager.instance.commandList.Length; i++)
            {
                if (pCommand.Take(commandCount).SequenceEqual(CommandManager.instance.commandList[i].command.Take(commandCount)))
                {
                    usableCommandList[j] = CommandManager.instance.commandList[i];
                    j++;
                }
            }
        }

        // 필요없는 부분은 삭제
        Array.Resize(ref usableCommandList, j);
    }
}
