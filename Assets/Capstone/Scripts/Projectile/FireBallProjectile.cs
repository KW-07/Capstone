using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;

    [SerializeField] private float duration = 1f;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private float speed = 2f;
    
    public AnimationCurve curve;

    Vector2 target2;
    Vector3 target3;

    private void Start()
    {
        target2 = new Vector2(Player.instance.neareastEnemy.transform.position.x, Player.instance.neareastEnemy.transform.position.y);
        target3 = new Vector3(Player.instance.shootPoint.position.x, Player.instance.shootPoint.position.y, Player.instance.shootPoint.position.z);
        StartCoroutine(Curve(target3, target2));
        Destroy(gameObject, lifetime); // ���� �ð� �� �ڵ� �ı�
    }
    public IEnumerator Curve(Vector3 start, Vector2 target)
    {
        float timePassed = 0f;

        Vector2 end = target;

        while (timePassed < duration)
        {
            timePassed += (Time.deltaTime * speed);

            float linearT = timePassed / duration;
            float heightT = curve.Evaluate(linearT);

            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);

            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾�� �������� ��
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // ���� ������ �ı�
            Destroy(gameObject);
        }
    }
}
