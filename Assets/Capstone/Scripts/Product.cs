using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    public Item item;

    [Header("Child")]
    [SerializeField] private Image itemImageSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image priceType;
    [SerializeField] private TextMeshProUGUI priceAmount;

    void Start()
    {
        // 아이템이 존재한다면 각 파트별로 삽입
        if(item != null)
        {
            itemImageSprite.sprite = item.itemImg;
            itemName.text = item.itemName;
            priceType.sprite = item.priceType;
            priceAmount.text = item.priceAmount.ToString();
        }
        else
        {
            itemImageSprite.sprite = null;
            itemName.text = null;
            priceType.sprite = null;
            priceAmount.text = null;
        }
    }

    void Update()
    {
        
    }
}
