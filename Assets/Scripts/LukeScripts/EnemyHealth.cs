using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Enemy")]
    [SerializeField] private SpriteRenderer enemySprite;
    private Color originalColour;

    void Start()
    {
        currentHealth = maxHealth;
        originalColour = enemySprite.color;
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

        Destroy(gameObject);
    }
}