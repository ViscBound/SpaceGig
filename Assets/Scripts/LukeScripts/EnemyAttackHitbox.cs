using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    [Header("Attack Settings")]
    public int damage = 15;
    public SpriteRenderer attackVisual;
    private EnemyPatrol parent;
    private bool stop;
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

        // Hide visual
        if (attackVisual != null)
            attackVisual.enabled = false;
    }

    void LateUpdate()
    {
        if (enemySprite == null)
            return;

        bool facingRight =
            enemySprite.flipX == flippedSpriteFacesRight;

        float direction = facingRight ? 1f : -1f;

        hitboxCollider.offset = new Vector2(
            Mathf.Abs(originalOffset.x) * direction,
            originalOffset.y
        );

        // Align with collider
        if (attackVisual != null)
        {
            attackVisual.transform.localPosition =
                hitboxCollider.offset;

            attackVisual.transform.localScale =
                hitboxCollider.bounds.size;
        }
    }

    // White square appears
    public void ShowWarning()
    {
        if (attackVisual != null)
        {
            attackVisual.color = Color.white;
            attackVisual.enabled = true;

            parent = gameObject.transform.parent.gameObject.GetComponent<EnemyPatrol>();
            stop = true;
            parent.TouchingPlayer = stop;
        }
    }

    // Square turns red
    public void EnableHitbox()
    {
        hasHitPlayer = false;

        if (attackVisual != null)
            attackVisual.color = Color.red;

        hitboxCollider.enabled = true;
    }

    // Attack disappears
    public void DisableHitbox()
    {
        hitboxCollider.enabled = false;

        if (attackVisual != null)
            attackVisual.enabled = false;
            stop = false;
            parent.TouchingPlayer = stop;
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