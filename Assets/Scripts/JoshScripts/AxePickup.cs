using UnityEngine;

public class AxePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           AxeAttack axe = other.GetComponentInParent<AxeAttack>();

           if (axe != null)
           {
            axe.enabled = true;
            Debug.Log("Axe collected");
            Destroy(gameObject);
           }
        }
    }
}
