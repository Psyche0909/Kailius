using UnityEngine;

public class BoundaryWall : MonoBehaviour
{
    private BoxCollider2D wallCollider;

    void Start()
    {
        wallCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector2 playerPos = rb.position;
        Vector2 wallPos = (Vector2)transform.position;
        Vector2 wallSize = wallCollider != null ? wallCollider.size : Vector2.one;

        float halfW = wallSize.x / 2f;
        float halfH = wallSize.y / 2f;

        float dx = playerPos.x - wallPos.x;
        float dy = playerPos.y - wallPos.y;

        float overlapX = halfW - Mathf.Abs(dx);
        float overlapY = halfH - Mathf.Abs(dy);

        if (overlapX <= overlapY)
        {
            float targetX = wallPos.x + Mathf.Sign(dx) * (halfW + 0.05f);
            rb.position = new Vector2(targetX, playerPos.y);
        }
        else
        {
            float targetY = wallPos.y + Mathf.Sign(dy) * (halfH + 0.05f);
            rb.position = new Vector2(playerPos.x, targetY);
        }
    }
}
