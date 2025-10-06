using UnityEngine;

public class InventoryCell : MonoBehaviour
{
    public bool isOccupied = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AnchorPointCollider"))
            isOccupied = false;
    }
}
