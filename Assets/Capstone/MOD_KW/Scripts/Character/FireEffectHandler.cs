using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffectHandler : MonoBehaviour
{
    private Coroutine dotCoroutine;

    public void ApplyDotDamage(float damage, float duration, float coolTime)
    {
        Debug.Log("Fire");
        if (dotCoroutine != null)
        {
            StopCoroutine(dotCoroutine);
        }
        dotCoroutine = StartCoroutine(DotDamageRoutine(damage, duration, coolTime));
    }

    private IEnumerator DotDamageRoutine(float damage, float duration, float coolTime)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            //if (TryGetComponent<CharacterBase_ShadowGrid>(out var target))
            //{
            //    target.TakeDamage(damage);
            //}
            CharacterBase_ShadowGrid characterBase = gameObject.GetComponent<CharacterBase_ShadowGrid>();
            if (characterBase != null)
            {
                characterBase.TakeDamage(damage);
            }

            yield return new WaitForSeconds(coolTime);
            elapsed += coolTime;

            Debug.Log($"{elapsed}√  ∏Æ≈œ");
        }

        dotCoroutine = null;
    }
}
