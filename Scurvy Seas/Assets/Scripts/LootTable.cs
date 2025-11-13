using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootTable", menuName = "Scriptable Objects/LootTable")]
public class LootTable : ScriptableObject
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private int minimumItems;
    [SerializeField] private int maximumItems;

    public List<GameObject> GenerateLoot()
    {
        if (maximumItems <= 0)
            return null;

        int itemsToGenerate = Random.Range(minimumItems,maximumItems+1);
        if (itemsToGenerate == 0)
            return null;

        List<GameObject> itemsGenerated = new List<GameObject>();

        for (int i = 0; i < itemsToGenerate; i++)
        {
            int itemIndex = Random.Range(0, items.Length);
            itemsGenerated.Add(items[itemIndex]);
        }

        return itemsGenerated;
    }
}
