using PlatformerController2D.Runtime.Scripts.Systems;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class AxeAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer axeSprite;
    [SerializeField] private Collider2D axeCollider;

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private int attackDamage = 20;

    [Header("Input")]
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private Animator animator;
    



    public bool isAttacking;

    private void Awake()
    {

        axeSprite.enabled = false;
        axeCollider.enabled = false;
    }

    private void OnEnable()
    {
        if (attackAction != null)
            attackAction.action.Enable();
    }

    private void OnDisable()
    {
        if (attackAction != null)
            attackAction.action.Disable();
    }

    private void Update()
    {
        if (attackAction != null &&
            attackAction.action.WasPressedThisFrame() &&
            !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        animator.Play("Attack");
        isAttacking = true;
        Debug.Log("attack start");

        //axeSprite.enabled = true;
        axeCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);
        
        axeCollider.enabled = false;
        //axeSprite.enabled = false;
        Debug.Log("attack end");
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (isAttacking &&
            other.CompareTag("Enemy") &&
            !other.isTrigger && enemyHealth != null)
        {
            
            enemyHealth.TakeDamage(attackDamage);

            Debug.Log("Enemy hit");
        }
    }
}