using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ButtonClickEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Click Settings")]
    [Tooltip("Factor")]
    public float clickScale = 0.9f;
    [Tooltip("Time")]
    public float transitionDuration = 0.1f;

    [Header("Sound")]
    [Tooltip("Soundclip")]
    public AudioClip clickSound;
    [Range(0f, 1f)] public float clickVolume = 1f;

    private Vector3 originalScale;
    private Coroutine clickCoroutine;
    private AudioSource audioSource;

    void Awake()
    {
        originalScale = transform.localScale;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickSound != null && audioSource != null)
            audioSource.PlayOneShot(clickSound, clickVolume);

        if (clickCoroutine != null)
            StopCoroutine(clickCoroutine);
        clickCoroutine = StartCoroutine(AnimateScale(transform.localScale, originalScale * clickScale));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (clickCoroutine != null)
            StopCoroutine(clickCoroutine);
        clickCoroutine = StartCoroutine(AnimateScale(transform.localScale, originalScale));
    }

    private IEnumerator AnimateScale(Vector3 fromScale, Vector3 toScale)
    {
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            transform.localScale = Vector3.Lerp(fromScale, toScale, t);
            yield return null;
        }

        transform.localScale = toScale;
    }
}
