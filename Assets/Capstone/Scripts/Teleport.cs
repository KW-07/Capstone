using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private bool teleportable;
    [SerializeField]
    private string map3DName;

private void Update()
    {
        if(teleportable && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(map3DName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        teleportable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        teleportable = false;
    }
}
