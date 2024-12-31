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
    public ItemSlotUI[] uidSlot;     // UI ���� ������ ����
    public ItemSlot[] slots;            // ���� ������ ������ ����Ǵ� �迭

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
            // UI Slot �ʱ�ȭ�ϱ�
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
        if (item.canStack)  // �������� ���� �� �ִ� ���������� Ȯ��
        {
            // ���� �� �ִ� �������� ��� ������ �׾��ش�.
            ItemSlot slotToStackTo = GetItemStack(item);
            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }

        // ���� ��� ��ĭ�� �������� �߰����ش�.
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        // �κ��丮�� ��ĭ�� ���� ��� ȹ���� ������ �ٽ� ������
        // �� �κ��� �����ؼ� �������� ��ȯ�ϵ���
        ThrowItem(item);
    }

    private void ThrowItem(Item item)
    {
        // ������ ������
    }

    void UpdateUI()
    {
        // slots�� �ִ� ������ �����ͷ� UI�� Slot �ֽ�ȭ�ϱ�
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
        // ���� ���õ� �������� �̹� ���Կ� �ְ�, ���� �ִ������ �� �Ѱ�ٸ� �ش� �������� ��ġ�� ������ ��ġ�� �����´�.
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount)
                return slots[i];
        }

        return null;
    }

    ItemSlot GetEmptySlot()
    {
        // �� ���� ã��
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }

        return null;
    }

    public void SelectItem(int index)
    {
        // ������ ���Կ� �������� ���� ��� return
        if (slots[index].item == null) return;

        // ������ ������ ���� ��������
        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.itemName;
        selectedItemDescription.text = selectedItem.item.description;
    }

    private void ClearSelectItemWindow()
    {
        // ������ �ʱ�ȭ
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
    }

    public void UseItem()
    {
        // ������ Ÿ���� ��� ������ ���
        if (selectedItem.item.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].consumableType)
                {
                    // consumables Ÿ�Կ� ���� Heal�� Stamina
                    case ConsumableType.Health:
                        // ������ ��ġ��ŭ ü�� ȸ��
                        break;
                    case ConsumableType.Stamina:
                        // ������ ��ġ��ŭ ���׹̳� ȸ��
                        break;
                }
            }
        }
        // ����� ������ ���ֱ�
        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        selectedItem.quantity--;    // ���� ���.

        // �������� ���� ������ 0�� �Ǹ�
        if (selectedItem.quantity <= 0)
        {
            // ������ ���� �� UI������ ������ ���� �����
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
