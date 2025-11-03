using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] GameObject itemDropPrefab;

    public string prefabPath;
    public int itemSize = 1;
    public int itemValue = 1;
    public bool isStackable = false;
    public bool isConsumable = false;
    public int stack = 0;
    [SerializeField] private TextMeshProUGUI stacktext;

    public UnityEvent OnConsumed;

    private void Awake()
    {
        if (!isStackable)
            stacktext.gameObject.SetActive(false);
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
            PlayerManager.instance.inventorySystem.RemoveItem(this);
    }
}
