using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    [SerializeField] private Transform inventoryContent;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject itemInfo;
    [SerializeField] private TextMeshProUGUI itemNameInfo;
    [SerializeField] private TextMeshProUGUI currentStorageText;
    public int storageSize = 100;
    [HideInInspector] public int currentStorageUsed = 0;
    private List<InventoryItem> items = new List<InventoryItem>();
    private InventoryItem selectedItem;

    private GameObject displayItem;
    [SerializeField] private Transform itemDisplayArea;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateStorageText();
    }

    public void ToggleInventory()
    {
        if (PlayerManager.instance != null)
            PlayerManager.instance.inventoryCamera.gameObject.SetActive(!inventoryUI.activeInHierarchy);

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
        UpdateStorageText();
    }

    public void DropItemButtonClicked()
    {
        if (selectedItem == null)
            return;

        RemoveItem(selectedItem);
    }

    public void RemoveItem(InventoryItem inventoryItem)
    {
        GameObject itemDrop = Instantiate(inventoryItem.GetItemDropPrefab());
        PlayerManager.instance.playerShip.ThrowItemOverboard(itemDrop);
        
        currentStorageUsed -= inventoryItem.itemSize;
        DeleteItem(inventoryItem);
        UpdateStorageText();
    }

    private void UpdateStorageText()
    {
        currentStorageText.SetText(currentStorageUsed.ToString() + "/" + storageSize);
    }

    public void DeleteItem(InventoryItem inventoryItem)
    {
        items.Remove(inventoryItem);
        Destroy(inventoryItem.gameObject);
    }

    public T FindFirstItemOfClass<T>() where T : Component
    {
        for (int i = 0; i < items.Count; i++)
        {
            T component = items[i].GetComponent<T>();
            if (component != null) return component;
        }

        return null; //nothing found
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

            if (item.isStackable)
                newItem.Stack = item.stack;

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
            {
                InventoryItem inventoryItem = Instantiate(itemPrefab, inventoryContent).GetComponent<InventoryItem>();
                items.Add(inventoryItem);

                if (inventoryItem.isStackable)
                    inventoryItem.SetStack(itemData.Stack);

                currentStorageUsed += inventoryItem.itemSize;
                UpdateStorageText();
            }
        }
    }

    public void DisplayItem(InventoryItem item)
    {
        //if (items.Count == 0)
            //return;

        if (displayItem != null)
            Destroy(displayItem);

        selectedItem = item;

        displayItem = Instantiate(item.GetItemDropPrefab(), itemDisplayArea.position, Quaternion.identity);
        displayItem.GetComponent<ItemDrop>().canBePickedUp = false;
        displayItem.GetComponent<Outline>().enabled = false;
        displayItem.layer = LayerMask.NameToLayer("Inventory");

        itemNameInfo.SetText(item.name);
    }
}
