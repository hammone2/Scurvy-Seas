using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] GameObject itemDropPrefab;

    public string prefabPath;
    public int itemSize = 1;
    public int itemValue = 1;
    public bool isStackable = false;
    public bool isConsumable = false;
    [HideInInspector] public bool isOwnedByShop = false;
    public int stack = 0;
    [SerializeField] private string itemName;
    [SerializeField] private TextMeshProUGUI stacktext;
    [SerializeField] private TextMeshProUGUI nameText;

    public UnityEvent OnConsumed;

    public bool hasStatusEffect;
    public UnityEvent ApplyEffects;
    public UnityEvent RemoveEffects;

    private void Awake()
    {
        if (!isStackable)
            stacktext.gameObject.SetActive(false);
        else
            stacktext.SetText(stack.ToString());

        nameText.SetText(itemName);
    }

    public GameObject GetItemDropPrefab()
    {
        return itemDropPrefab;
    }

    public void OnItemClicked()
    {
        InventorySystem.instance.DisplayItem(this);
    }

    public void SetStack(int value)
    {
        if (!isStackable)
            return;

        stacktext.SetText(value.ToString());
        stack = value;

        if (stack <= 0)
        {
            if (InventorySystem.instance.GetIsShopping() == true)
            {
                if (!isOwnedByShop) { InventorySystem.instance.RemoveItem(this); }
            }
            else
            {
                InventorySystem.instance.DeleteItem(this);
            }
        }    
    }

    public string GetName()
    {
        if (isStackable)
            return itemName + " (" + stack.ToString() + ")";
        return itemName;
    }
}
