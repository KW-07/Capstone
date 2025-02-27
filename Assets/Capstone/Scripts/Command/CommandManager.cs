using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager instance;

    [Header("Command List")]
    public Command[] commandList;
    public string commandName;
    
    // A Ä¿¸Çµå·ù
    public C_F_Fist f_Fist { get; private set; }
    public C_F_Fierce f_Fierce { get; private set; }
    public C_F_Kick f_Kick { get; private set; }
    public C_F_Dance f_Dance { get; private set; }
    
    // S Ä¿¸Çµå·ù
    public C_F_Shuriken f_Shuriken { get; private set; }
    public C_F_Triple f_Triple { get; private set; }
    public C_F_Chase f_Chase { get; private set; }

    // D Ä¿¸Çµå·ù

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        commandName = null;

        f_Fist = GetComponent<C_F_Fist>();
        f_Fierce = GetComponent<C_F_Fierce>();
        f_Kick = GetComponent<C_F_Kick>();
        f_Dance = GetComponent<C_F_Dance>();

        f_Shuriken = GetComponent<C_F_Shuriken>();
        f_Triple = GetComponent<C_F_Triple>();
        f_Chase = GetComponent<C_F_Chase>();
    }

    private void Update()
    {
        switch(commandName)
        {
            case null:
                break;
            case "¿°±Ç":
                f_Fist.UseCommand();
                commandName = null;
                break;
            case "¸ÍÈ­":
                f_Fierce.UseCommand();
                commandName = null;
                break;
            case "È­°Ý":
                f_Kick.UseCommand();
                commandName = null;
                break;
            case "¿°¹«":
                f_Dance.UseCommand();
                commandName = null;
                break;
            case "È­¼ö":
                f_Shuriken.UseCommand();
                commandName = null;
                break;
            case "¿°È­":
                f_Triple.UseCommand();
                commandName = null;
                break;
            case "ÃßÀû¿°":
                f_Chase.UseCommand();
                commandName = null;
                break;
            case "´ëÈ­¿°":
            default:
                break;
        }
    }
}
