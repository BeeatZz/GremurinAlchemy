using UnityEngine;

public class GremWander : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;
    public float wanderRadius = 2f;
    public float minPauseTime = 0.5f;
    public float maxPauseTime = 2f;
    public float padding = 0.3f;

    private Vector3 targetPosition;
    private bool isPaused = false;
    private float pauseTimer = 0f;
    private float currentPauseDuration;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector2 minBounds;
    private Vector2 maxBounds;


    private void Start()
    {
        PickNewTarget();
    }
    private void Update()
    {
        if (targetPosition == Vector3.zero) return; 

        if (isPaused)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= currentPauseDuration)
            {
                isPaused = false;
                PickNewTarget();
            }

            SetIdleAnimation();
            return;
        }

        Vector3 dir = targetPosition - transform.position;
        float distance = dir.magnitude;

        if (distance <= 0.05f)
        {
            StartPause();
        }
        else
        {
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;

            if (dir.x > 0.01f)
                spriteRenderer.flipX = true;
            else if (dir.x < -0.01f)
                spriteRenderer.flipX = false;

            SetWalkAnimation();
        }
    }

    private void PickNewTarget()
    {
        Vector3 newTarget = transform.position;
        int attempts = 0;
        const int maxAttempts = 10;

        while (attempts < maxAttempts)
        {
            Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
            newTarget = transform.position + (Vector3)randomOffset;

            newTarget.x = Mathf.Clamp(newTarget.x, minBounds.x, maxBounds.x);
            newTarget.y = Mathf.Clamp(newTarget.y, minBounds.y, maxBounds.y);

            if (Vector3.Distance(newTarget, transform.position) >= wanderRadius * 0.3f)
                break;

            attempts++;
        }

        targetPosition = newTarget;
    }


    private void StartPause()
    {
        isPaused = true;
        pauseTimer = 0f;
        currentPauseDuration = Random.Range(minPauseTime, maxPauseTime);
        SetIdleAnimation();
    }

    private void SetIdleAnimation()
    {
        if (animator != null)
            animator.SetBool("IsWalking", false);
    }

    private void SetWalkAnimation()
    {
        if (animator != null)
            animator.SetBool("IsWalking", true);
    }

    public void Initialize(BehaviorConfig config)
    {
        moveSpeed = config.moveSpeed;
        wanderRadius = config.wanderRadius;
        minPauseTime = config.minPauseTime;
        maxPauseTime = config.maxPauseTime;
        padding = config.padding;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            animator = GetComponent<Animator>();

        Camera cam = Camera.main;
        if (cam == null) return;

        float zDistance = transform.position.z - cam.transform.position.z;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        Vector2 spriteSize = Vector2.zero;
        if (spriteRenderer != null && spriteRenderer.sprite != null)
            spriteSize = spriteRenderer.sprite.bounds.size;

        minBounds = new Vector2(bottomLeft.x + padding + spriteSize.x / 2f,
                                bottomLeft.y + padding + spriteSize.y / 2f);
        maxBounds = new Vector2(topRight.x - padding - spriteSize.x / 2f,
                                topRight.y - padding - spriteSize.y / 2f);

        PickNewTarget();
    }
}
