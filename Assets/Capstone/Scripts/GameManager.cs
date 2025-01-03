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
        // ���� ���� ������ ��� ����������Ʈ����, �ƴҰ�� ���� �� ����������Ʈ���� ������
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
