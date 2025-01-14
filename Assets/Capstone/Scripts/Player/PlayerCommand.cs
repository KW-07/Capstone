using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCommand : MonoBehaviour
{
    public static PlayerCommand instance { get; private set; }

    public int[] pCommand = new int[8];

    [SerializeField]private float limitCommandTime;
    [SerializeField]private float inCommandingTimeScale;
    float initTime = 0;
    public float commandingTime = 0;
    private bool movePossible; // ��ġ�� �̵����� ��ũ��Ʈ�� ���� ����

    public GameObject commandTimeUI;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }
    private void Start()
    {
        initTime = 0;
        GameManager.instance.isCommand = false;
        //commandTimeUI.SetActive(false);
        CommandInitialization(pCommand);
    }

    private void Update()
    {
        
        // Ŀ�ǵ� ���� ��
        if (GameManager.instance.isCommand)
        {
            if(initTime == 0)
            {
                CommandInitialization(pCommand);
                initTime++;
            }
            else if(initTime > 0)
            {
                commandingTime -= Time.unscaledDeltaTime;
                Debug.Log(commandingTime);
                movePossible = BooleanOnOff(movePossible);

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    for (int i = 0; i < pCommand.Length; i++)
                    {
                        if (pCommand[i] == 0)
                        {
                            pCommand[i] = 1;
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
                        for (int i = 0; i < CommandList.instance.commandList.Length; i++)
                        {
                            // Ŀ�ǵ尡 �����Ѵٸ�
                            if (Enumerable.SequenceEqual(pCommand, CommandList.instance.commandList[i].command))
                            {
                                Debug.Log("Ŀ�ǵ� : " + CommandList.instance.commandList[i].commandName);
                                PlayerAttack.instance.commandObject = CommandList.instance.commandList[i].commandObject;
                                commandCount = true;

                                PlayerAttack.instance.CommandAttack();
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
            if (GameManager.instance.nothingState())
            {
                GameManager.instance.isCommand = BooleanOnOff(GameManager.instance.isCommand);

                GameManager.instance.isCommand = true;

                if (GameManager.instance.isCommand)
                {
                    commandTimeUI.SetActive(true);
                    commandingTime = limitCommandTime;
                    Time.timeScale = inCommandingTimeScale;
                }
                else
                {
                    commandingTime = limitCommandTime;
                    Time.timeScale = 1.0f;
                    commandTimeUI.SetActive(false);

                }
            }
        }
    }

    private void CommandInitialization(int[] command)
    {
        for(int i=0;i<command.Length;i++)
        {
            command[i] = 0;
        }
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
