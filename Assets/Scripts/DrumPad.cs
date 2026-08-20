using System.Collections;
using UnityEngine;

/// <summary>
/// Handles interaction with a drum pad.
/// Supports:
/// - Mouse clicks (Editor testing)
/// - Public Play() method (HoloLens)
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class DrumPad : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip drumSound;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    [Header("Animation")]
    [SerializeField] private bool animateHit = true;

    // NEW: Reference to the visual drum part
    [SerializeField] private InstrumentPressEffect pressEffect;

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

    /// <summary>
    /// Mouse click support (Unity Editor)
    /// </summary>
    private void OnMouseDown()
    {
        Play();
    }

    /// <summary>
    /// Call this from HoloLens interaction.
    /// </summary>
    public void Play()
    {
        // Play drum sound
        if (drumSound != null)
            audioSource.PlayOneShot(drumSound, volume);

        // Animate the visible drum model
        pressEffect?.Play();

        // Animate the invisible interaction pad
        if (animateHit && !isAnimating)
            StartCoroutine(HitAnimation());
    }

    private IEnumerator HitAnimation()
    {
        isAnimating = true;

        Vector3 pressedPosition =
            originalPosition + Vector3.down * pressDistance;

        while (Vector3.Distance(transform.localPosition, pressedPosition) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                pressedPosition,
                Time.deltaTime * animationSpeed);

            yield return null;
        }

        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalPosition,
                Time.deltaTime * animationSpeed);

            yield return null;
        }

        transform.localPosition = originalPosition;

        isAnimating = false;
    }
}