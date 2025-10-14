using UnityEngine;

public class ItemDropSearcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemDrop"))
        {
            ItemDrop itemDrop = other.GetComponent<ItemDrop>();

            itemDrop.canBePickedUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ItemDrop"))
        {
            ItemDrop itemDrop = other.GetComponent<ItemDrop>();

            itemDrop.canBePickedUp = false;
        }
    }
}
