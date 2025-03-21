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
    private int commandCount;
    private bool movePossible; // ��ġ�� �̵����� ��ũ��Ʈ�� ���� ����

    [Header("UI")]
    public GameObject commandTimeUI;
    private Vector2 currentPCommandSize = new Vector2(0,0);
    public GameObject pCommandUI;
    public GameObject pCommandUIGrid;
    [SerializeField] private Sprite[] commandIcon;
    [SerializeField] private Image[] pCommandIcon;

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
        // Ŀ�ǵ� ���� ��
        if (GameManager.instance.isCommand)
        {
            if(initTime == 0)
            {
                // ���� pCommand �� �ʱ�ȭ
                CommandInitialization(pCommand);

                currentPCommandSize = new Vector2(0, 0);
                // pCommandUI ������ �ʱⰪ ����
                pCommandUI.GetComponent<RectTransform>().sizeDelta = currentPCommandSize;

                // pCommandGrid ������ �ʱⰪ ����
                pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = pCommandUI.GetComponent<RectTransform>().sizeDelta;

                initTime++;
            }
            else if(initTime > 0)
            {
                commandingTime -= Time.unscaledDeltaTime;
                movePossible = BooleanOnOff(movePossible);

                Debug.Log(commandCount);
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 1;
                            commandCount++;
                            ShowPCommand(i);
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
                            break;
                        }
                    }
                }

                // Ŀ�ǵ� �ð� �ʰ� ��
                if (commandingTime <= 0)
                {
                    int sum = 0;
                    bool commandCount = false;

                    GameManager.instance.isCommand = BooleanOnOff(GameManager.instance.isCommand);

                    commandTimeUI.SetActive(false);
                    pCommandUI.SetActive(false);

                    for (int i=0;i<pCommand.Length;i++)
                    {
                        sum += pCommand[i];
                    }

                    // Ŀ�ǵ��� ���� 0 ���϶�� ��, �ƹ��͵� ������ �ʾҴٸ�
                    if (sum <= 0)
                    {
                        Debug.Log("Do not enter!");
                    }
                    // Ŀ�ǵ��� ���� 0 �ʰ��ϸ� ��, ���𰡰� ���ȴٸ�
                    else
                    {
                        Debug.Log("Entered!");

                        // Ŀ�ǵ� ����Ʈ Ž��
                        for (int i = 0; i < CommandManager.instance.commandList.Length; i++)
                        {
                            // Ŀ�ǵ尡 �����Ѵٸ�
                            if (Enumerable.SequenceEqual(pCommand, CommandManager.instance.commandList[i].command))
                            {
                                Debug.Log("Ŀ�ǵ� : " + CommandManager.instance.commandList[i].commandName);
                                commandCount = true;

                                SkillSystem.instance.command = CommandManager.instance.commandList[i];

                                // ��ų ���
                                // ������ ��� �÷��̾� ��ġ���� ����
                                if (CommandManager.instance.commandList[i].castPlayerPosition)
                                {
                                    skillSystem.UseSkill(gameObject, PlayerAttack.instance.neareastEnemy);
                                }
                                // ������ �ƴ� ��� �÷��̾� ���濡�� ����
                                else
                                {
                                    skillSystem.UseSkill(PlayerAttack.instance.shootPoint.gameObject, PlayerAttack.instance.neareastEnemy);
                                }

                                // Ŀ�ǵ� Ÿ�̸� ����
                                commandTimeUI.SetActive(false);
                                pCommandUI.SetActive(false);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (!commandCount)
                        {
                            Debug.Log("Nothing Command!");
                        }
                    }
                    // End
                    initTime = 0;
                    Time.timeScale = 1.0f;
                }
            }
        }
    }

    public void PlayerCommanding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // �ƹ� ��Ȳ�� �ƴ� ��쿡�� Ŀ�ǵ� ����
            if (GameManager.instance.nothingUI())
            {
                GameManager.instance.isCommand = BooleanOnOff(GameManager.instance.isCommand);

                GameManager.instance.isCommand = true;

                if (GameManager.instance.isCommand)
                {
                    commandTimeUI.SetActive(true);
                    pCommandUI.SetActive(true);
                    commandingTime = limitCommandTime;
                    Time.timeScale = inCommandingTimeScale;
                }
                else
                {
                    commandingTime = limitCommandTime;
                    Time.timeScale = 1.0f;
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

    // �ʱ� ������ ��ȯ
    private void PCommandInitSize()
    {
        currentPCommandSize = new Vector2(
            pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.left + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.right,
            pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.y + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.top + pCommandUIGrid.GetComponent<GridLayoutGroup>().padding.bottom);

        // pCommand�� �ʱⰪ ����
        pCommandUI.GetComponent<RectTransform>().sizeDelta = currentPCommandSize;

        // pCommandGrid�� �ʱⰪ ����
        pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
            pCommandUI.GetComponent<RectTransform>().sizeDelta.x, 
            pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void ShowPCommand(int i)
    {
        // ù��° ũ���� �⺻������ ����
        if(commandCount == 1)
        {
            PCommandInitSize();
        }
        // ���� ũ��� ���� ���� ���� ����
        else
        {
            // ����UI ���̰� + ���� ���� ũ�Ⱚ + ���� ����
            pCommandUI.GetComponent<RectTransform>().sizeDelta = new Vector2(
                pCommandUI.GetComponent<RectTransform>().sizeDelta.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().cellSize.x + pCommandUIGrid.GetComponent<GridLayoutGroup>().spacing.x,
                pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
            // ����UIGrid ���̰� + ���� ���� ũ�Ⱚ + ���� ����
            pCommandUIGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
                pCommandUI.GetComponent<RectTransform>().sizeDelta.x,
                pCommandUI.GetComponent<RectTransform>().sizeDelta.y);
        }

        // �̹��� ����
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
}
