using UnityEngine;

public class MagicSailItem : MonoBehaviour
{
    [SerializeField] private float maxHealthModifier;
    [SerializeField] private float maxSpeedModifier;

    public void ApplyEffects()
    {
        PlayerManager.instance.playerShip.moveSpeed += maxSpeedModifier;
        PlayerManager.instance.playerShip.SetMaxHealth(maxHealthModifier);
    }

    public void RemoveEffects()
    {
        PlayerManager.instance.playerShip.moveSpeed -= maxSpeedModifier;
        PlayerManager.instance.playerShip.SetMaxHealth(-maxHealthModifier);
    }
}
