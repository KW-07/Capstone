using System.Collections;
using UnityEngine;

public class ParabolicProjectile : MonoBehaviour
{
    public AnimationCurve curve;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float heightY = 3f;

    [SerializeField] private float speed = 2f;

    Vector2 target2;
    Vector3 target3;

    [SerializeField] private float gDamagePer;
    [SerializeField] private float sDamagePer;
    public float totalDamage = 0;

    [SerializeField] private float destroyTime = 5;

    private void Start()
    {
        target2 = new Vector2(Player.instance.neareastEnemy.transform.position.x, Player.instance.neareastEnemy.transform.position.y);
        target3 = new Vector3(Player.instance.shootPoint.position.x, Player.instance.shootPoint.position.y, Player.instance.shootPoint.position.z);
        StartCoroutine(Curve(target3, target2));
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

    //public void InitializeProjectile(Transform target, float speed)
    //{
    //    this.target = target;
    //    this.speed = speed;
    //}
}
