using UnityEngine;

public class Ball_Controller : MonoBehaviour
{
    public float speed = 8f;              // Starting speed
    public float speedIncrease = 0.5f;    // Speed gain per paddle hit
    public float maxSpeed = 15f;          // Cap speed
    public float minYVelocity = 0.5f;     // Prevents horizontal "sticking"

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchBall();
    }

    void LaunchBall()
    {
        float xDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        float yDirection = Random.Range(-0.5f, 0.5f);
        Vector2 direction = new Vector2(xDirection, yDirection).normalized;
        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Audio_Manager.Instance.PlayBounce();
        Vector2 vel = rb.velocity;

        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Increase speed
            speed = Mathf.Min(speed + speedIncrease, maxSpeed);

            // Where on the paddle did we hit? (normalized -1 to 1)
            float hitPos = (transform.position.y - collision.transform.position.y) / (collision.collider.bounds.size.y / 2);

            // Clamp hitPos so small errors don't cause crazy angles
            hitPos = Mathf.Clamp(hitPos, -1f, 1f);

            // Max deflection from horizontal (e.g. 45 degrees)
            float maxDeflection = 45f;

            // Calculate new angle
            float bounceAngle = hitPos * maxDeflection;
            float directionX = Mathf.Sign(vel.x); // Keep moving toward opponent

            // Convert angle to velocity
            vel.x = Mathf.Cos(bounceAngle * Mathf.Deg2Rad) * directionX;
            vel.y = Mathf.Sin(bounceAngle * Mathf.Deg2Rad);

            vel = vel.normalized * speed;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            vel = vel.normalized * speed;
        }
        vel = EnforceAngleLimits(vel, 15f, 50f);
        vel = vel.normalized * speed;
        rb.velocity = vel;
    }

    Vector2 EnforceAngleLimits(Vector2 velocity, float minAngleDeg, float maxAngleDeg)
{
    float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
    float absAngle = Mathf.Abs(angle);

    // If too flat (close to horizontal)
    if (absAngle < minAngleDeg)
    {
        float signY = Mathf.Sign(velocity.y == 0 ? Random.Range(-1f, 1f) : velocity.y);
        velocity.y = Mathf.Tan(minAngleDeg * Mathf.Deg2Rad) * Mathf.Abs(velocity.x) * signY;
    }
    // If too steep (close to vertical)
    else if (absAngle > maxAngleDeg)
    {
        float signX = Mathf.Sign(velocity.x == 0 ? Random.Range(-1f, 1f) : velocity.x);
        velocity.x = Mathf.Abs(velocity.y) / Mathf.Tan(maxAngleDeg * Mathf.Deg2Rad) * signX;
    }

    return velocity;
}

    public void ResetBall()
    {
        // Reset speed on new round
        speed = 8f;

        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        Invoke(nameof(LaunchBall), 1f);
    }
}
