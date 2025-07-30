using UnityEngine;

public class Ball_Controller : MonoBehaviour
{
    public float speed = 8f;
    public float minYVelocity = 0.5f; // Prevents horizontal "sticking"

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
        rb.velocity = direction.normalized * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Audio_Manager.Instance.PlayBounce();

        if (collision.gameObject.CompareTag("Paddle"))
        {
            float y = (transform.position.y - collision.transform.position.y);
            Vector2 dir = new Vector2(rb.velocity.x, y).normalized;
            rb.velocity = dir * speed;
        }

        ClampYVelocity();
    }

    void ClampYVelocity()
    {
        Vector2 velocity = rb.velocity;

        // Clamp Y velocity to prevent flat horizontal movement
        if (Mathf.Abs(velocity.y) < minYVelocity)
        {
            float yDirection = velocity.y == 0 ? 1f : Mathf.Sign(velocity.y);
            velocity.y = minYVelocity * yDirection;
            rb.velocity = velocity.normalized * speed;
        }
    }

    public void ResetBall()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        Invoke(nameof(LaunchBall), 1f);
    }
}
