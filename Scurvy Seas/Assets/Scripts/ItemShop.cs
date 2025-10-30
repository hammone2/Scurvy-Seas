using TMPro;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    [SerializeField] private Transform shopContent;
    
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
