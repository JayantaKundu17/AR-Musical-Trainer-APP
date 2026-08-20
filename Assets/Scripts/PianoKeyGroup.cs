using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class PianoKeyGroup : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip pianoSound;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    [Header("Animation")]
    [SerializeField] private bool animateHit = true;

    [SerializeField] private float pressDistance = 0.02f;

    [SerializeField] private float animationSpeed = 15f;

    private AudioSource audioSource;
    private Vector3 originalPosition;
    private bool isAnimating;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;

        originalPosition = transform.localPosition;
    }

    // Unity Editor mouse testing
    private void OnMouseDown()
    {
        Play();
    }

    // Can also be called by HoloLens interaction
    public void Play()
    {
        if (pianoSound != null)
        {
            audioSource.PlayOneShot(pianoSound, volume);
        }

        if (animateHit && !isAnimating)
        {
            StartCoroutine(HitAnimation());
        }
    }

    private IEnumerator HitAnimation()
    {
        isAnimating = true;

        Vector3 pressedPosition =
            originalPosition + Vector3.down * pressDistance;

        // Move down
        while (Vector3.Distance(transform.localPosition, pressedPosition) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                pressedPosition,
                Time.deltaTime * animationSpeed
            );

            yield return null;
        }

        // Move back up
        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalPosition,
                Time.deltaTime * animationSpeed
            );

            yield return null;
        }

        transform.localPosition = originalPosition;

        isAnimating = false;
    }
}