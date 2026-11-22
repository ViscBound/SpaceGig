using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public GameObject pointA;
    public GameObject pointB;
    public float speed = 2f;

    // Furthest the enemy can chase
    [Header("Chase Boundaries")]
    public GameObject pointC; 
    public GameObject pointD;

    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public float attackWindup = 0.7f;
    public float hitboxDuration = 0.2f;
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

    public bool TouchingPlayer;

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

        TouchingPlayer = false;
            
    }

    void Update()
    {
        if (chasingPlayer && player != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            float enemyX = transform.position.x;

            // Check if enemy has reached its chase boundaries
            if (enemyX <= pointC.transform.position.x &&
                player.position.x < enemyX)
            {
                StopChasing();
            }
            else if (enemyX >= pointD.transform.position.x &&
                    player.position.x > enemyX)
            {
                StopChasing();
            }
            else if (distance <= attackRange)
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
                float direction = Mathf.Sign(
                    player.position.x - transform.position.x
                );

                if (!TouchingPlayer)
                {
                    rb.linearVelocity = new Vector2(direction * speed, 0f);
                }

                UpdateFacing();
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else if (!isAttacking)
        {
            // Patrolling
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
            if (!TouchingPlayer)
            {
                rb.linearVelocity = new Vector2(direction * speed, 0f);
            }

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

        FacePlayer();

        animator.SetBool("Moving", false);

        // TEMP: UNTIL ATTACK ANIM IS MADE
        if (hitboxScript != null)
            hitboxScript.ShowWarning();

        Invoke(nameof(EnableAttackHitbox), attackWindup);

        Invoke(nameof(AttackFinished),
        attackWindup + hitboxDuration);
        // END TEMP

        // animator.SetTrigger("Attack");
    }

    public void EnableAttackHitbox()
    {
        if (hitboxScript != null)
            hitboxScript.EnableHitbox();
    }

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

    void StopChasing()
    {
        chasingPlayer = false;
        player = null;

        rb.linearVelocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null)
            Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);

        if (pointB != null)
            Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        
        if (pointC != null)
            Gizmos.DrawWireSphere(pointC.transform.position, 0.5f);

        if (pointD != null)
            Gizmos.DrawWireSphere(pointD.transform.position, 0.5f);
    }
    //hi i darragh added this to freeze enemy when touching player 
    //just added a bool to any instances of updating the velocity to disable them if bool is true
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player touched");
            TouchingPlayer = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TouchingPlayer = false;
        }
    }
}