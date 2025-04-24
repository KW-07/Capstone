using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public float delaySeconds = 0f; // 씬 전환까지 기다릴 시간
    public string nextSceneName = "NextScene"; // 전환할 씬 이름

    private bool isTriggered = false;
    float time;

    void Update()
    {
        if(time < 0.5f)
        {
            GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1 - time);
        }
        else
        {
            GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, time);
            if(time > 1f)
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
