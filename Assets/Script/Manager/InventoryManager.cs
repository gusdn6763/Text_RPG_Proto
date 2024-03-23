using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Chest
{
    public static InventoryManager instance;

    protected override void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this.gameObject);

        base.Awake();
    }

    public override void OnItemCountChanged(Item item)
    {
        base.OnItemCountChanged(item);
        currentWeight += Player.instance.GetWeight();
        weightText.text = currentWeight + " / " + fullWeight;
    }

    public override void ItemCommandChange(Item item)
    {
        item.GetItem();
    }
}
