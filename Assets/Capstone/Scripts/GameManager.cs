using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤
    public static GameManager instance { get; private set; }

    [Header("Player")]
    [SerializeField] private Transform player;
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

    [Header("Item")]
    public Dictionary<ItemType, List<Item>> equipmentInventory;
    private Dictionary<ItemType, int> maxEquipmentCount;

    private QuickSlot quickSlot;

    [Header("State")]
    public bool isConversation;
    public bool isCommand;
    public bool isShop;

    [Header("BossEvent")]
    public bool isBossBattle;
    [SerializeField] private GameObject fakeWall;
    [SerializeField] private GameObject bossObject;
    [SerializeField] private GameObject bossHpUI;

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

        equipmentInventory = new Dictionary<ItemType, List<Item>>
        { 
            { ItemType.Gear, new List<Item>() },
            { ItemType.Talisman, new List<Item>() },
            { ItemType.Essence, new List<Item>() }         
        };
        maxEquipmentCount = new Dictionary<ItemType, int>
        {
            { ItemType.Gear , 6 },
            { ItemType.Talisman , 1 },
            { ItemType.Essence , 1 }
        };

        quickSlot = gameObject.AddComponent<QuickSlot>();
    }

    private void Update()
    {
        if(isBossBattle)
        {
            fakeWall.SetActive(true);
        }
    }

    public void EquipItem(Item item)
    {
        ItemType type = item.itemType;
        if (type == ItemType.Consumable)
        {
            for (int i = 1; i < quickSlot.maxQuickSlot; i++) 
                {
                    if (quickSlot.Itemslot[i] == null)
                    {
                        quickSlot.RegisterItem(item);
                        return;
                    }
                }
        }
        else if (equipmentInventory.ContainsKey(type))
        {
            if (equipmentInventory[type].Count >= maxEquipmentCount[type])
            {
                Debug.Log("inventory is full");
                return;
            }
            equipmentInventory[type].Add(item);
            Debug.Log($"Equipped {item.itemName} in {type} slot");

        }
        else
        {
            Debug.Log("out of type");
        }
    }
    public void UnEquipItem(ItemType type, Item item)
    {
        if (equipmentInventory.ContainsKey(type) && equipmentInventory[type].Contains(item))
        {
            equipmentInventory[type].Remove(item);
            Debug.Log($"Unequipped {item.itemName} from {type} slot");
        }
        else
        {
            Debug.Log("error"); 
        }
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

        // 사망 시 일정 시간 후 리스폰
        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        // 현재 맵이 마을일 경우 리스폰포인트에서, 아닐경우 마을 및 리스폰포인트에서 리스폰
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
