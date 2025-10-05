using UnityEngine;

public class InventoryCell : MonoBehaviour
{
    private bool isOccupied = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AnchorPointCollider"))
            isOccupied = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AnchorPointCollider"))
            isOccupied = false;
    }
}
