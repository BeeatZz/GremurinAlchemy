using UnityEngine;

public class GremDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private GremWander wander;

    [Header("Drag Scale")]
    public float dragScaleMultiplier = 1.2f;
    public float dragScaleSpeed = 6f;

    [Header("Drag Smooth")]
    public float dragSmoothTime = 0.1f;

    [Header("Click Settings")]
    public float clickThreshold = 0.1f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private Vector3 targetPosition;
    private Vector3 mouseDownPos;
    private bool potentialClick = false;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        wander = GetComponent<GremWander>();
        originalScale = transform.localScale;
        targetScale = originalScale;
        targetPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * dragScaleSpeed);

        if (isDragging)
        {
            spriteRenderer.sortingOrder = 10;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / dragSmoothTime);
        }
    }

    void OnMouseDown()
    {
        mouseDownPos = GetMouseWorldPos();
        offset = transform.position - mouseDownPos;
        isDragging = true;
        potentialClick = true;
        wander?.SetDragging(true);

        targetScale = originalScale * dragScaleMultiplier;
        targetPosition = transform.position;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            targetPosition = GetMouseWorldPos() + offset;

            if (Vector3.Distance(mouseDownPos, GetMouseWorldPos()) > clickThreshold)
            {
                potentialClick = false;
            }
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        spriteRenderer.sortingOrder = 0;
        wander?.SetDragging(false);
        targetScale = originalScale;

        if (potentialClick)
        {
            //wander?.OnClick();
        }
        
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
