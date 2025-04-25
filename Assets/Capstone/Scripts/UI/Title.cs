using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public float delaySeconds = 0f; // �� ��ȯ���� ��ٸ� �ð�
    public string nextSceneName = "NextScene"; // ��ȯ�� �� �̸�

    private bool isTriggered = false;
    float time;

    void Update()
    {
        if (time < 1f)
        {
            GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1 - time);
        }
        else
        {
            GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, time);
            if (time > 2f)
            {
                time = 0;
            }
        }
        time += Time.deltaTime;
        if (!isTriggered && Input.anyKeyDown)
        {
            isTriggered = true;
            Invoke("LoadNextScene", delaySeconds);
        }
    }
    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}