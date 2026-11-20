using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    [Header("Attack Settings")]
    public int damage = 15;

    private bool flippedSpriteFacesRight = true;

    private Collider2D hitboxCollider;
    private SpriteRenderer enemySprite;

    private Vector2 originalOffset;
    private bool hasHitPlayer;

    void Awake()
    {
        hitboxCollider = GetComponent<Collider2D>();
        enemySprite = GetComponentInParent<SpriteRenderer>();

        originalOffset = hitboxCollider.offset;

        hitboxCollider.enabled = false;
    }

    void LateUpdate()
    {
        if (enemySprite == null)
            return;

        // Determine enemy facing direction
        bool facingRight =
            enemySprite.flipX == flippedSpriteFacesRight;

        // Flip collider offset around its centre
        float direction = facingRight ? 1f : -1f;

        hitboxCollider.offset = new Vector2(
            Mathf.Abs(originalOffset.x) * direction,
            originalOffset.y
        );
    }

    public void EnableHitbox()
    {
        hasHitPlayer = false;
        hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitPlayer)
            return;

        if (other.CompareTag("Player"))
        {
            OxygenSystem oxygen =
                other.GetComponentInParent<OxygenSystem>();

            if (oxygen != null)
            {
                oxygen.TakeDamage(damage);
                hasHitPlayer = true;

                Debug.Log("Enemy dealt " + damage + " damage!");
            }
        }
    }
}