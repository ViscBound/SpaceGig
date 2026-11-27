using UnityEngine;
using UnityEngine.UI;
using PlatformerController2D.Runtime.Scripts.Player;
using PlatformerController2D.Runtime.Scripts.StateMachine;
using PlatformerController2D.Runtime.Scripts.StateMachine.PlayerStateMachine.PlayerState;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OxygenSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private Image oxygenFill;
    [SerializeField] private PlayerController player;

    [Header("Oxygen Settings")]
    public int maxOxygen = 100;
    [SerializeField] private int walkingDrain = 2;
    [SerializeField] private int runningDrain = 6;
    [SerializeField] private int jumpingDrain = 4;
    [SerializeField] private int dashingDrain = 10;

    [HideInInspector] public float currentOxygen;
    [SerializeField] private int oxygenDisplay;
    public bool canDrainOxygen;
    private IState previousState;
    private float oxygenColour;

    [Header("Oxygen Colours")]
    [SerializeField] private float yellowOxygen = 0.5f;
    [SerializeField] private float redOxygen = 0.25f;

    [SerializeField] private Color greenColour;
    [SerializeField] private Color yellowColor;
    [SerializeField] private Color redColor;

    [Header("Vignette Settings")]
    [SerializeField] private Volume globalVolume;
    [SerializeField, Range(0f, 1f)] private float maxVignetteIntensity = 0.45f;
    [SerializeField, Range(0f, 0.5f)] private float vignetteFadeOutOxygen = 0.05f;

    private Vignette vignette;

    private void Start()
    {
        currentOxygen = maxOxygen;

        if (globalVolume != null && globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = 0f;
        }
        else
        {
            Debug.LogWarning("GV or Vignette is missing!");
        }

        UpdateOxygenUI();
    }

    private void Update()
    {
        if (player == null || currentOxygen <= 0f || !canDrainOxygen)
            return;

        float drainRate = 0f;

        if (player.CurrentState is RunState)
            drainRate = runningDrain;
        
        else if (player.CurrentState is LocomotionState)
            drainRate = walkingDrain;
        
        else if ((player.CurrentState is JumpState || player.CurrentState is RunJumpState) && player.CurrentState != previousState)
            currentOxygen -= jumpingDrain;

        else if (player.CurrentState is DashState && player.CurrentState != previousState)
            currentOxygen -= dashingDrain;

        currentOxygen -= drainRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
        previousState = player.CurrentState;

        UpdateOxygenUI();
    }

    // Enemies and projectile dammage
    public void TakeDamage(float damage)
    {
        currentOxygen -= damage;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);

        UpdateOxygenUI();

        Debug.Log("Player took " + damage + " damage! Oxygen: " + currentOxygen);
    }

    public void RefillOxygen(float amount)
    {
        currentOxygen += amount;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
        UpdateOxygenUI();
    }

    public void UpdateOxygenColour()
    {
        oxygenColour = currentOxygen / maxOxygen;
        if (oxygenColour >= yellowOxygen)
            oxygenFill.color = greenColour;
        else if (oxygenColour <= yellowOxygen && oxygenColour >= redOxygen)
            oxygenFill.color = yellowColor;
        else if (oxygenColour <= redOxygen)
            oxygenFill.color = redColor;
    }

    private void UpdateOxygenUI()
    {
        if (oxygenSlider != null)
        {
            oxygenSlider.value = currentOxygen;
            oxygenDisplay = Mathf.FloorToInt(currentOxygen);
            UpdateOxygenColour();
        }

        UpdateOxygenVignette();
    }

    private void UpdateOxygenVignette()
    {
        if (vignette == null)
            return;

        float oxygenPercentage = currentOxygen / maxOxygen;

        // No vignette above 50% or below 0
        if (oxygenPercentage >= 0.5f || oxygenPercentage <= 0f)
        {
            vignette.intensity.value = 0f;
            return;
        }

        // Increase vignette from 50%
        if (oxygenPercentage >= vignetteFadeOutOxygen)
        {
            float progress = Mathf.InverseLerp(
                0.5f,
                vignetteFadeOutOxygen,
                oxygenPercentage
            );

            // Exponential-style curve (I think this is better than Linear)
            progress = Mathf.Pow(progress, 0.5f);

            vignette.intensity.value = Mathf.Lerp(
                0f,
                maxVignetteIntensity,
                progress
            );
        }
        else
        {
            float progress = oxygenPercentage / vignetteFadeOutOxygen;

            vignette.intensity.value = Mathf.Lerp(
                0f,
                maxVignetteIntensity,
                progress
            );
        }
    }
}