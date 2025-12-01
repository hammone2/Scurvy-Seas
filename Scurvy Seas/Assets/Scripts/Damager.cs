using UnityEngine;

public class Damager : MonoBehaviour
{
    [HideInInspector] public float damage;

    public float GetDamage()
    {
        return damage;
    }
}
