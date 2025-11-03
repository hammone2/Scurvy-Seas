using UnityEngine;

public class ReparKitItem : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;
    public void ReparShip()
    {
        PlayerManager.instance.playerShip.Heal(healAmount);
    }
}
