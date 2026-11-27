using UnityEngine;

public class AxePickup : MonoBehaviour
{
    public GameObject axeControls;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           AxeAttack axe = other.GetComponentInParent<AxeAttack>();

           if (axe != null)
           {
            axe.enabled = true;
            axeControls.SetActive(true);
            Debug.Log("Axe collected");
            Destroy(gameObject);
            StaticVars.instance.AxeFlag = true;
           }
        }
    }
}
