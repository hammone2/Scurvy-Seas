using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private LootTable lootTable;

    public void CreateItemDrops(Transform _transform, bool isChild = false)
    {
        if (lootTable == null)
            return;

        List<GameObject> loot = lootTable.GenerateLoot();
        if (loot == null)
            return;
        Debug.Log(loot.Count);
        for (int i = 0; i < loot.Count; i++)
        {
            GameObject lootObj = loot[i];
            if (!isChild)
            {
                Instantiate(lootObj, _transform.position, Quaternion.identity);
                return;
            }

            //this is a child
            Instantiate(lootObj, _transform);
        }
    }
}