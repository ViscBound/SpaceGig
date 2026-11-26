using UnityEngine;

public class WallFlowerController : MonoBehaviour
{
    public enum FiringDirection
    {
        Left,
        Right
    }

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
    public FiringDirection firingDirection = FiringDirection.Left;

    [Header("Animation Settings")]
    public Animator animator;

    private float fireTimer;
    private bool isActive = false;
    private bool isShooting = false;

    void Update()
    {
        if (!isActive || isShooting)
            return;

        if (playerHead == null || pointA == null || pointB == null)
            return;

        // Moves to player's head
        float targetHeight = playerHead.position.y;

        targetHeight = Mathf.Clamp(
            targetHeight,
            Mathf.Min(pointA.position.y, pointB.position.y),
            Mathf.Max(pointA.position.y, pointB.position.y)
        );

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
            fireTimer = 0f;
            StartShooting();
        }
    }

    void StartShooting()
    {
        isShooting = true;

        if (animator != null)
            animator.SetTrigger("Shoot");
    }

    // Animation Event to fire projectile on certain frame
    public void FireProjectile()
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Projectile projectileScript =
            projectile.GetComponent<Projectile>();

        Vector2 direction = firingDirection == FiringDirection.Left
            ? Vector2.left
            : Vector2.right;

        projectileScript.Launch(direction);
    }

    // Animation Event to stop firing on certain frame
    public void FinishShooting()
    {
        isShooting = false;
    }

    public void SetActive(bool active)
    {
        isActive = active;

        if (!isActive)
            fireTimer = 0f;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null)
            Gizmos.DrawWireSphere(pointA.position, 0.5f);

        if (pointB != null)
            Gizmos.DrawWireSphere(pointB.position, 0.5f);

        if (pointA != null && pointB != null)
            Gizmos.DrawLine(pointA.position, pointB.position);
    }
}