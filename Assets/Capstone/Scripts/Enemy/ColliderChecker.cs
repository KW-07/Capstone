using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ColliderChecker : MonoBehaviour
{
    public string colliderType; // "Circle" �Ǵ� "Box" ������ ����

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player detected by {colliderType} Collider");
            transform.parent.GetComponent<BossDaru>().PlayerDetected(colliderType, other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player exited by {colliderType} Collider");
            transform.parent.GetComponent<BossDaru>().PlayerExited(colliderType, other);
        }
    }
}
