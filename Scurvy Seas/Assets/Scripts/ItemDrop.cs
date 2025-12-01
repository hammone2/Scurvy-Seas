using NUnit.Framework.Interfaces;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int itemSize = 1;
    [SerializeField] private int itemValue = 1;
    [SerializeField] private int stack;
    [SerializeField] private Outline outline;
    [SerializeField] private GameObject textPopup;
    
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

    public void DropItem(InventoryItem item, int stack = 0)
    {
        if (stack == 0) //no override was passed
            this.stack = item.stack;
        else
            this.stack = stack;

        inventoryItemPrefab = Resources.Load<GameObject>(item.prefabPath);
    }

    public virtual void PickUpItem()
    {
        if (!canBePickedUp)
            return;

        //create inventory item and add to inventory
        InventorySystem inventory = PlayerManager.instance.inventorySystem;

        if (!inventory.HasEnoughStorage(itemSize))
            return;

        if (stack > 0)
            inventory.PickUpItem(inventoryItemPrefab, stack);
        else
            inventory.PickUpItem(inventoryItemPrefab);

        TextPopup popup = Instantiate(textPopup, transform.position, Quaternion.identity).GetComponent<TextPopup>();
        popup.SetTextValue("Picked up " + name, 8, Color.white, 1.5f, 10f);

        Destroy(gameObject);
    }

    public (int _size, int _value) GetInfo()
    {
        return (itemSize, itemValue);
    }
}
