using UnityEngine;

[System.Serializable]
public class Itemslot
{
    public Item item;
    public int stackCount;
    public Itemslot(Item item, int stackCount)
    {
        this.item = item;
        this.stackCount = stackCount;
    }

}
public class QuickSlot : MonoBehaviour
{
    public Itemslot[] Itemslot;
    public int maxQuickSlot = 5;
    
    private void Start()
    {
        Itemslot = new Itemslot[maxQuickSlot];
    }

    public void RegisterItem(Item item) // ��� ������ ������ �μ��� �־���� �� �ʿ䰡 ������?
    {
        for (int i = 0; i < Itemslot.Length; i++)   // ���� �������� �ִ� ���� ã�Ƽ� ���� ���� ����
        {
            if (Itemslot[i].stackCount < item.maxStackAmount)
            {
                Itemslot[i].stackCount++;
                Debug.Log("item stack");
                return;
            }
            else 
            {
                Debug.Log("cannot add more item");
                return;
            }
        }

        for(int i = 0; i < Itemslot.Length; i++)   // �� ���Կ� ���� ���
        {
            if (Itemslot == null)
            {
                Itemslot[i] = new Itemslot(item, 1); // ������ ������ ������ �μ��� �޾ƿ��� �̰��� �ֱ�
                Debug.Log("RegisterItem");
                return;
            }
        }
    }
    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Itemslot.Length || Itemslot[slotIndex] == null)
        {
            Debug.LogError($"Quick Slot {slotIndex + 1} is empty or invalid!");
            return;
        }

        Itemslot slot = Itemslot[slotIndex];
        Debug.Log($"Used item: {slot.item.itemName}");

        // ���� ����
        slot.stackCount--;
        if (slot.stackCount <= 0)
        {
            Debug.Log($"Quick Slot {slotIndex + 1} is now empty.");
            Itemslot[slotIndex] = null; // ���� ����
        }
    }
}
