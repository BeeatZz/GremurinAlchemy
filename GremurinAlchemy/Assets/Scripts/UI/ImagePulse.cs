using UnityEngine;

public class ImagePulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    [Tooltip("Maximum scale factor relative to original scale.")]
    public float maxScale = 1.2f;
    [Tooltip("Minimum scale factor relative to original scale.")]
    public float minScale = 0.8f;
    [Tooltip("Speed of the pulsing effect.")]
    public float pulseSpeed = 2f;

    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scaleFactor = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        transform.localScale = originalScale * scaleFactor;
    }
}
