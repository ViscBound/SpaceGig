using UnityEngine;
using UnityEngine.UI;
using PlatformerController2D.Runtime.Scripts.Player;
using PlatformerController2D.Runtime.Scripts.StateMachine;
using PlatformerController2D.Runtime.Scripts.StateMachine.PlayerStateMachine.PlayerState;

public class OxygenSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private Image oxygenFill;
    [SerializeField] private PlayerController player;

    [Header("Oxygen Settings")]
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float walkingDrain = 2f;
    [SerializeField] private float runningDrain = 6f;
    [SerializeField] private int jumpingDrain = 4;
    [SerializeField] private float dashingDrain = 10f;

    public float currentOxygen;
    private IState previousState;
    private float oxygenColour;

    [Header("Oxygen Colours")]
    [SerializeField] private float yellowOxygen = 0.5f;
    [SerializeField] private float redOxygen = 0.25f;

    [SerializeField] private Color greenColour;
    [SerializeField] private Color yellowColor;
    [SerializeField] private Color redColor;

    private void Start()
    {
        currentOxygen = maxOxygen;

        UpdateOxygenUI();
    }

    private void Update()
    {
        if (player == null || currentOxygen <= 0f)
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
            UpdateOxygenColour();
        }
    }
}