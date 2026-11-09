
using UnityEngine;

public class WallFlowerController : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform playerHead;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float minHeight = -3f;
    public float maxHeight = 3f;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireInterval = 2f;

    private float fireTimer;

    void Update()
    {
        if (playerHead == null)
            return;

        // Move to player's head
        float targetHeight = Mathf.Clamp(
            playerHead.position.y,
            minHeight,
            maxHeight
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

        // Fire projectile
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            FireProjectile();
            fireTimer = 0f;
        }
    }

    void FireProjectile()
    {
        float direction = Mathf.Sign(
            playerHead.position.x - firePoint.position.x
        );

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Projectile projectileScript =
            projectile.GetComponent<Projectile>();

        projectileScript.Launch(new Vector2(direction, 0f));
    }
}
