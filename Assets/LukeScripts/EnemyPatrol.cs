
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed = 2f;

    private Rigidbody2D rb;
    private Transform currentPoint;
    private Transform player;

    private bool chasingPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }


    void Update()
    {
        if (chasingPlayer && player != null)
        {
            // Chasing player horizontally
            float direction = Mathf.Sign(
                player.position.x - transform.position.x
            );

            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }
        else
        {
            // Patrolling point A and point B
            float enemyX = transform.position.x;
            float pointAX = pointA.transform.position.x;
            float pointBX = pointB.transform.position.x;

            // Target reached or passed, turn around
            if (currentPoint == pointB.transform && enemyX >= pointBX)
            {
                currentPoint = pointA.transform;
            }
            else if (currentPoint == pointA.transform && enemyX <= pointAX)
            {
                currentPoint = pointB.transform;
            }

            // Move towards the current patrol target
            float direction = Mathf.Sign(
                currentPoint.position.x - enemyX
            );

            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }
    }

    // Enter circle collider to have enemy follow you
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            chasingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
