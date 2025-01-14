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
    private bool movePossible; // 합치고 이동관련 스크립트에 넣을 예정

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
        
        // 커맨드 시작 시
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

                // 커맨드 시간 초과 시
                if (commandingTime <= 0)
                {
                    int sum = 0;
                    bool commandCount = false;

                    GameManager.instance.isCommand = BooleanOnOff(GameManager.instance.isCommand);

                    for (int i=0;i<pCommand.Length;i++)
                    {
                        sum += pCommand[i];
                    }

                    // 커맨드의 합이 0 이하라면 즉, 아무것도 누르지 않았다면
                    if (sum <= 0)
                    {
                        Debug.Log("Do not enter!");
                    }
                    // 커맨드의 합이 0 초과하면 즉, 무언가가 눌렸다면
                    else
                    {
                        Debug.Log("Entered!");

                        // 커맨드 리스트 탐색
                        for (int i = 0; i < CommandList.instance.commandList.Length; i++)
                        {
                            // 커맨드가 존재한다면
                            if (Enumerable.SequenceEqual(pCommand, CommandList.instance.commandList[i].command))
                            {
                                Debug.Log("커맨드 : " + CommandList.instance.commandList[i].commandName);
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
            // 아무 상황이 아닐 경우에만 커맨드 실행
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
