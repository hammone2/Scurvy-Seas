using UnityEngine;

public class ItemShop : MonoBehaviour
{
    [SerializeField] private Transform shopContent;

    public void BuyItem()
    {
        InventoryItem item = InventorySystem.instance.GetSelectedItem();
        if (!item)
            return;

        InventorySystem.instance.AddItem(item);
        item.transform.SetParent(InventorySystem.instance.GetInventoryContent());
        InventorySystem.instance.DisplayItem(item);
    }

    public void SellItem()
    {
        InventoryItem item = InventorySystem.instance.GetSelectedItem();
        if (!item)
            return;

        InventorySystem.instance.RemoveItem(item);
        item.transform.SetParent(shopContent);
    }
}
