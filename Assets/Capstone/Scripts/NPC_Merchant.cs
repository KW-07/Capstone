using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Merchant : MonoBehaviour
{
    [SerializeField] private GameObject storeUI;

    private bool isInteractable;
    private void Start()
    {
        isInteractable = false;
        storeUI.SetActive(false);
    }

    private void Update()
    {
        if(isInteractable)
        {
            if(Input.GetKeyDown(KeyCode.A) && !GameManager.instance.isShop)
            {
                GameManager.instance.isShop = true;
                UIManager.instance.selectedIndex = 0;
                storeUI.SetActive(true);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.isShop && !UIManager.instance.buyTapOnOff)
        {
            GameManager.instance.isShop = false;
            storeUI.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Player")
        {
            isInteractable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInteractable = false;
        }
    }
}
