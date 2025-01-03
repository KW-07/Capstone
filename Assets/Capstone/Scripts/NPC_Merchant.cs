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
            if(Input.GetKeyDown(KeyCode.A) && !UIManager.instance.isConversaiton)
            {
                UIManager.instance.isConversaiton = true;
                storeUI.SetActive(true);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) && UIManager.instance.isConversaiton)
        {
            UIManager.instance.isConversaiton = false;
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
