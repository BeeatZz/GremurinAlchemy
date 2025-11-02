using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    [Tooltip("Factor")]
    public float hoverScale = 1.2f;
    [Tooltip("Time")]
    public float transitionDuration = 0.2f;

    [Header("Rotation Settings")]
    [Tooltip("degrees")]
    public float hoverRotation = 10f;

    [Header("Sprite Settings")]
    [Tooltip("Sprite to show when hovered.")]
    public Sprite hoverSprite;

    [Header("Sound Settings")]
    [Tooltip("Soundclip")]
    public AudioClip hoverSound;
    [Tooltip("Volume")]
    [Range(0f, 1f)] public float hoverVolume = 1f;

    private Sprite originalSprite;
    private Image buttonImage;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    private Coroutine transitionCoroutine;
    private AudioSource audioSource;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
            originalSprite = buttonImage.sprite;

        originalScale = transform.localScale;
        originalRotation = transform.localRotation;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonImage != null && hoverSprite != null)
            buttonImage.sprite = hoverSprite;

        if (hoverSound != null && audioSource != null)
            audioSource.PlayOneShot(hoverSound, hoverVolume);

        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(AnimateTransform(originalScale * hoverScale, Quaternion.Euler(0f, 0f, hoverRotation)));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
            buttonImage.sprite = originalSprite;

        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(AnimateTransform(originalScale, originalRotation));
    }

    private IEnumerator AnimateTransform(Vector3 targetScale, Quaternion targetRotation)
    {
        Vector3 startScale = transform.localScale;
        Quaternion startRotation = transform.localRotation;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.localScale = targetScale;
        transform.localRotation = targetRotation;
    }
}
