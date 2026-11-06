using UnityEngine;
using PlatformerController2D.Runtime.Scripts.Player;

public enum AbilityType
{
    DoubleJump,
    WallJump,
    Dash
}

public class AbilityPickup : MonoBehaviour
{
    [SerializeField] private AbilityType abilityToUnlock;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        player.UnlockAbility(abilityToUnlock);

        Destroy(gameObject);
    }
}