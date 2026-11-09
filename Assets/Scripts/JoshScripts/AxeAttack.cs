
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class AxeAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer axeSprite;
    [SerializeField] private Collider2D axeCollider;

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.2f;

    [Header("Input")]
    [SerializeField] private InputActionReference attackAction;

    private bool isAttacking;

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
        isAttacking = true;

        axeSprite.enabled = true;
        axeCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        axeCollider.enabled = false;
        axeSprite.enabled = false;

        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking &&
            other.CompareTag("Enemy") &&
            !other.isTrigger)
        {
            Debug.Log("Enemy hit");
        }
    }
}
