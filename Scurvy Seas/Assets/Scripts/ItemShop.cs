using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ItemShop : MonoBehaviour
{
    [SerializeField] private Transform shopContent;
    [SerializeField] private LootTable lootTable;
    
    //Gold stuff
    [SerializeField] private TextMeshProUGUI goldText;
    private int _gold;
    public int gold
    {
        set
        {
            if (_gold == value) return;

            _gold = value;
            goldText.SetText(_gold.ToString());
        }
        get { return _gold; }
    }

    private void Start()
    {
        gold = Random.Range(100, 1000);

        if (lootTable == null)
            return;

        List<GameObject> loot = lootTable.GenerateLoot();
        if (loot == null)
            return;
        Debug.Log(loot.Count);
        for (int i = 0; i < loot.Count; i++)
        {
            GameObject lootObj = loot[i];
            if (lootObj.GetComponent<InventoryItem>())
            {
                Instantiate(lootObj, shopContent);
            }
        }
    }

    public void BuyItem()
    {
        InventorySystem inventory = InventorySystem.instance;
        InventoryItem item = inventory.GetSelectedItem();
        if (!item)
            return;

        if (inventory.gold - item.itemValue < 0)
            return;

        inventory.AddItem(item);
        item.transform.SetParent(inventory.GetInventoryContent());
        inventory.DisplayItem(item);

        inventory.gold -= item.itemValue;
        gold += item.itemValue;
    }

    public void SellItem()
    {
        InventorySystem inventory = InventorySystem.instance;
        InventoryItem item = inventory.GetSelectedItem();
        if (!item)
            return;

        if (gold - item.itemValue < 0)
            return;

        inventory.RemoveItem(item);
        item.transform.SetParent(shopContent);

        inventory.gold += item.itemValue;
        gold -= item.itemValue;
    }
}
