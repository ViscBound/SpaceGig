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

    private void Start()
    {
        currentOxygen = maxOxygen;

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
    }
}