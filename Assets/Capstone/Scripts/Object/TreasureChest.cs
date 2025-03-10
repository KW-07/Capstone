using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] private bool isLootable = false;
    [SerializeField] private bool isExist = true;

    [SerializeField] private GameObject[] item;
    [SerializeField] private int numDropItem;
    private int[] randomItem;

    [SerializeField] private RectTransform itemArray;
    private float arrayLength;
    private float firstItemPosX = 0.5f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        isExist = true;

        spriteRenderer = GetComponent<SpriteRenderer>();

        firstItemPosX = this.gameObject.transform.position.x + 0.6f * (1 - numDropItem);

        arrayLength = (numDropItem + 0.2f * (numDropItem - 1));
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

        if(isLootable && isExist)
        {
            // 상자 파밍
            if(Input.GetKeyDown(KeyCode.A))
            {
                randomItem = UtilScripts.RandomArray(0, item.Length, numDropItem);

                isLootable = false;
                isExist = false;
                // Chest Open
                this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                Debug.Log("ChestOpen");

                for(int i=0;i< numDropItem; i++)
                {
                    // 생성
                    GameObject chestItem = Instantiate(item[randomItem[i]]);
                    Debug.Log(randomItem[i]);

                    // 자식지정 및 위치값 조정
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
