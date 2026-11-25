using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Enemy")]
    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private WFWall wallToRestore;
    private Color originalColour;
    private Vector3 startPosition;

    [Header("Events")]
    public UnityEvent onDeath;

    void Start()
    {
        currentHealth = maxHealth;
        originalColour = enemySprite.color;
        startPosition = transform.position;
    }

    public void ResetEnemy()
    {
        transform.position = startPosition;
        currentHealth = maxHealth;
        enemySprite.color = originalColour;
        
        if (wallToRestore != null)
            wallToRestore.RestoreWall();

        gameObject.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(DamageFlash());

        Debug.Log(gameObject.name + " took " + damage + " damage. Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageFlash()
    {
        enemySprite.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        enemySprite.color = originalColour;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");

        onDeath?.Invoke();

        gameObject.SetActive(false);
    }
}