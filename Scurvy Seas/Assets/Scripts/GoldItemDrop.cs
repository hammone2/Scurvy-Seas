using UnityEngine;

public class GoldItemDrop : ItemDrop
{
    private int goldAmount;

    private void Start()
    {
        goldAmount = Random.Range(25, 250);
    }

    public override void PickUpItem()
    {
        if (!canBePickedUp)
            return;

        InventorySystem.instance.gold += goldAmount;

        Destroy(gameObject);
    }
}
