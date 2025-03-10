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

    public void RegisterItem(Item item) // 얻는 아이템 개수를 인수로 넣어줘야 할 필요가 있을듯?
    {
        for (int i = 0; i < Itemslot.Length; i++)   // 같은 아이템이 있는 슬롯 찾아서 스택 개수 증가
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

        for(int i = 0; i < Itemslot.Length; i++)   // 빈 슬롯에 새로 등록
        {
            if (Itemslot == null)
            {
                Itemslot[i] = new Itemslot(item, 1); // 위에서 아이템 개수를 인수로 받아오면 이곳에 넣기
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

        // 스택 감소
        slot.stackCount--;
        if (slot.stackCount <= 0)
        {
            Debug.Log($"Quick Slot {slotIndex + 1} is now empty.");
            Itemslot[slotIndex] = null; // 슬롯 비우기
        }
    }
}
