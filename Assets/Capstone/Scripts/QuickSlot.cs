using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
public class ItemSlot
{
    public Item item;
    public int quantity;
}

public class QuickSlot : MonoBehaviour
{
    public ItemSlotUI[] uidSlot;     // UI 상의 아이템 슬롯
    public ItemSlot[] slots;            // 실제 아이템 슬롯이 저장되는 배열

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;

    public Item testItem; 

    //private PlayerStatus status;

    public static QuickSlot instance;

    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        slots = new ItemSlot[uidSlot.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            // UI Slot 초기화하기
            slots[i] = new ItemSlot();
            uidSlot[i].index = i;
            uidSlot[i].Clear();
        }
        ClearSelectItemWindow();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            AddItem(testItem);
            Debug.Log("water");
        }
    }
    public void OnQuickSlot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.interaction is HoldInteraction)
            {
                Debug.Log("hold");
            }
            if (context.interaction is PressInteraction)
            {
                SelectItem(0);
                UseItem();
                Debug.Log("press");
            }
        }
    }

    public void AddItem(Item item)
    {
        if (item.canStack)  // 아이템이 쌓일 수 있는 아이템인지 확인
        {
            // 쌓을 수 있는 아이템일 경우 스택을 쌓아준다.
            ItemSlot slotToStackTo = GetItemStack(item);
            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }

        // 없을 경우 빈칸에 아이템을 추가해준다.
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        // 인벤토리에 빈칸이 없을 경우 획득한 아이템 다시 버리기
        // 이 부분을 수정해서 아이템을 교환하도록
        ThrowItem(item);
    }

    private void ThrowItem(Item item)
    {
        // 아이템 버리기
    }

    void UpdateUI()
    {
        // slots에 있는 아이템 데이터로 UI의 Slot 최신화하기
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                uidSlot[i].Set(slots[i]);
            else
                uidSlot[i].Clear();
        }
    }

    void UpdateSlotHighlight()
    {
        for (int i = 0; i < uidSlot.Length; i++)
        {
            uidSlot[i].SetHighlight(i == selectedItemIndex);
        }
    }
    ItemSlot GetItemStack(Item item)
    {
        // 현재 선택된 아이템이 이미 슬롯에 있고, 아직 최대수량을 안 넘겼다면 해당 아이템이 위치한 슬롯의 위치를 가져온다.
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount)
                return slots[i];
        }

        return null;
    }

    ItemSlot GetEmptySlot()
    {
        // 빈 슬롯 찾기
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }

        return null;
    }

    public void SelectItem(int index)
    {
        // 선택한 슬롯에 아이템이 없을 경우 return
        if (slots[index].item == null) return;

        // 선택한 아이템 정보 가져오기
        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.itemName;
        selectedItemDescription.text = selectedItem.item.description;
    }

    private void ClearSelectItemWindow()
    {
        // 아이템 초기화
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
    }

    public void UseItem()
    {
        // 아이템 타입이 사용 가능할 경우
        if (selectedItem.item.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].consumableType)
                {
                    // consumables 타입에 따라 Heal과 Stamina
                    case ConsumableType.Health:
                        // 아이템 수치만큼 체력 회복
                        break;
                    case ConsumableType.Stamina:
                        // 아이템 수치만큼 스테미나 회복
                        break;
                }
            }
        }
        // 사용한 아이템 없애기
        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        selectedItem.quantity--;    // 수량 깎기.

        // 아이템의 남은 수량이 0이 되면
        if (selectedItem.quantity <= 0)
        {
            // 아이템 제거 및 UI에서도 아이템 정보 지우기
            selectedItem.item = null;
            ClearSelectItemWindow();
        }

        UpdateUI();
    }

    public void RemoveItem(Item item)
    {

    }

    public bool HasItems(Item item, int quantity)
    {
        return false;
    }
}
