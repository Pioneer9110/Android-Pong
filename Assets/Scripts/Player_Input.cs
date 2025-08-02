using UnityEngine;

public class Player_Input : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isPlayer = true;

    private Camera cam;
    private Vector3 touchPosition;

    private BoxCollider2D boxCol;

    void Start()
    {
        cam = Camera.main;
        boxCol = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!isPlayer) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = cam.ScreenToWorldPoint(touch.position);
            Vector3 targetPos = new Vector3(transform.position.x, touchPosition.y, 0);

            // Smooth movement
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Check for collision using OverlapBox at the new position
            if (!WouldCollideWithWall(newPosition))
            {
                transform.position = newPosition;
            }
        }
    }

    bool WouldCollideWithWall(Vector3 position)
    {
        // Use the paddle's box collider size
        Vector2 size = boxCol.size;
        Vector2 worldSize = Vector2.Scale(size, transform.lossyScale);

        // Cast an imaginary box at the next position
        Collider2D hit = Physics2D.OverlapBox(position, worldSize, 0f, LayerMask.GetMask("Wall"));

        return hit != null;
    }
}
