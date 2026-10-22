
using UnityEngine;
using UnityEngine.UI;
using PlatformerController2D.Runtime.Scripts.Player;

public class OxygenSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private PlayerController player;

    [Header("Oxygen Settings")]
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float walkingDrain = 2f;
    [SerializeField] private float runningDrain = 6f;
    [SerializeField] private float crouchingDrain = 1f;
    [SerializeField] private float jumpingDrain = 4f;
    [SerializeField] private float dashingDrain = 10f;

    private float currentOxygen;

    private void Start()
    {
        currentOxygen = maxOxygen;

        if (oxygenSlider != null)
        {
            oxygenSlider.minValue = 0f;
            oxygenSlider.maxValue = maxOxygen;
            oxygenSlider.value = currentOxygen;
        }
    }

    private void Update()
    {
        if (oxygenSlider == null || player == null)
            return;

        if (currentOxygen <= 0f)
            return;

        float drainRate = 0f;

        if (player.IsDashing)
        {
            drainRate = dashingDrain;
        }
        else if (!player.IsGrounded)
        {
            drainRate = jumpingDrain;
        }
        else if (player.IsCrouching)
        {
            drainRate = crouchingDrain;
        }
        else if (player.IsMoving && player.IsRunning)
        {
            drainRate = runningDrain;
        }
        else if (player.IsMoving)
        {
            drainRate = walkingDrain;
        }

        currentOxygen -= drainRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);

        oxygenSlider.value = currentOxygen;
    }
}
