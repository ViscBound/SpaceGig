using UnityEngine;
using System.Collections.Generic;

public class OxygenSafeZone : MonoBehaviour
{
    [SerializeField] private OxygenSystem playerOxygen;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOxygen.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOxygen.enabled = true;
        }
    }
}
