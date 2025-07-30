using UnityEngine;

public class AI_Paddle : MonoBehaviour
{
    public Transform ball;
    public float moveSpeed = 8f;
    public float verticalOffsetRange = 0.6f;
    public float reactionInterval = 0.4f;
    public float smoothTime = 0.15f;
    public float maxSpeed = 10f;
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

        Vector2 currentPos = rb.position;

        if (ballComingAtAI)
        {
            offsetLockTimer -= Time.fixedDeltaTime;

            // Change offset less frequently to avoid twitching
            if (offsetLockTimer <= 0f)
            {
                targetYOffset = Random.Range(-verticalOffsetRange, verticalOffsetRange);
                offsetLockTimer = reactionInterval;
            }

            float targetY = ball.position.y + targetYOffset;

            // Only move if outside dead zone
            if (Mathf.Abs(currentPos.y - targetY) > trackingDeadZoneY)
            {
                float newY = Mathf.SmoothDamp(currentPos.y, targetY, ref velocity.y, smoothTime, maxSpeed);
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
