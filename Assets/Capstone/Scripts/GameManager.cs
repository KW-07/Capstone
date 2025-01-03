using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform player;

    private float respawnTime = 2f;

    public bool isPlayerDie;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    private void Start()
    {
        isPlayerDie = false;
    }

    private void Update()
    {
        if(isPlayerDie)
        {
            Invoke("Respawn", respawnTime);
        }
    }

    private void Respawn()
    {
        // 현재 맵이 마을일 경우 리스폰포인트에서, 아닐경우 마을 및 리스폰포인트에서 리스폰
        if(SceneManager.GetActiveScene().name == "Village")
        {
            player.position = respawnPoint.position;
            isPlayerDie = false;
        }
        else
        {
            SceneManager.LoadScene("Village");
            player.position = respawnPoint.position;
            isPlayerDie = false;
        }
    }
}
