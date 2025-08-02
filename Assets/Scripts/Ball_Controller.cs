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

        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Increase speed
            speed = Mathf.Min(speed + speedIncrease, maxSpeed);

            // Calculate bounce direction
            float y = (transform.position.y - collision.transform.position.y);
            Vector2 dir = new Vector2(rb.velocity.x, y).normalized;

            // Apply new speed
            rb.velocity = dir * speed;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Keep same speed after wall bounces
            rb.velocity = rb.velocity.normalized * speed;
        }

        ClampYVelocity();
    }

    void ClampYVelocity()
    {
        Vector2 velocity = rb.velocity;

        if (Mathf.Abs(velocity.y) < minYVelocity)
        {
            float yDirection = velocity.y == 0 ? 1f : Mathf.Sign(velocity.y);
            velocity.y = minYVelocity * yDirection;
            rb.velocity = velocity.normalized * speed;
        }
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
