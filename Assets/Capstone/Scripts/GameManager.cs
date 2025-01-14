using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �̱���
    public static GameManager instance { get; private set; }

    [Header("Player")]
    [SerializeField] private Transform player;
    public bool isPlayerLive;

    [Header("Status")]
    [SerializeField] private float healthPoint;
    [SerializeField] private float StaminaPoint;

    [Header("Currency")]
    private float currency1;
    private float currency2;
    private float currency3;

    [Header("PlayerRespwan")]
    [SerializeField] private Transform respawnPoint;
    private float respawnTime = 2f;

    [Header("Item")]
    public Item[] item;

    [Header("State")]
    public bool isConversation;
    public bool isCommand;
    public bool isShop;

    [Header("SynergySprite")]
    public Sprite synergy_Sprite_A;
    public Sprite synergy_Sprite_B;
    public Sprite synergy_Sprite_C;
    public Sprite synergy_Sprite_D;
    public Sprite synergy_Sprite_E;

    [Header("SpriteEffect")]
    public string synergy_Effect_A;
    public string synergy_Effect_B;
    public string synergy_Effect_C;
    public string synergy_Effect_D;
    public string synergy_Effect_E;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    private void Start()
    {
        isPlayerLive = true;
    }

    private void Update()
    {
        
    }

    public bool nothingState()
    {
        if (!isConversation && !isCommand && !isShop)
            return true;
        else
            return false;
    }

    public void GameOver()
    {
        isPlayerLive=false;

        // ��� �� ���� �ð� �� ������
        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        // ���� ���� ������ ��� ����������Ʈ����, �ƴҰ�� ���� �� ����������Ʈ���� ������
        if(SceneManager.GetActiveScene().name == "Village")
        {
            player.position = respawnPoint.position;
            isPlayerLive = true;
        }
        else
        {
            SceneManager.LoadScene("Village");
            player.position = respawnPoint.position;
            isPlayerLive = true;
        }
    }

    // Coin �߰�
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
    // Coin ����Ҹ�ŭ �ִ°�
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
    // Coin �ش� ����ŭ ���
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
