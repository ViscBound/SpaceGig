using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 10;
    public float lifetime = 5f;

    private Rigidbody2D rb;
    private Vector2 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 launchDirection)
    {
        direction = launchDirection.normalized;
        rb.linearVelocity = direction * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player damage here when ready
            Destroy(gameObject);
        }
    }
}