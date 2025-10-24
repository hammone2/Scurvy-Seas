using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int itemSize = 1;
    [SerializeField] private Outline outline;
    
    private bool _canBePickedUp;
    public bool canBePickedUp
    {
        get { return _canBePickedUp; }
        set 
        {
            if (value == _canBePickedUp) return;
            _canBePickedUp = value;

            if (_canBePickedUp)
                outline.OutlineWidth = 5f;
            else
                outline.OutlineWidth = 2f;
        }
    }

    public GameObject inventoryItemPrefab;

    private void Start()
    {
        canBePickedUp = false;
    }

    public void PickUpItem()
    {
        if (!canBePickedUp)
            return;

        //create inventory item and add to inventory
        InventorySystem inventory = PlayerManager.instance.inventorySystem;

        if (!inventory.HasEnoughStorage(itemSize)) //are there free cells?
            return;

        inventory.AddItem(inventoryItemPrefab);

        Destroy(gameObject);
    }
}
