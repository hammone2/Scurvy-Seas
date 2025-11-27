using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using System.Net;
using static UnityEditor.Progress;

public class ItemShop : MonoBehaviour
{
    [SerializeField] private Transform shopContent;
    [SerializeField] private LootSpawner lootSpawner;
    
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

    private List<InventoryItem> items = new List<InventoryItem>();

    private void Start()
    {
        gold = Random.Range(100, 1000);

        lootSpawner.CreateItemDrops(shopContent,true);

        for (int i = 0; i < shopContent.childCount; i++)
        {
            InventoryItem item = shopContent.GetChild(i).GetComponent<InventoryItem>();
            if (item == null)
                continue;

            item.isOwnedByShop = true;

            if (item.isStackable)
            {
                InventoryItem existingItem = FindFirstItemOfPath(item.prefabPath);
                if (existingItem != null)
                {
                    existingItem.SetStack(existingItem.stack + item.stack);
                    Destroy(item.gameObject);
                    continue;
                }
            }

            items.Add(item);
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

        bool itemCanBeDisplayed = true;

        if (item.isStackable)
        {
            InventoryItem newItem = GetSplitStack(item, inventory);
            if (newItem != null)
            {
                item = newItem;
                itemCanBeDisplayed = false; //dont display the item we just bought if we only bought a portion of the stack, keep the hop's item stack selected so you can still buy more at an incremental rate
            }
            else
            {
                items.Remove(item);
            }
        }
        else
            items.Remove(item);

        inventory.AddItem(item);
        item.transform.SetParent(inventory.GetInventoryContent());
        item.isOwnedByShop = false;

        if (itemCanBeDisplayed)
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

        if (item.isStackable)
        {
            InventoryItem newItem = GetSplitStack(item, inventory);
            if (newItem != null)
                item = newItem;
            else
                inventory.RemoveItem(item);
        }
        else
            inventory.RemoveItem(item);

        inventory.gold += item.itemValue;
        gold -= item.itemValue;

        InventoryItem existingItem = FindFirstItemOfPath(item.prefabPath);
        if (existingItem != null)
        {
            existingItem.SetStack(existingItem.stack + item.stack);
            Destroy(item.gameObject);
        }

        item.transform.SetParent(shopContent);
        item.isOwnedByShop = true;
    }

    private InventoryItem FindFirstItemOfPath(string compareString)
    {
        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i].GetComponent<InventoryItem>();

            if (item.prefabPath == compareString)
                return item;
        }
        return null;
    }

    private InventoryItem GetSplitStack(InventoryItem item, InventorySystem inventory)
    {
        int amountBought = inventory.GetSelectedStackAmount();
        if (amountBought < item.stack) // if we're not buying the entire stack, then split the item stack
        {
            item.SetStack(item.stack - amountBought);
            GameObject itemPrefab = Instantiate(Resources.Load<GameObject>(item.prefabPath));
            InventoryItem newItem = itemPrefab.GetComponent<InventoryItem>();
            newItem.SetStack(amountBought);
            item = newItem;
            return item;
        }
        return null;
    }
}
