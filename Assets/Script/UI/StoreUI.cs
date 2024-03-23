using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : Area
{
    [SerializeField] private RectTransform createPosition;

    public void SetItemList(NPC npc)
    {
        foreach (Transform child in createPosition.transform)
            Destroy(child.gameObject);

        List<MerchantItem> itemList = npc.Items;

        for (int i = 0; i < itemList.Count;i++)
        {
            Item item = Instantiate(itemList[i].MyItem, createPosition);
            SetItem(item);

            item.transform.SetParent(createPosition);
            item.StackSize = itemList[i].Count;
            item.Price = itemList[i].Price;
        }
    }

    public override void ItemCommandChange(Item item)
    {
        item.SellingItem();
    }
}
