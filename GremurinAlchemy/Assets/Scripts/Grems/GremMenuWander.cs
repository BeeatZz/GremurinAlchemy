using UnityEngine;
using System.Collections;

public class GremMenuWander : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;
    public float wanderRadius = 2f;
    public float minPauseTime = 0.5f;
    public float maxPauseTime = 2f;
    public float padding = 0.3f;

    [Header("Entry Settings")]
    public float spawnOffset = 2f;   
    public float entrySpeedMultiplier = 1.2f;

    [HideInInspector] public GremDatabaseSO database; 

    private Vector3 targetPosition;
    private bool isPaused = false;
    private float pauseTimer = 0f;
    private float currentPauseDuration;
    private bool isClicked = false;
    private bool isDragging = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private GremData gremData;

    private Vector2 minBounds;
    private Vector2 maxBounds;

    private enum State { Entering, Wandering }
    private State currentState = State.Entering;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            animator = GetComponent<Animator>();

        if (gremData != null)
        {
            SetupBounds();
            SetSpawnPosition();
            PickEntryTarget();
        }

        SetupBounds();
        SetSpawnPosition(); 
        PickEntryTarget();
    }

    private void Update()
    {
        if (isClicked || isDragging)
        {
            SetIdleAnimation();
            return;
        }

        switch (currentState)
        {
            case State.Entering:
                HandleEntry();
                break;
            case State.Wandering:
                HandleWander();
                break;
        }
    }

    private void HandleEntry()
    {
        Vector3 dir = targetPosition - transform.position;
        float distance = dir.magnitude;

        if (distance <= 0.05f)
        {
            currentState = State.Wandering;
            StartPause();
            return;
        }

        transform.position += dir.normalized * moveSpeed * entrySpeedMultiplier * Time.deltaTime;

        if (dir.x > 0.01f) spriteRenderer.flipX = true;
        else if (dir.x < -0.01f) spriteRenderer.flipX = false;

        SetWalkAnimation();
    }

    private void HandleWander()
    {
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

            if (dir.x > 0.01f) spriteRenderer.flipX = true;
            else if (dir.x < -0.01f) spriteRenderer.flipX = false;

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

    private void PickEntryTarget()
    {
        targetPosition = new Vector3(
            Random.Range(minBounds.x, maxBounds.x),
            Random.Range(minBounds.y, maxBounds.y),
            transform.position.z
        );
    }

    private void SetSpawnPosition()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float zDistance = transform.position.z - cam.transform.position.z;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        int side = Random.Range(0, 4);
        Vector3 spawnPos = Vector3.zero;

        switch (side)
        {
            case 0: // Left
                spawnPos = new Vector3(bottomLeft.x - spawnOffset, Random.Range(bottomLeft.y, topRight.y), 0);
                break;
            case 1: // Right
                spawnPos = new Vector3(topRight.x + spawnOffset, Random.Range(bottomLeft.y, topRight.y), 0);
                break;
            case 2: // Bottom
                spawnPos = new Vector3(Random.Range(bottomLeft.x, topRight.x), bottomLeft.y - spawnOffset, 0);
                break;
            case 3: // Top
                spawnPos = new Vector3(Random.Range(bottomLeft.x, topRight.x), topRight.y + spawnOffset, 0);
                break;
        }

        transform.position = spawnPos;
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

    private void SetupBounds()
    {
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
    }

    // ---- Initialize for your current spawner ----
    public void Initialize(string id)
    {
        database = database ?? null; // spawner will assign database before calling
        // Normally the Grem base class handles id -> gremData mapping
        if (database == null)
        {
            Debug.LogWarning("Database not set in Menu Grem, but continuing...");
        }

        // Set default config for menu grems
        moveSpeed = 1.5f;
        wanderRadius = 2f;
        minPauseTime = 0.5f;
        maxPauseTime = 2f;
        padding = 0.3f;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer != null && gremData != null && gremData.defaultSprite != null)
            spriteRenderer.sprite = gremData.defaultSprite;

        if (spriteRenderer != null && gremData != null)
            spriteRenderer.color = gremData.tint;

        if (animator != null && gremData != null)
        {
            var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

            if (gremData != null && gremData.idleAnimation != null)
                overrideController["idleTest"] = gremData.idleAnimation;
            if (gremData != null && gremData.walkAnimation != null)
                overrideController["runTest"] = gremData.walkAnimation;

            animator.runtimeAnimatorController = overrideController;
        }

        SetupBounds();
        SetSpawnPosition();
        PickEntryTarget();
    }

    private void OnMouseDown()
    {
        if (animator != null && gremData != null && gremData.clickAnimation != null)
        {
            animator.Play(gremData.clickAnimation.name);
            isClicked = true;
            SetIdleAnimation();

            StartCoroutine(ResumeAfterClick(gremData.clickAnimation.length));
        }

        if (gremData != null && gremData.clickSound != null)
        {
            AudioSource.PlayClipAtPoint(gremData.clickSound, transform.position);
        }
    }

    private IEnumerator ResumeAfterClick(float duration)
    {
        yield return new WaitForSeconds(duration);
        isClicked = false;
    }

    public void SetDragging(bool dragging)
    {
        isDragging = dragging;
    }
}
