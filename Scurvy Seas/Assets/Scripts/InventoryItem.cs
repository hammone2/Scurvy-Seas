using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    private bool isDragging = false;
    private bool isInInventory = false;
    [SerializeField] float zDepth;
    [SerializeField] Transform anchorPoints;
    [SerializeField] GameObject itemDropPrefab;

    public string prefabPath;

    private void Update()
    {
        if (isDragging)
        {


            //for perspective cam (needs some work the item will fly toward the camera)
            /*Ray ray = PlayerManager.instance.inventoryCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50))
            {
                transform.position = hit.point;
            }*/



            //orthographic cam method
            Vector3 mousePosition = PlayerManager.instance.inventoryCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = zDepth;

            transform.position = mousePosition;
        }
    }

    public void ToggleDrag()
    {
        if (isDragging)
        {
            int collidingPoints = 0;

            for (int i = 0; i < anchorPoints.childCount; i++)
            {
                AnchorPoint anchorPoint = anchorPoints.GetChild(i).GetComponent<AnchorPoint>();
                if (anchorPoint.IsColliding())
                {
                    InventoryCell collidingCell = anchorPoint.GetColliderPosition().GetComponent<InventoryCell>();

                    if (collidingCell.isOccupied == false)
                    {
                        collidingPoints++;
                        collidingCell.isOccupied = true;
                    }
                }
            }

            if (collidingPoints == anchorPoints.childCount)
            {
                isDragging = !isDragging;

                AnchorPoint anchorPoint = anchorPoints.GetChild(0).GetComponent<AnchorPoint>();
                //Vector3 offset = anchorPoints.GetChild(0).transform.position - transform.position; //get the first anchor point 
                Vector3 targetPosition = anchorPoint.GetColliderPosition().localPosition;
                //transform.localPosition = targetPosition - offset;

                SnapToGrid(targetPosition);

                return;
            }
            else
            {
                //check to see if we're placing it in the discard area here
                return; //cant be placed, keep dragging
            }
        }
        else //if we're picking up the item to move
        {
            for (int i = 0; i < anchorPoints.childCount; i++)
            {
                AnchorPoint anchorPoint = anchorPoints.GetChild(i).GetComponent<AnchorPoint>();
                if (anchorPoint.IsColliding())
                {
                    InventoryCell collidingCell = anchorPoint.GetColliderPosition().GetComponent<InventoryCell>();
                    collidingCell.isOccupied = false;
                }
            }
        }
        isDragging = !isDragging;
    }

    public bool GetIsInInventory()
    {
        return isInInventory;
    }

    public GameObject GetItemDropPrefab()
    {
        return itemDropPrefab;
    }

    public void SnapToGrid(Vector3 targetPosition)
    {
        Vector3 offset = anchorPoints.GetChild(0).transform.position - transform.position; //get the first anchor point 

        transform.localPosition = targetPosition - offset;

        Debug.Log("Placed @ " + Time.time);
    }
}
