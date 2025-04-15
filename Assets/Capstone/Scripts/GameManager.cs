using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    // 싱글톤
    public static GameManager instance { get; private set; }

    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    public bool isPlayerLive;

    [Header("Status")]
    public float maxHealthPoint;
    public float currentHealthPoint;
    //[SerializeField] private float StaminaPoint;
    public bool isBuff = false;
    public float increaseDamageBuff;
    public float increaseSpeedBuff;

    [Header("Currency")]
    private float currency1;
    private float currency2;
    private float currency3;

    [Header("PlayerRespwan")]
    [SerializeField] private Transform respawnPoint;
    private float respawnTime = 2f;

    [Header("State")]
    public bool isGrounded = false;
    public bool isMeleeAttack = false;
    public bool isRangeAttack = false;
    public bool isCommandAction = false;

    [Header("UIState")]
    public bool isConversation = false;
    public bool isCommand = false;
    public bool isShop = false;

    [Header("BossEvent")]
    public bool isBossBattle;
    [SerializeField] private GameObject fakeWall;
    [SerializeField] private GameObject bossObject;
    [SerializeField] private GameObject bossHpUI;


    public Player player { get; set; }
    public SceneData sceneData { get; set; }
    public SceneLoader sceneLoader { get; set; }

    private bool _isSaving;
    private bool _isLoading;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    private void Start()
    {
        isPlayerLive = true;
        isBossBattle = false;
    }

    private void Update()
    {
        if(isBossBattle)
        {
            fakeWall.SetActive(true);
        }

        // 세이브 및 로드
        // 현시점 사용안되어 주석처리
        //if(!_isSaving)
        //{
        //    SaveAsync();
        //}
        //if(!_isLoading)
        //{
        //    LoadAsync();
        //}
    }

    private async void SaveAsync()
    {
        _isSaving = true;
        await SaveSystem.SaveAsynchronously();
        _isSaving = false;
    }

    private async void LoadAsync()
    {
        _isLoading = true;
        await SaveSystem.LoadAsync();
        _isLoading = false;
    }

    public bool nothingUI()
    {
        if (!isConversation && !isCommand && !isShop)
            return true;
        else
            return false;
    }

    public void GameOver()
    {
        isPlayerLive=false;

        // 사망 시 일정 시간 후 리스폰
        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        // 현재 맵이 마을일 경우 리스폰포인트에서, 아닐경우 마을 및 리스폰포인트에서 리스폰
        if(SceneManager.GetActiveScene().name == "Village")
        {
            playerTransform.position = respawnPoint.position;
            isPlayerLive = true;
        }
        else
        {
            SceneManager.LoadScene("Village");
            playerTransform.position = respawnPoint.position;
            isPlayerLive = true;
        }
    }

    // Coin 추가
    public void AddCoin(string currency, int amount)
    {
        switch(currency)
        {
            case "currency1":
                currency1 += amount;
                break;
            case "currency2":
                currency2 += amount;
                break;
            case "currency3":
                currency3 += amount;
                break;
            default:
                Debug.Log(currency + "is not valid");
                break;
        }
    }
    // Coin 사용할만큼 있는가
    public bool CanSpendCoin(string currency, int amount)
    {
        switch(currency)
        {
            case "currency1":
                return (currency1 >= amount);
            case "currency2":
                return (currency2 >= amount);
            case "currency3":
                return (currency3 >= amount);
            default:
                Debug.Log(currency + "is not valid");
                return false;
        }
    }
    // Coin 해당 값만큼 사용
    public void SpendCoin(string currency, int amount)
    {
        switch (currency)
        {
            case "currency1":
                currency1 -= amount;
                break;
            case "currency2":
                currency2 -= amount;
                break;
            case "currency3":
                currency3 -= amount;
                break;
            default:
                Debug.Log(currency + "is not valid");
                break;
        }
    }

    
}
