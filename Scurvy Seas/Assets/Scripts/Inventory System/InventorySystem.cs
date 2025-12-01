using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    [SerializeField] private Transform inventoryContent;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject itemInfo;
    [SerializeField] private TextMeshProUGUI itemNameInfo;
    [SerializeField] private TextMeshProUGUI itemSizeText;
    [SerializeField] private TextMeshProUGUI itemValueText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI currentStorageText;
    public int storageSize = 100;
    [HideInInspector] public int currentStorageUsed = 0;
    private List<InventoryItem> items = new List<InventoryItem>();
    private InventoryItem selectedItem;

    private GameObject displayItem;
    [SerializeField] private Transform itemDisplayArea;
    [SerializeField] private Transform itemDisplayOffset;

    [SerializeField] private bool isShopping = false;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject sellButton;
    [SerializeField] private GameObject dropButton;
    [SerializeField] private GameObject consumeButton;


    //item stack selection stuff
    [SerializeField] private GameObject stackSelectionUI;
    [SerializeField] private TextMeshProUGUI selectedStackText;
    [SerializeField] private Slider stackSlider;
    private int selectedStackAmount;

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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateStorageText();

        if (isShopping)
            dropButton.SetActive(false);
        else
        {
            sellButton.SetActive(false);
            buyButton.SetActive(false);
        }
    }

    private void Update()
    {
        //spin the item around
        if (selectedItem != null)
        {
            itemDisplayArea.Rotate(Vector3.up * Time.deltaTime * 70f);

            float floatAmount = 0.1f; // Amount of floating
            float floatInterval = 2f; // Time interval for floating up/down
            float newYPos = floatAmount * Mathf.Sin(Time.time * floatInterval);
            itemDisplayArea.localPosition = new Vector3(itemDisplayArea.localPosition.x, newYPos, itemDisplayArea.localPosition.z);
        }
    }

    public void ToggleInventory()
    {
        if (PlayerManager.instance != null)
            PlayerManager.instance.inventoryCamera.gameObject.SetActive(!inventoryUI.activeInHierarchy);

        inventoryUI.SetActive(!inventoryUI.activeInHierarchy);
        PlayerManager.instance.ToggleShipHUD();
    }

    public bool HasEnoughStorage(int itemSize)
    {
        int sizeCheck = itemSize + currentStorageUsed;
        if (sizeCheck > storageSize)
            return false;

        return true;
    }

    public void PickUpItem(GameObject newItem, int stack = 0)
    {
        GameObject pickedUpItem = Instantiate(newItem, inventoryContent);
        InventoryItem inventoryItem = pickedUpItem.GetComponent<InventoryItem>();

        if (stack != 0)
            inventoryItem.stack = stack;

        AddItem(inventoryItem);
    }

    public void AddItem(InventoryItem item)
    {
        if (item.isStackable)
        {
            InventoryItem existingItem = FindFirstItemOfPath(item.prefabPath);

            if (existingItem != null)
            {
                existingItem.SetStack(existingItem.stack + item.stack);
                Destroy(item.gameObject);
                return;
            }
        }

        if (item.hasStatusEffect)
        {
            item.ApplyEffects?.Invoke();
        }

        items.Add(item);

        currentStorageUsed += item.itemSize;
        UpdateStorageText();
    }

    public void DropItemButtonClicked()
    {
        if (selectedItem == null)
            return;

        GameObject itemDrop = Instantiate(selectedItem.GetItemDropPrefab());
        PlayerManager.instance.playerShip.ThrowItemOverboard(itemDrop);

        if (selectedItem.isStackable)
        {
            itemDrop.GetComponent<ItemDrop>().DropItem(selectedItem, selectedStackAmount);
            selectedItem.SetStack(selectedItem.stack - selectedStackAmount); //SetStack will automatically handle item deletion so we're returning
            return;
        }

        itemDrop.GetComponent<ItemDrop>().DropItem(selectedItem);

        DeleteItem(selectedItem);
    }

    public void ConsumeButtonClick()
    {
        if (selectedItem == null)
            return;

        if (selectedItem.isConsumable == false)
            return;

        selectedItem.OnConsumed?.Invoke();

        if (selectedItem.isStackable)
        {
            selectedItem.SetStack(selectedItem.stack - 1);
            return;
        }
        
        DeleteItem(selectedItem);
    }

    public void RemoveItem(InventoryItem inventoryItem)
    {
        currentStorageUsed -= inventoryItem.itemSize;
        items.Remove(inventoryItem);
        RemoveDisplayItem();
        UpdateStorageText();

        if (inventoryItem.hasStatusEffect)
        {
            inventoryItem.RemoveEffects?.Invoke();
        }
    }

    public void DeleteItem(InventoryItem inventoryItem)
    {
        RemoveItem(inventoryItem);
        Destroy(inventoryItem.gameObject);
    }

    private void UpdateStorageText()
    {
        currentStorageText.SetText(currentStorageUsed.ToString() + "/" + storageSize);
    }

    public T FindFirstItemOfClass<T>() where T : Component
    {
        for (int i = 0; i < items.Count; i++)
        {
            T component = items[i].GetComponent<T>();
            if (component != null)
                return component;
        }

        return null; //nothing found
    }

    public InventoryItem FindFirstItemOfPath(string compareString)
    {
        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i].GetComponent<InventoryItem>();

            if (item.prefabPath == compareString)
                return item;
        }
        return null;
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

        inventoryData.Gold = gold;

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

                if (inventoryItem.hasStatusEffect)
                    inventoryItem.ApplyEffects?.Invoke();

                currentStorageUsed += inventoryItem.itemSize;
                UpdateStorageText();
            }
        }

        gold = inventoryData.Gold;
    }

    public void DisplayItem(InventoryItem item)
    {
        //if (items.Count == 0)
            //return;

        if (displayItem != null)
            Destroy(displayItem);

        selectedItem = item;
        itemInfo.SetActive(true);

        if (isShopping)
        {
            if (!items.Contains(selectedItem))
            {
                Debug.Log("This item can be bought");
                buyButton.SetActive(true);
                sellButton.SetActive(false);
            }
            else
            {
                Debug.Log("This is your item");
                buyButton.SetActive(false);
                sellButton.SetActive(true);
            }
        }
        else
        {
            if (item.isConsumable)
                consumeButton.SetActive(true);
            else
                consumeButton.SetActive(false);
        }

        if (item.isStackable)
            OnStackableItemSelected(item);
        else
            stackSelectionUI.SetActive(false);

        displayItem = Instantiate(item.GetItemDropPrefab(), itemDisplayOffset);
        displayItem.transform.localPosition = Vector3.zero;
        displayItem.GetComponent<ItemDrop>().canBePickedUp = false;
        displayItem.GetComponent<Outline>().enabled = false;
        displayItem.layer = LayerMask.NameToLayer("Inventory");

        itemNameInfo.SetText(item.GetName());
        itemSizeText.SetText("Size: "+item.itemSize.ToString());
        itemValueText.SetText("Value: "+item.itemValue.ToString());
        descriptionText.SetText(item.description);
    }

    private void OnStackableItemSelected(InventoryItem item)
    {
        stackSelectionUI.SetActive(true);
        stackSlider.value = 1;
        OnStackSliderChanged();

        stackSlider.maxValue = item.stack;
    }

    public void OnStackSliderChanged()
    {
        selectedStackText.SetText(stackSlider.value.ToString());
        selectedStackAmount = (int)stackSlider.value;
    }

    public int GetSelectedStackAmount()
    {
        return selectedStackAmount;
    }

    public void RemoveDisplayItem()
    {
        Destroy(displayItem);
        selectedItem = null;
        stackSelectionUI.SetActive(false);
        itemInfo.SetActive(false);
    }

    public InventoryItem GetSelectedItem()
    {
        return selectedItem;
    }

    public Transform GetInventoryContent()
    {
        return inventoryContent;
    }

    public bool GetIsShopping()
    {
        return isShopping;
    }
}
