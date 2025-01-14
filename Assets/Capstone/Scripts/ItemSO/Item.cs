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
public enum Synergy //시너지 이름 짓는게 최대 난제
{
    None,
    A,
    B,
}
public enum ConsumableType //진짜 이름 짓는게 최대 난제
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
