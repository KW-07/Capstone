using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.TextCore.LowLevel;

public class GameManager : MonoBehaviour
{
    // 싱글톤
    public static GameManager instance { get; private set; }

    bool isFirstPlay;

    [Header("State")]
    public bool isGrounded = false;
    public bool isMeleeAttack = false;
    public bool isRangeAttack = false;
    public bool isCommandAction = false;

    [Header("UIState")]
    public bool isConversation = false;
    public bool isCommand = false;
    public bool isUI = false;

    [Header("BossEvent")]
    public bool isBossBattle;
    [SerializeField] private GameObject fakeWall;
    [SerializeField] private GameObject bossObject;
    [SerializeField] private GameObject bossHpUI;

    [Header("Event")]
    [SerializeField] private Dialogue testDialogue;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    private void Start()
    {
        isFirstPlay = true;

        isBossBattle = false;

        
    }

    private void Update()
    {
        if (isBossBattle)
        {
            fakeWall.SetActive(true);
        }

        // Scene 넘어오면 시작
        //if (isFirstPlay)
        //{
        //    UIManager.instance.showDialogue(testDialogue);
        //    isFirstPlay = false;
        //}
    }

    public bool nothingUI() // safe모드 나가기 위해서 임시로 다시 넣었는데 나중에 수정하기. Player에서 6개 참조중
    {
        //if (!isConversation && !isCommand)
            return true;
        //else
          //  return false;
    }
    public void Save(ref GameManagerSaveData data)
    {
        data.isFirstPlay = this.isFirstPlay;
    }

    public void Load(GameManagerSaveData data)
    {
        this.isFirstPlay = data.isFirstPlay;
    }
}

[System.Serializable]
public struct GameManagerSaveData
{
    public bool isFirstPlay;
}
