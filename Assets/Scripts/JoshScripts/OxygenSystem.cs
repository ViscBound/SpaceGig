using UnityEngine;
using UnityEngine.UI;

public class OxygenSystem : MonoBehaviour
{
    public Slider oxygenSlider;
    public float maxOxygen = 100f;
    public float oxygenDrainRate = 5f;

    public Transform player;

    private Vector3 lastPosition;

    void Start()
    {
        oxygenSlider.maxValue = maxOxygen;
        oxygenSlider.value = maxOxygen;

        lastPosition = player.position;
    }

    void Update()
    {
        // Check whether the player is moving
        bool isMoving = Vector3.Distance(player.position, lastPosition) > 0.001f;

        if (isMoving && oxygenSlider.value > 0)
        {
            oxygenSlider.value -= oxygenDrainRate * Time.deltaTime;
        }

        lastPosition = player.position;
    }
}
