using UnityEngine;

public class WFDetection : MonoBehaviour
{
    public WallFlowerController wallFlower;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wallFlower.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wallFlower.SetActive(false);
        }
    }
}