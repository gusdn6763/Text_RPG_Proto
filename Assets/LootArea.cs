using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootArea : Area
{
    [SerializeField] private LootTable lootTable;

    private bool create = false;

    public void CreateItem()
    {
        if (create == false)
        {
            create = true;

            for(int i = 0; i < lootTable.DroppedItems.Count; i++)
            {
                SetItem(lootTable.DroppedItems[i], false);
                lootTable.DroppedItems[i].transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}
