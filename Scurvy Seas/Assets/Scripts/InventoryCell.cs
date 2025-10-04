using UnityEngine;

public class InventoryCell : MonoBehaviour
{
    public bool isOccupied = false;

    private void OnTriggerEnter(Collider other)
    {
        isOccupied = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isOccupied = false;
    }
}
