using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Consumable,
    Gear,
    Talisman,
    Essence,
}
public enum Synergy //�ó��� �̸� ���°� �ִ� ����
{
    None,
    A,
    B,
}
public enum ConsumableType //��¥ �̸� ���°� �ִ� ����
{
    Health,
    Stamina,
}

[Serializable]
public class ItemConsumable
{
    public ConsumableType consumableType;
    public float value;
}

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public ItemType itemType;
    public Synergy[] synergy;
    public string itemName;
    public string description;  
    public Sprite itemImg;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemConsumable[] consumables;

    [Header("Store")]
    public Sprite priceType;
    public int priceAmount;
}
