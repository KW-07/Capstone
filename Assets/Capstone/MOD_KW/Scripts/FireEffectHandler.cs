using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffectHandler : MonoBehaviour
{
    private Coroutine dotCoroutine;

    public void ApplyDotDamage(int damage, float duration, float coolTime)
    {
        if (dotCoroutine != null)
        {
            StopCoroutine(dotCoroutine);
        }
        dotCoroutine = StartCoroutine(DotDamageRoutine(damage, duration, coolTime));
    }

    private IEnumerator DotDamageRoutine(int damage, float duration, float coolTime)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (TryGetComponent<CharacterBase_ShadowGrid>(out var target))
            {
                target.TakeDamage(damage);
            }

            yield return new WaitForSeconds(coolTime);
            elapsed += coolTime;
        }

        dotCoroutine = null;
    }
}
