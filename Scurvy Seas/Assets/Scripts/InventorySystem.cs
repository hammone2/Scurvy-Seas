using UnityEngine;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] Vector2Int size = new Vector2Int(3, 3);
    [SerializeField] float cellSize = 2f;
    [SerializeField] float spacing = 0.1f;

    private List<GameObject> cells = new List<GameObject>();
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellContainer;
    [SerializeField] private Transform itemContainer;

    private void Start()
    {
        ArrangeCells();
    }

    private void ArrangeCells()
    {
        //remove old cells
        if (cells.Count != 0)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                GameObject cell = cells[i];
                if (cell != null)
                {
                    Destroy(cell);
                }
            }
        }

        cells.Clear();

        //create new grid
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 newPosition = new Vector3(x * cellSize + spacing, y * cellSize + spacing, 0);
                GameObject newCell = Instantiate(cellPrefab, cellContainer);
                cells.Add(newCell);
                newCell.transform.localPosition = newPosition;
            }
        }
    }

    // Gizmo drawing for the grid and cells
    private void OnDrawGizmos()
    {
        // Set the color for the grid lines (light gray)
        Gizmos.color = new Color(0.8f, 0.8f, 0.8f, 0.6f);

        // Draw the grid layout based on the size and cell dimensions
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize + spacing, y * cellSize + spacing, 0);

                // Draw a wire cube to represent each cell in the grid
                Gizmos.DrawWireCube(cellPosition, new Vector3(cellSize, cellSize, 0));
            }
        }
    }

    public Vector2Int GetGridSize()
    {
        return size;
    }

    public void ToggleInventory() //we're just toggling the rotation of the camera so the inventory is still active
    {
        Transform camera = PlayerManager.instance.inventoryCamera.transform;
        float currentYRot = camera.eulerAngles.y;

        if (currentYRot == 0f)
        {
            camera.eulerAngles = new Vector3(camera.eulerAngles.x, 180f, camera.eulerAngles.z);
        }
        else
        {
            camera.eulerAngles = new Vector3(camera.eulerAngles.x, 0f, camera.eulerAngles.z);
        }
    }

    public bool IsFreeCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            InventoryCell cell = cells[i].GetComponent<InventoryCell>();
            if (!cell.isOccupied)
            {
                return true; //we found a vacant cell
            }
        }
        return false;
    }

    public void AddItem(GameObject newItem)
    {
        //check for first avaliable vacant cell
        for (int i = 0; i < cells.Count; i++)
        {
            InventoryCell cell = cells[i].GetComponent<InventoryCell>();
            if (!cell.isOccupied)
            {
                //instance the new item and snap it to the cell
                InventoryItem inventoryItem = Instantiate(newItem, itemContainer).GetComponent<InventoryItem>();
                inventoryItem.SnapToGrid(cell.transform.localPosition);
                cell.isOccupied = true;
                return; //we found a vacant cell
            }
        }
    }

    private void OnTriggerEnter(Collider other) //this is temporary until i make a proper disposal area
    {
        if (other.CompareTag("Item"))
        {
            InventoryItem inventoryItem = other.GetComponent<InventoryItem>();
            RemoveItem(inventoryItem);
        }
    }

    public void RemoveItem(InventoryItem inventoryItem)
    {
        GameObject itemDrop = Instantiate(inventoryItem.GetItemDropPrefab());
        PlayerManager.instance.playerShip.ThrowItemOverboard(itemDrop);

        Destroy(inventoryItem.gameObject);
    }
}
