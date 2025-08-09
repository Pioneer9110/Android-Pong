using UnityEngine;

public class AI_Paddle : MonoBehaviour
{
    public Transform ball;
    public float baseMaxSpeed = 4f;        // AI speed when ball is slow
    public float speedScaleFactor = 0.5f;  // How much AI speed scales with ball speed
    public float maxAIMaxSpeed = 10f;      // Absolute AI speed cap

    public float baseVerticalOffsetRange = 0.6f; // Base tracking error
    public float baseReactionInterval = 0.4f;    // Base reaction time
    public float smoothTime = 0.15f;
    public float trackingDeadZoneY = 0.25f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCol;
    private float offsetLockTimer = 0f;
    private float targetYOffset = 0f;
    private Vector2 velocity = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        if (ball == null) return;

        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
        bool ballComingAtAI = ballRb.velocity.x > 0f;

        // Get current ball speed
        float ballSpeed = ballRb.velocity.magnitude;

        // Dynamically adjust AI paddle speed
        float dynamicMaxSpeed = Mathf.Min(baseMaxSpeed + (ballSpeed * speedScaleFactor), maxAIMaxSpeed);

        // Scale tracking error with ball speed (miss more at high speeds)
        // Base offset scaling with speed
        float minOffset = 0.05f; // almost perfect at low speeds
        float maxOffset = baseVerticalOffsetRange; 
        float verticalOffsetRange = Mathf.Lerp(minOffset, maxOffset, Mathf.InverseLerp(2f, 5f, ballSpeed));

        // Occasionally mess up even at low speeds
        if (ballSpeed < 5f && Random.value < 0.05f) // 5% chance early on
        {
            verticalOffsetRange += Random.Range(0.3f, 0.6f); // extra sloppiness burst
        }

        // Scale reaction time with ball speed (slower at high speeds)
        float reactionInterval = baseReactionInterval + (ballSpeed * 0.05f);

        Vector2 currentPos = rb.position;

        if (ballComingAtAI)
        {
            offsetLockTimer -= Time.fixedDeltaTime;

            // Change offset less frequently → simulate slower thinking
            if (offsetLockTimer <= 0f)
            {
                targetYOffset = Random.Range(-verticalOffsetRange, verticalOffsetRange);
                offsetLockTimer = reactionInterval;
            }

            float targetY = ball.position.y + targetYOffset;

            // Only move if outside dead zone
            if (Mathf.Abs(currentPos.y - targetY) > trackingDeadZoneY)
            {
                float newY = Mathf.SmoothDamp(currentPos.y, targetY, ref velocity.y, smoothTime, dynamicMaxSpeed);
                Vector2 proposedPos = new Vector2(currentPos.x, newY);

                if (!WouldCollideWithWall(proposedPos))
                {
                    rb.MovePosition(proposedPos);
                }
            }
        }
    }

    bool WouldCollideWithWall(Vector2 position)
    {
        Vector2 size = boxCol.size;
        Vector2 worldSize = Vector2.Scale(size, transform.lossyScale);
        Collider2D hit = Physics2D.OverlapBox(position, worldSize, 0f, LayerMask.GetMask("Wall"));
        return hit != null;
    }
}
