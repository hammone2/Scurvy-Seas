using UnityEngine;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private GameObject inventoryUI;
    public int storageSize = 100;
    [HideInInspector] public int currentStorageUsed = 0;
    private List<InventoryItem> items = new List<InventoryItem>();

    public void ToggleInventory() //we're just toggling the rotation of the camera so the inventory is still active
    {
        inventoryUI.SetActive(!inventoryUI.activeInHierarchy);
    }

    public bool HasEnoughStorage(int itemSize)
    {
        int sizeCheck = itemSize + currentStorageUsed;
        if (sizeCheck > storageSize)
            return false;

        return true;
    }

    public void AddItem(GameObject newItem)
    {
        InventoryItem inventoryItem = Instantiate(newItem, inventoryContent).GetComponent<InventoryItem>();
        items.Add(inventoryItem);

        currentStorageUsed += inventoryItem.itemSize;
    }

    public void RemoveItem(InventoryItem inventoryItem)
    {
        GameObject itemDrop = Instantiate(inventoryItem.GetItemDropPrefab());
        PlayerManager.instance.playerShip.ThrowItemOverboard(itemDrop);
        items.Remove(inventoryItem);
        Destroy(inventoryItem.gameObject);
    }

    public InventoryData Save()
    {
        InventoryData inventoryData = new InventoryData();
        inventoryData.Items = new InventoryItemData[items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            InventoryItemData newItem = new InventoryItemData();
            InventoryItem item = items[i];
            newItem.PrefabPath = item.prefabPath; //use asset bundles for this in future (resources becomes expensive)

            inventoryData.Items[i] = newItem;
        }

        return inventoryData;
    }

    public void Load(SaveData saveData)
    {
        InventoryData inventoryData = saveData.Inventory;

        for (int i = 0; i < inventoryData.Items.Length; i++)
        {
            InventoryItemData itemData = inventoryData.Items[i];
            GameObject itemPrefab = Resources.Load<GameObject>(itemData.PrefabPath); //use asset bundles for this in future (resources becomes expensive)
            
            if (itemPrefab != null)
                AddItem(itemPrefab);
        }
    }
}
