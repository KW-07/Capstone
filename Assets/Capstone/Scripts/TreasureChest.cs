using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] private bool isLootable = false;

    [SerializeField] private GameObject[] item;

    [SerializeField] private RectTransform itemArray;
    private float arrayLength;
    private float firstItemPosX = 0.5f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        firstItemPosX = this.gameObject.transform.position.x + 0.6f * (1 - item.Length);

        arrayLength = (item.Length + 0.2f * (item.Length - 1));
        setWidth(arrayLength);

    }

    private void Update()
    {
        if(isLootable)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.gray;
        }

        if(isLootable)
        {
            // 상자 파밍
            if(Input.GetKeyDown(KeyCode.A))
            {
                isLootable = false;
                // Chest Open
                this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                Debug.Log("ChestOpen");

                foreach(GameObject i in item)
                {
                    GameObject chestItem = Instantiate(i);
                    chestItem.transform.SetParent(itemArray.transform);
                    chestItem.gameObject.GetComponent<ItemDrop>().posX = firstItemPosX;

                    firstItemPosX += 1.2f;
                }
                itemArray.gameObject.SetActive(true);
            }
        }
    }

    void setWidth(float width)
    {
        itemArray.sizeDelta = new Vector2(width, itemArray.sizeDelta.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(this.name + other.gameObject.name + " 충돌 ");
        if (other.gameObject.tag == "Player")
        {
            isLootable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isLootable = false;
        }
    }
}
