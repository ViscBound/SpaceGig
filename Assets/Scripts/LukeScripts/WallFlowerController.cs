using UnityEngine;

public class WallFlowerController : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform playerHead;

    [Header("Movement Boundaries")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 3f;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireInterval = 2f;

    private float fireTimer;

    void Update()
    {
        if (playerHead == null || pointA == null || pointB == null)
            return;

        // Player's head
        float targetHeight = playerHead.position.y;

        // Can only move between Point A and Point B
        targetHeight = Mathf.Clamp(
            targetHeight,
            Mathf.Min(pointA.position.y, pointB.position.y),
            Mathf.Max(pointA.position.y, pointB.position.y)
        );

        // Move to player's head
        Vector3 targetPosition = new Vector3(
            transform.position.x,
            targetHeight,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Fire
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            FireProjectile();
            fireTimer = 0f;
        }
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Projectile projectileScript =
            projectile.GetComponent<Projectile>();

        // Always shoot left
        projectileScript.Launch(Vector2.left);
    }

    private void OnDrawGizmos()
    {
        if (pointA != null)
            Gizmos.DrawWireSphere(pointA.position, 0.5f);

        if (pointB != null)
            Gizmos.DrawWireSphere(pointB.position, 0.5f);

        if (pointA != null && pointB != null)
        {
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}