using System.Collections;
using UnityEngine;

public class InstrumentPressEffect : MonoBehaviour
{
    public enum EffectType
    {
        Scale,
        Rotate,
        ScaleAndRotate
    }

    [Header("Effect")]
    [SerializeField] private EffectType effect = EffectType.Scale;

    [Header("Scale")]
    [SerializeField] private float pressedScale = 0.95f;

    [Header("Rotation")]
    [SerializeField] private Vector3 rotationOffset = new Vector3(3f, 0f, 0f);

    [Header("Animation")]
    [SerializeField] private float pressTime = 0.05f;
    [SerializeField] private float releaseTime = 0.10f;

    private Vector3 originalScale;
    private Quaternion originalRotation;

    private Coroutine currentAnimation;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
    }

    public void Play()
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Vector3 targetScale = originalScale * pressedScale;
        Quaternion targetRotation =
            originalRotation * Quaternion.Euler(rotationOffset);

        float t = 0f;

        while (t < pressTime)
        {
            t += Time.deltaTime;

            float k = Mathf.Clamp01(t / pressTime);

            if (effect == EffectType.Scale ||
                effect == EffectType.ScaleAndRotate)
            {
                transform.localScale =
                    Vector3.Lerp(originalScale, targetScale, k);
            }

            if (effect == EffectType.Rotate ||
                effect == EffectType.ScaleAndRotate)
            {
                transform.localRotation =
                    Quaternion.Slerp(originalRotation, targetRotation, k);
            }

            yield return null;
        }

        t = 0f;

        while (t < releaseTime)
        {
            t += Time.deltaTime;

            float k = Mathf.Clamp01(t / releaseTime);

            if (effect == EffectType.Scale ||
                effect == EffectType.ScaleAndRotate)
            {
                transform.localScale =
                    Vector3.Lerp(targetScale, originalScale, k);
            }

            if (effect == EffectType.Rotate ||
                effect == EffectType.ScaleAndRotate)
            {
                transform.localRotation =
                    Quaternion.Slerp(targetRotation, originalRotation, k);
            }

            yield return null;
        }

        transform.localScale = originalScale;
        transform.localRotation = originalRotation;
    }
}