using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;

    public float posX;

    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.parent.transform.position;

        float posY = transform.position.y;

        Vector2 endPoint = new Vector2(posX, posY);

        float timepassed = 0f;

        while(timepassed < popDuration)
        {
            timepassed += Time.deltaTime;
            float linearT = timepassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
            yield return null;
        }
    }
}
