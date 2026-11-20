using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public GameObject pointA;
    public GameObject pointB;
    public float speed = 2f;

    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public Collider2D attackHitbox;
    public EnemyAttackHitbox hitboxScript;

    private Rigidbody2D rb;
    private Transform currentPoint;
    private Transform player;

    private SpriteRenderer sprite;
    private Animator animator;

    private bool Orientation;
    private bool chasingPlayer = false;
    private bool isAttacking = false;
    private float nextAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentPoint = pointB.transform;

        if (attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }
            
    }

    void Update()
    {
        if (chasingPlayer && player != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            // Player is close enough to attack
            if (distance <= attackRange)
            {
                rb.linearVelocity = Vector2.zero;

                FacePlayer();

                animator.SetBool("Moving", false);

                if (!isAttacking && Time.time >= nextAttackTime)
                {
                    StartAttack();
                }
            }
            else if (!isAttacking)
            {
                // Chase player
                float direction = Mathf.Sign(
                    player.position.x - transform.position.x
                );

                rb.linearVelocity = new Vector2(direction * speed, 0f);

                UpdateFacing();
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else if (!isAttacking)
        {
            // Original patrol behaviour
            float enemyX = transform.position.x;
            float pointAX = pointA.transform.position.x;
            float pointBX = pointB.transform.position.x;

            if (currentPoint == pointB.transform && enemyX >= pointBX)
            {
                currentPoint = pointA.transform;
            }
            else if (currentPoint == pointA.transform && enemyX <= pointAX)
            {
                currentPoint = pointB.transform;
            }

            float direction = Mathf.Sign(
                currentPoint.position.x - enemyX
            );

            rb.linearVelocity = new Vector2(direction * speed, 0f);

            UpdateFacing();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void UpdateFacing()
    {
        if (rb.linearVelocityX < 0f)
        {
            animator.SetBool("Moving", true);
            Orientation = false;
            sprite.flipX = Orientation;
        }
        else if (rb.linearVelocityX > 0f)
        {
            animator.SetBool("Moving", true);
            Orientation = true;
            sprite.flipX = Orientation;
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    void FacePlayer()
    {
        if (player.position.x < transform.position.x)
        {
            Orientation = false;
        }
        else
        {
            Orientation = true;
        }

        sprite.flipX = Orientation;
    }

    void StartAttack()
    {
        isAttacking = true;

        nextAttackTime = Time.time + attackCooldown;

        rb.linearVelocity = Vector2.zero;

        animator.SetBool("Moving", false);

        // TEMP: UNTIL ATTACK ANIM IS MADE
        if (hitboxScript != null)
            hitboxScript.EnableHitbox();

        Invoke(nameof(AttackFinished), 0.5f);
        // END TEMP

        // animator.SetTrigger("Attack");
    }

    // public void EnableAttackHitbox()
    // {
    //     if (hitboxScript != null)
    //         hitboxScript.EnableHitbox();
    // }

    // public void DisableAttackHitbox()
    // {
    //     if (hitboxScript != null)
    //         hitboxScript.DisableHitbox();
    // }

    public void AttackFinished()
    {
        // TEMP
        if (hitboxScript != null)
            hitboxScript.DisableHitbox();
        // END TEMP
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDetection"))
        {
            player = other.transform.root;
            chasingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDetection"))
        {
            chasingPlayer = false;
            player = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA != null)
            Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);

        if (pointB != null)
            Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
    }
}