using UnityEngine;
using System.Collections.Generic;

public class CursedMedallionItem : MonoBehaviour
{
    [SerializeField] private int damageModifier;
    [SerializeField] private float maxHealthModifier;


    public void ApplyEffects()
    {
        if (PlayerManager.instance == null)
            return;


        PlayerManager.instance.playerShip.SetMaxHealth(maxHealthModifier);

        List<Cannon> cannons = PlayerManager.instance.playerShip.GetCannons();

        foreach (var cannon in cannons)
        {
            cannon.damage += damageModifier;
        }
    }

    public void RemoveEffects()
    {
        if (PlayerManager.instance == null)
            return;


        PlayerManager.instance.playerShip.SetMaxHealth(maxHealthModifier);

        List<Cannon> cannons = PlayerManager.instance.playerShip.GetCannons();

        foreach (var cannon in cannons)
        {
            cannon.damage -= damageModifier;
        }
    }
}
