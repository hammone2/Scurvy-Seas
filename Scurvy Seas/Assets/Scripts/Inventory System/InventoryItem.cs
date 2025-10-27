using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] GameObject itemDropPrefab;

    public string prefabPath;
    public int itemSize = 1;

    public GameObject GetItemDropPrefab()
    {
        return itemDropPrefab;
    }

    public void OnItemClicked()
    {
        PlayerManager.instance.inventorySystem.DisplayItem(this);
    }
}
