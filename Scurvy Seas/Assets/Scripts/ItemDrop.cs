using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [HideInInspector] public bool canBePickedUp = true;
    public GameObject inventoryItemPrefab;

    public void DropItem()
    {
        canBePickedUp = false;
        Invoke("MakePickupable", 1f);
    }

    private void MakePickupable()
    {
        canBePickedUp = true;
    }
}
