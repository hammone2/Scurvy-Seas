using UnityEngine;

public class ItemDropSearcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemDrop"))
        {
            ItemDrop itemDrop = other.GetComponent<ItemDrop>();

            if (itemDrop.canBePickedUp)
            {
                //create inventory item and add to inventory
                InventorySystem inventory = PlayerManager.instance.inventorySystem;

                if (!inventory.IsFreeCells()) //are there free cells?
                    return;

                inventory.AddItem(itemDrop.inventoryItemPrefab);

                Destroy(other.gameObject);
            }
        }
    }
}
