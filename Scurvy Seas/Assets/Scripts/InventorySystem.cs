using UnityEngine;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{

    [SerializeField] Vector2Int size = new Vector2Int(3, 3);
    [SerializeField] float cellSize = 2f;
    [SerializeField] float spacing = 0.1f;

    private List<GameObject> cells = new List<GameObject>();
    private bool isActive = false;
    [SerializeField] private GameObject cellPrefab;

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
                GameObject newCell = Instantiate(cellPrefab, transform);
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

    public void ToggleInventory()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }
}
