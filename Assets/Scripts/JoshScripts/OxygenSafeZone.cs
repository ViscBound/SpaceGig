using UnityEngine;
using System.Collections.Generic;

public class OxygenSafeZone : MonoBehaviour
{
    [SerializeField] private OxygenSystem playerOxygen;
    [SerializeField] private float refillRate = 10f;
    private bool isPlayerInside;

    void Update()
    {
        if ((isPlayerInside) && (playerOxygen.currentOxygen < playerOxygen.maxOxygen))
            playerOxygen.RefillOxygen(refillRate * Time.deltaTime);
    }    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOxygen.canDrainOxygen = false;
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOxygen.canDrainOxygen = true;
            isPlayerInside = false;
        }
    }
}
